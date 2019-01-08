using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.IO.Helpers;
using Walmart.Sgp.Infrastructure.Web.Resources;

namespace Walmart.Sgp.Infrastructure.Web.FileVault
{
    /// <summary>
    /// Serviço de domínio relacionado ao gerenciamento de arquivos em caráter temporário.
    /// </summary>
    public class FileVaultService : IFileVaultService
    {
        #region Fields
        private readonly FileVaultConfiguration m_config;
        private readonly IProcessOrderArgumentGateway m_processOrderArgumentGateway;
        private DateTime? m_lastCheck;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FileVaultService"/>.
        /// </summary>
        public FileVaultService(FileVaultConfiguration configuration, IProcessOrderArgumentGateway processOrderArgumentGateway)
        {
            m_config = configuration;
            m_lastCheck = null;
            m_processOrderArgumentGateway = processOrderArgumentGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Armazena um arquivo.
        /// </summary>
        /// <param name="file">O arquivo a ser armazenado.</param>
        /// <returns>O ticket para o arquivo.</returns>
        public FileVaultTicket Store(IFile file)
        {
            FileVaultTicket result;
            string filePath;
            int attempts = 0;

            do
            {
                result = FileVaultTicket.Create(file.FileName);
                filePath = Path.Combine(m_config.Staging, result.PartialPath);
                attempts++;
            }
            while (File.Exists(filePath) && attempts < 10);

            if (attempts == 10)
            {
                throw new InvalidOperationException(Texts.CouldNotGenerateUniqueIdentifierForFile);
            }

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            file.SaveAs(filePath);

            File.SetCreationTime(filePath, result.CreatedDate);

            return result;
        }

        /// <summary>
        /// Move o arquivo para fora da área temporária.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <param name="path">O caminho para o diretório de destino.</param>
        /// <returns>O caminho completo para o arquivo salvo.</returns>
        public string SavePermanently(FileVaultTicket ticket, string path)
        {
            string filePath = Path.Combine(m_config.Staging, ticket.PartialPath);

            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException(Texts.CouldNotFindFile);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string destination = Path.Combine(path, ticket.FileName);

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            File.Move(filePath, destination);

            File.SetCreationTime(destination, ticket.CreatedDate);

            DirectoryHelper.DeleteDirectoriesWhenEmpty(Path.GetDirectoryName(filePath), 3);

            return destination;
        }

        /// <summary>
        /// Obtém um arquivo previamente armazenado, retornando um Stream, e remove o arquivo assim que o Stream for descartado.
        /// </summary>
        /// <param name="ticket">O ticket para o arquivo.</param>
        /// <returns>O Stream para leitura do arquivo.</returns>
        /// <remarks>O arquivo é removido após o descarte do Stream.</remarks>
        public Stream Retrieve(FileVaultTicket ticket)
        {
            Cleanup();

            string filePath = Path.Combine(m_config.Staging, ticket.PartialPath);

            if (File.Exists(filePath))
            {
                return new FileVaultStream(filePath);
            }

            return null;
        }

        /// <summary>
        /// Remove arquivos antigos.
        /// </summary>
        /// <remarks>O arquivo é considerado antigo se foi armazenado há mais de 24 horas.</remarks>
        public void Cleanup()
        {
            if ((DateTime.Now - (m_lastCheck ?? DateTime.MinValue)).TotalMinutes > 1)
            {
                List<string> paths = new List<string>();

                var files = Directory.EnumerateFiles(m_config.Staging, "*", SearchOption.AllDirectories).ToList();

                files.ForEach(file =>
                {
                    DateTime fileDate = File.GetCreationTime(file);

                    if (File.Exists(file) && (DateTime.Now - fileDate).TotalHours > 24)
                    {
                        FileVaultTicket fileVaultTicket = FileVaultTicket.Deserialize(Path.GetFileName(file), fileDate);

                        if (!m_processOrderArgumentGateway.HasFileVaultTicket(fileVaultTicket))
                        {
                            File.Delete(file);

                            string thePath = Path.GetDirectoryName(file);

                            if (!paths.Contains(thePath))
                            {
                                paths.Add(thePath);
                            }
                        }
                    }
                });

                paths.ForEach(path =>
                {
                    DirectoryHelper.DeleteDirectoriesWhenEmpty(path, 3);
                });

                m_lastCheck = DateTime.Now;
            }
        }

        /// <summary>
        /// Remove um arquivo, se existe.
        /// </summary>
        /// <param name="theTicket">O ticket.</param>
        public void Discard(FileVaultTicket theTicket)
        {
            string filePath = Path.Combine(m_config.Staging, theTicket.PartialPath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                DirectoryHelper.DeleteDirectoriesWhenEmpty(Path.GetDirectoryName(filePath), 3);
            }
        }

        #endregion
    }
}

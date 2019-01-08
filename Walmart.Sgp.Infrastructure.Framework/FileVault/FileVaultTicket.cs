using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Helpers;

namespace Walmart.Sgp.Infrastructure.Framework.FileVault
{
    /// <summary>
    /// Representa um arquivo armazenado pelo FileVaultService.
    /// </summary>
    /// <remarks>
    /// Path.GetExtension(Id) deve retornar o GUID do ticket.
    /// Path.GetFileNameWithoutExtension(Id) deve retornar o nome do arquivo do ticket.
    /// CreatedDate é aplicado no arquivo físico pelo FileTicketService.
    /// TODO: Guardar a informação sobre o ticket no banco e usar apenas o GUID aqui (serializar como algo tipo fvt://guid ?)
    /// </remarks>
    public class FileVaultTicket
    {
        #region Fields
        private static char s_serializedSeparator = '|';
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FileVaultTicket"/>.
        /// </summary>
        public FileVaultTicket()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define identificador do arquivo.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Obtém ou define a data de criação.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Obtém o nome do arquivo.
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.Id);
            }
        }

        /// <summary>
        /// Obtém o caminho para o arquivo.
        /// </summary>
        public string PartialPath
        {
            get
            {
                return @"{0:yyyy}\{0:MM}\{0:dd}\{0:HH}\{1}".With(this.CreatedDate, this.Id);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Cria o ticket a partir do nome do arquivo.
        /// </summary>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <returns>O ticket.</returns>
        public static FileVaultTicket Create(string fileName)
        {
            ExceptionHelper.ThrowIfNull("fileName", fileName);

            var ticket = new FileVaultTicket();

            DateTime date = DateTime.Now;

            // corta os milisegundos
            ticket.CreatedDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            ticket.Id = @"{0}.{1}".With(Path.GetFileName(fileName), Guid.NewGuid().ToString());

            return ticket;
        }

        /// <summary>
        /// Deserializa o ticket.
        /// </summary>
        /// <param name="id">O id do ticket.</param>
        /// <param name="createdDate">A data de criação.</param>
        /// <returns>O ticket.</returns>
        public static FileVaultTicket Deserialize(string id, DateTime createdDate)
        {
            ExceptionHelper.ThrowIfNull("id", id);

            var ticket = new FileVaultTicket();
            ticket.Id = id;
            ticket.CreatedDate = createdDate;

            return ticket;
        }

        /// <summary>
        /// Deserializa o ticket.
        /// </summary>
        /// <param name="serialized">O conteúdo serializado.</param>
        /// <returns>O ticket.</returns>
        public static FileVaultTicket Deserialize(string serialized)
        {
            if (null == serialized)
            {
                return null;
            }

            var terms = serialized.Split(s_serializedSeparator);

            string id = terms[0];
            DateTime createdDate = DateTime.Parse(terms[1], System.Globalization.CultureInfo.InvariantCulture);

            return Deserialize(id, createdDate);
        }

        /// <summary>
        /// Tenta deserializar um valor como FileVaultTicket.
        /// </summary>
        /// <param name="serialized">O valor.</param>
        /// <param name="result">O FileVaultTicket, se o valor for um conteúdo válido.</param>
        /// <returns>Boolean indicando se o valor é um FileVaultTicket válido.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse(string serialized, out FileVaultTicket result)
        {
            if (string.IsNullOrWhiteSpace(serialized))
            {
                result = null;
                return false;
            }

            if (!serialized.Contains(s_serializedSeparator))
            {
                result = null;
                return false;
            }

            var temp = serialized.Split(s_serializedSeparator);

            if (temp.Length != 2)
            {
                result = null;
                return false;
            }

            var id = temp[0];

            if (string.IsNullOrWhiteSpace(id))
            {
                result = null;
                return false;
            }

            if (id.Length < 37)
            {
                result = null;
                return false;
            }

            if (id[id.Length - 37] != '.' || id[id.Length - 13] != '-' || id[id.Length - 18] != '-' || id[id.Length - 23] != '-' || id[id.Length - 28] != '-')
            {
                result = null;
                return false;
            }

            DateTime theDate;
            bool didParseDate = DateTime.TryParse(temp[1], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out theDate);

            if (!didParseDate)
            {
                result = null;
                return false;
            }

            result = Deserialize(id, theDate);

            return true;
        }

        /// <summary>
        /// Serializa um ticket.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <returns>O conteúdo serializado.</returns>
        public static string Serialize(FileVaultTicket ticket)
        {
            return "{0}{2}{1:yyyy-MM-dd HH:mm:ss.000}".With(ticket.Id, ticket.CreatedDate, s_serializedSeparator);
        }
        #endregion
    }
}

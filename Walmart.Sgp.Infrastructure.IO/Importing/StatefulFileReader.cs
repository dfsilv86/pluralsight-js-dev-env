using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Um leitor de arquivo texto que armazena seu estado entre cada linha lida.
    /// </summary>
    public class StatefulFileReader : IDisposable
    {
        #region Fields
        private StreamReader m_streamReader;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="StatefulFileReader"/>.
        /// </summary>
        /// <param name="path">O caminho para o arquivo.</param>
        public StatefulFileReader(string path)
        {
            this.FileName = path;
            this.m_streamReader = new StreamReader(path, Encoding.Default);
            this.CurrentRange = new Range(0, 0);
            NextLine();
        }

        /// <summary>
        /// Finaliza uma instância da classe <see cref="StatefulFileReader"/>.
        /// </summary>
        ~StatefulFileReader()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Obtém o caminho completo para o arquivo.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Obtém o conteúdo da última linha lida do arquivo.
        /// </summary>
        public string CurrentLine { get; private set; }

        /// <summary>
        /// Obtém o número da última linha lida do arquivo.
        /// </summary>
        public int CurrentLineNumber { get; private set; }

        /// <summary>
        /// Obtém a posição da última linha lida, dentro do arquivo, em caracteres.
        /// </summary>
        public Range CurrentRange { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Lê a próxima linha do arquivo.
        /// </summary>
        /// <returns>True caso a linha tenha sido lida com sucesso, false caso contrário (arquivo chegou no final).</returns>
        public bool NextLine()
        {
            this.CurrentLine = this.m_streamReader.ReadLine();

            if (null == this.CurrentLine)
            {
                this.CurrentRange = this.CurrentRange.Advance(0);

                return false;
            }

            this.CurrentLineNumber++;

            this.CurrentRange = this.CurrentRange.Advance(this.CurrentLine.Length + 1);

            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (null != this.m_streamReader)
                {
                    this.m_streamReader.Dispose();
                }
            }

            this.m_streamReader = null;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.FileVault
{
    /// <summary>
    /// Um FileStream que remove o arquivo ao ser fechado.
    /// </summary>
    public class FileVaultStream : FileStream
    {
        #region Fields
        private string m_path;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="FileVaultStream"/>.
        /// </summary>
        /// <param name="path">O caminho para o arquivo.</param>
        public FileVaultStream(string path)
            : base(path, FileMode.Open, FileAccess.Read, FileShare.None)
        {
            m_path = path;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.FileStream" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (File.Exists(m_path))
            {
                File.Delete(m_path);
            }
        }
        #endregion
    }
}

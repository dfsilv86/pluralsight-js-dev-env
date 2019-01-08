using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;

namespace Walmart.Sgp.WebApi.Controllers
{
    [AllowAnonymous]
    public class FileVaultController : ApiControllerBase<IFileVaultService>
    {
        public FileVaultController(IFileVaultService fileVaultService)
            : base(fileVaultService)
        {
        }

        [HttpGet]
        [Route("FileVault/Ticket/DownloadSerialized")]
        public HttpResponseMessage DownloadSerialized(string serialized)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var ticket = FileVaultTicket.Deserialize(serialized);
            var fileStream = MainService.Retrieve(ticket);

            if (fileStream == null)
            {
                throw new InvalidOperationException(Texts.FileNotFoundInFileVault);
            }

            return fileStream.AsAttachment(ticket.FileName);
        }

        [HttpGet]
        [Route("FileVault/Ticket/Download")]
        public HttpResponseMessage Download(string ticketId, DateTime ticketCreatedDate)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var ticket = FileVaultTicket.Deserialize(ticketId, ticketCreatedDate);
            var fileStream = MainService.Retrieve(ticket);

            if (fileStream == null)
            {
                throw new InvalidOperationException(Texts.FileNotFoundInFileVault);
            }

            return fileStream.AsAttachment(ticket.FileName);
        }
    }
}
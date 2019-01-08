using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.Controllers
{
    public class ProcessingController : ApiControllerBase<IProcessingService>
    {
        public ProcessingController(IProcessingService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("Processing/")]
        public IEnumerable<ProcessOrder> Find(string processName, ProcessOrderState? state, [FromUri]Paging paging)
        {
            var result = this.MainService.FindAllByUser(RuntimeContext.Current.User.Id, processName, state, paging);

            return result;
        }

        [HttpGet]
        [Route("Processing/ProcessNames/")]
        public IEnumerable<object> GetProcessNames([FromUri]int? createdUserId)
        {
            var result = this.MainService.GetProcessNames(createdUserId);

            return result;
        }

        [HttpGet]
        [Route("Processing/Notifications/")]
        public IEnumerable<ProcessOrder> Find(DateTime? lastCheck)
        {
            var result = this.MainService.CheckNotifications(lastCheck);

            return result;
        }

        [HttpGet]
        [Route("Processing/AllUsers/")]
        public IEnumerable<ProcessOrder> FindAll(int? userId, string processName, ProcessOrderState? state, [FromUri]Paging paging)
        {
            var result = this.MainService.FindAllByUser(userId, processName, state, paging);

            return result;
        }

        [HttpGet]
        [Route("Processing/Ticket/{ticket}")]
        public ProcessOrderSummary Get(string ticket)
        {
            var result = this.MainService.GetByTicket(ticket);

            return result;
        }

        /*
        [HttpPost]
        [Route("Processing/Ticket/{ticket}/ExecuteImmediately")]
        public ProcessOrder Run(string ticket)
        {
            var result = this.MainService.Run(ticket);

            Commit();

            return result;
        }
        */

        [HttpGet]
        [Route("Processing/Ticket/{ticket}/Details")]
        public ProcessOrder Detail(string ticket)
        {
            var result = this.MainService.GetDetailsByTicket(ticket);

            return result;
        }

        [HttpGet]
        [Route("Processing/Ticket/{ticket}/Logs")]
        public IEnumerable<AuditRecord<ProcessOrder>> Logs(string ticket, [FromUri]Paging paging)
        {
            var result = this.MainService.GetLogsByTicket(ticket, paging);

            return result;
        }

        [HttpGet]
        [Route("Processing/Ticket/{ticket}/Results")]
        public ProcessOrderResult Results(string ticket)
        {
            var result = this.MainService.GetProcessingResults(ticket);

            return result;
        }
    }
}
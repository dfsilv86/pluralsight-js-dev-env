using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.Processing.Specs
{
    /// <summary>
    /// Spec que valida um ProcessOrder
    /// </summary>
    public class ProcessOrderSpec : SpecBase<ProcessOrder>
    {
        /// <summary>
        /// Verifica se o process order informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O process order.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo process order.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ProcessOrder target)
        {
            var all = new AllMustBeInformedSpec();

            // Ordem recem criada precisa destes campos
            var result = all.IsSatisfiedBy(new 
            {
                target.ProcessName, 
                target.CreatedDate, 
                target.CreatedUserId,
                target.Service, 
                target.Service.ServiceMethodName, 
                target.Service.ServiceTypeName, 
                target.Ticket, 
                target.CreatedMachineName,
                target.Service.RoleId
            });

            if (!result.Satisfied)
            {
                return result;
            }

            // Ordem enfileirada precisa destes campos
            if ((int)target.State > (int)ProcessOrderState.Created)
            {
                result = all.IsSatisfiedBy(new { target.ModifiedDate, target.ModifiedUserId });

                if (!result.Satisfied)
                {
                    return result;
                }
            }

            // Ordem em execução ou em erro precisa destes campos
            if ((int)target.State > (int)ProcessOrderState.Queued || target.State == ProcessOrderState.Error)
            {
                result = new AllMustBeInformedSpec(true).IsSatisfiedBy(new { target.WorkerName });

                if (!result.Satisfied)
                {
                    return result;
                }
            }

            // Ordem em execução precisa destes campos (como é possível transitar de um estado de erro para estado de expurgo, expurgo nao precisa de startdate)
            if ((int)target.State > (int)ProcessOrderState.Queued && target.State != ProcessOrderState.ResultsExpunged)
            {
                result = new AllMustBeInformedSpec(true).IsSatisfiedBy(new { target.StartDate });

                if (!result.Satisfied)
                {
                    return result;
                }
            }

            // Ordem executada e finalizada com sucesso ou falha de negocio, ou com resultado disponivel, precisa destes campos
            if (target.State == ProcessOrderState.Failed || target.State == ProcessOrderState.Finished || target.State == ProcessOrderState.ResultsAvailable)
            {
                result = all.IsSatisfiedBy(new { target.EndDate });

                if (!result.Satisfied)
                {
                    return result;
                }
            }

            // Ordem executada de imediato e finalizada com sucesso e resultado disponivel, precisa destes campos
            if (target.State == ProcessOrderState.ResultsAvailable && target.WorkerName != ProcessOrder.ImmediateWorkerName)
            {
                result = all.IsSatisfiedBy(new { target.Service.ResultFilePath, target.Service.ResultTypeFullName });

                if (!result.Satisfied)
                {
                    return result;
                }
            }

            return Satisfied();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Processing.Specs;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.IO.Helpers;

namespace Walmart.Sgp.Infrastructure.IO.Processing
{
    /// <summary>
    /// Serviço de domínio relacionado a background process.
    /// </summary>
    public class ProcessingService : DomainServiceBase<IProcessOrderGateway>, IProcessingService
    {
        #region Fields
        private static readonly string[] s_propertiesToLog = new string[] { "Ticket", "ProcessName", "State", "CurrentProgress", "TotalProgress", "Message"/*, "ResultTypeFullName", "ResultFilePath"*/ };

        private readonly Func<Type, object> m_resolver;
        private readonly IAuditService m_auditService;
        private readonly IFileVaultService m_fileVaultService;
        private readonly ProcessingConfiguration m_processingConfiguration;
        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProcessingService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        /// <param name="processingConfiguration">A configuração do serviço.</param>
        /// <param name="resolver">O resolvedor de dependência de serviços (configurado no LightInject da WebApi.)</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        /// <param name="fileVaultService">O serviço de file vault.</param>
        public ProcessingService(IProcessOrderGateway mainGateway, ProcessingConfiguration processingConfiguration, Func<Type, object> resolver, IAuditService auditService, IFileVaultService fileVaultService)
            : base(mainGateway)
        {
            m_resolver = resolver;
            m_auditService = auditService;
            m_fileVaultService = fileVaultService;
            m_processingConfiguration = processingConfiguration;
        }

        #endregion

        #region Methods

        #region Dispatch overloads

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <param name="serviceType">O tipo do serviço, conforme registrado na injeção de dependência.</param>
        /// <param name="methodName">O nome do método do serviço.</param>
        /// <param name="parameters">Os parâmetros do serviço.</param>
        /// <param name="exposedParameterNames">A lista de parâmetros considerados expostos (que podem ser exibidos em tela em uma visualização rápida do pedido de execução).</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        public ProcessOrderResult Dispatch(Type serviceType, string methodName, IReadOnlyDictionary<string, object> parameters, IReadOnlyList<string> exposedParameterNames)
        {
            var serviceInstance = m_resolver(serviceType);

            if (null == serviceInstance)
            {
                throw new InvalidOperationException(Texts.ServiceLookupFailed.With(serviceType.Name));
            }

            var parameterNames = parameters.GetParameterNames();

            var methodInfo = FindServiceMethod(serviceType, methodName, parameterNames, parameters);

            string serviceTypeName = "{0}, {1}".With(serviceType.FullName, serviceType.Assembly.FullName.Split(',')[0]);
            string serviceMethodName = methodInfo.Name;

            ProcessOrder processOrder = new ProcessOrder();

            processOrder.Message = Texts.ProcessOrderStateCreatedMessage;
            processOrder.Service.ServiceTypeName = serviceTypeName;
            processOrder.Service.ServiceMethodName = serviceMethodName;
            processOrder.Service.RoleId = RuntimeContext.Current.User.RoleId;
            processOrder.Service.StoreId = RuntimeContext.Current.User.StoreId;
            processOrder.Service.BandeiraId = RuntimeContext.Current.User.BandeiraId;
            processOrder.CreatedMachineName = Environment.MachineName;

            ServiceProcessAttribute serviceProcessAttribute = (ServiceProcessAttribute)methodInfo.GetCustomAttributes(typeof(ServiceProcessAttribute), true).FirstOrDefault() ?? new ServiceProcessAttribute(methodName);

            var processName = serviceProcessAttribute.ProcessName;

            processOrder.ProcessName = processName;
            processOrder.Service.MaxGlobal = serviceProcessAttribute.MaxGlobal;
            processOrder.Service.MaxPerUser = serviceProcessAttribute.MaxPerUser;

            foreach (var theArg in parameters.ToArgumentList(exposedParameterNames, fvt => MoveToInputFile(processOrder, fvt)))
            {
                processOrder.Arguments.Add(theArg);
            }

            processOrder.Stamp();

            Assert(processOrder, new ProcessOrderSpec());

            this.MainGateway.Insert(processOrder);

            m_auditService.LogInsert(processOrder, s_propertiesToLog);

            object theResult = null;

            bool runImmediately = !m_processingConfiguration.EnableQueueing || !serviceProcessAttribute.EnableQueueing;

            if (runImmediately)
            {
                processOrder.WorkerName = ProcessOrder.ImmediateWorkerName;

                theResult = this.InternalRun(processOrder);
            }
            else
            {
                UpdateOrderSetQueued(processOrder);
            }

            return new ProcessOrderResult(theResult, processOrder);
        }

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TService">O tipo do serviço, conforme registrado na injeção de dependência.</typeparam>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        public ProcessOrderResult<TResult> Dispatch<TService, TResult>(Expression<Func<TService, TResult>> serviceCall)
        {
            return Dispatch<TService, TResult>(serviceCall, null);
        }

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TService">O tipo do serviço, conforme registrado na injeção de dependência.</typeparam>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <param name="exposedParameterNames">A lista de parâmetros considerados expostos (que podem ser exibidos em tela em uma visualização rápida do pedido de execução).</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        public ProcessOrderResult<TResult> Dispatch<TService, TResult>(Expression<Func<TService, TResult>> serviceCall, IReadOnlyList<string> exposedParameterNames)
        {
            Type serviceType = typeof(TService);

            var methodCall = serviceCall.Body as MethodCallExpression;

            if (null == methodCall)
            {
                throw new InvalidOperationException(Texts.CouldNotDetermineServiceMethod);
            }

            return InternalDispatchByMethodCall<TResult>(exposedParameterNames, serviceType, methodCall);
        }

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        public ProcessOrderResult<TResult> Dispatch<TResult>(Expression<Func<TResult>> serviceCall)
        {
            return Dispatch<TResult>(serviceCall, null);
        }

        /// <summary>
        /// Dispara a execução de um serviço, executando imediatamente ou enfileirando para execução posterior, conforme configurações do serviço e da aplicação.
        /// </summary>
        /// <typeparam name="TResult">O tipo do retorno do serviço.</typeparam>
        /// <param name="serviceCall">A chamada do serviço.</param>
        /// <param name="exposedParameterNames">A lista de parâmetros considerados expostos (que podem ser exibidos em tela em uma visualização rápida do pedido de execução).</param>
        /// <returns>O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.</returns>
        public ProcessOrderResult<TResult> Dispatch<TResult>(Expression<Func<TResult>> serviceCall, IReadOnlyList<string> exposedParameterNames)
        {
            var expression = serviceCall.Body;

            var visitor = new ServiceMethodCallExpressionVisitor();

            visitor.Visit(expression);

            var serviceMethodCall = visitor.ServiceMethodCallExpression;

            Type serviceType = FindServiceType(serviceMethodCall.Method.DeclaringType);

            return InternalDispatchByMethodCall<TResult>(exposedParameterNames, serviceType, serviceMethodCall);
        }

        #endregion

        /// <summary>
        /// Obtém os processamentos que devem aparecer na área de notificação.
        /// </summary>
        /// <param name="lastCheck">A data da última verificação.</param>
        /// <returns>Os processamentos que devem aparecer na área de notificação.</returns>
        public IEnumerable<ProcessOrder> CheckNotifications(DateTime? lastCheck)
        {
            var userId = RuntimeContext.Current.User.Id;

            Assert(new { userId }, new AllMustBeInformedSpec());

            return this.MainGateway.CheckNotifications(userId, lastCheck);
        }

        /// <summary>
        /// Obtém o resumo do processamento.
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>
        /// O processamento.
        /// </returns>
        public ProcessOrder GetDetailsByTicket(string ticket)
        {
            Assert(new { ticket }, new AllMustBeInformedSpec());

            ProcessOrder processOrder = this.MainGateway.GetProcessOrderDetail(ticket);

            return processOrder;
        }

        /// <summary>
        /// Obtém o resumo do processamento.
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>
        /// O processamento.
        /// </returns>
        public ProcessOrderSummary GetByTicket(string ticket)
        {
            Assert(new { ticket }, new AllMustBeInformedSpec());

            ProcessOrder processOrder = this.MainGateway.Find("Ticket = @Ticket", new { ticket }).SingleOrDefault();

            return processOrder.Summarize();
        }

        /// <summary>
        /// Obtém o próximo ticket para executar, marcando com o nome do worker.
        /// </summary>
        /// <param name="workerName">O nome do worker que irá executar o processo.</param>
        /// <param name="serviceTypeName">O nome do tipo de serviço que pode ser executado.</param>
        /// <param name="serviceMethodName">O nome do método de serviço que pode ser executado.</param>
        /// <param name="createdUserId">O id do usuário criador dos processamentos que podem ser executados.</param>
        /// <param name="machineName">O nome do servidor que irá executar o processo.</param>
        /// <returns>O ticket.</returns>
        public string GetNextTicketToRun(string workerName, string serviceTypeName, string serviceMethodName, int? createdUserId, string machineName)
        {
            return this.MainGateway.GetNextTicketToRun(workerName, serviceTypeName, serviceMethodName, createdUserId, machineName);
        }

        /// <summary>
        /// Dispara a execução do processamento.
        /// </summary>
        /// <param name="ticket">O ticket do processo.</param>
        /// <returns>O processamento atualizado.</returns>
        public ProcessOrder Run(string ticket)
        {
            ProcessOrder processOrder = GetDetailsByTicket(ticket);

            Assert(processOrder, new ProcessOrderCanBeExecutedSpec());

            var result = this.InternalRun(processOrder);

            try
            {
                string fileName = null;

                if (null != result)
                {
                    string serviceMethodName = processOrder.Service.ServiceMethodName;

                    fileName = @"{1:yyyy}\{1:MM}\{1:dd}\{1:HH}\{4}\SerializedResult_{3}.bpo".With(processOrder.CreatedUserId, processOrder.CreatedDate, DateTime.Now, serviceMethodName, processOrder.Ticket);

                    processOrder.Service.ResultTypeFullName = "{0}, {1}".With(result.GetType().FullName, result.GetType().Assembly.FullName.Split(',')[0]);
                    processOrder.Service.ResultFilePath = Path.Combine(m_processingConfiguration.ResultsPath, fileName);

                    var directoryName = Path.GetDirectoryName(processOrder.Service.ResultFilePath);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    processOrder.Message = Texts.ProcessOrderPreparingResultsMessage;

                    UpdateProgress(processOrder);
                }

                if (null != result)
                {
                    var fvt = result as FileVaultTicket;

                    if (null != fvt)
                    {
                        fileName = @"{1:yyyy}\{1:MM}\{1:dd}\{1:HH}\{2}\{0}".With(fvt.FileName, processOrder.CreatedDate, processOrder.Ticket);
                        processOrder.Service.ResultFilePath = Path.Combine(m_processingConfiguration.ResultsPath, fileName);

                        m_fileVaultService.SavePermanently(fvt, Path.GetDirectoryName(processOrder.Service.ResultFilePath));
                    }
                    else
                    {
                        using (FileStream fstream = new FileStream(processOrder.Service.ResultFilePath, FileMode.CreateNew, FileAccess.Write))
                        {
                            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(result.GetType());
                            ser.Serialize(fstream, result);
                            fstream.Flush();
                            fstream.Close();
                        }
                    }

                    UpdateOrderSetResultsAvailable(processOrder);
                }
            }
            catch (Exception ex)
            {
                UpdateOrderSetError(processOrder, ex, ProcessOrderState.Failed);
            }

            return processOrder;
        }

        /// <summary>
        /// Obtém o resultado do processamento, caso este tenha terminado sua execução (com ou sem erros).
        /// </summary>
        /// <param name="ticket">O ticket de processamento.</param>
        /// <returns>
        /// O BackgroundProcessResult contendo informações sobre a execução do serviço, e o resultado caso disponível.
        /// </returns>
        public ProcessOrderResult GetProcessingResults(string ticket)
        {
            ProcessOrder processOrder = GetDetailsByTicket(ticket);

            if (null == processOrder)
            {
                return null;
            }

            if (processOrder.State == ProcessOrderState.ResultsAvailable)
            {
                var resultType = Type.GetType(processOrder.Service.ResultTypeFullName);
                object result;

                if (resultType == typeof(FileVaultTicket))
                {
                    result = m_fileVaultService.Store(new FileWrapper(processOrder.Service.ResultFilePath));
                }
                else
                {
                    using (FileStream fstream = new FileStream(processOrder.Service.ResultFilePath, FileMode.Open, FileAccess.Read))
                    {
                        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(resultType);

                        result = Convert.ChangeType(ser.Deserialize(fstream), resultType, System.Globalization.CultureInfo.InvariantCulture);

                        fstream.Close();
                    }
                }

                return new ProcessOrderResult(result, processOrder);
            }
            else
            {
                return new ProcessOrderResult(null, processOrder);
            }
        }

        /// <summary>
        /// Obtém todos os processamentos do usuário.
        /// </summary>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        public IEnumerable<ProcessOrderModel> FindAllByUser(int? createdUserId, string processName, ProcessOrderState? state, Paging paging)
        {
            return this.MainGateway.FindAllByUser(RuntimeContext.Current.User.Id, RuntimeContext.Current.User.IsAdministrator, createdUserId, processName, state, paging);
        }

        /// <summary>
        /// Obtém os logs do processamento.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os logs.</returns>
        public IEnumerable<AuditRecord<ProcessOrder>> GetLogsByTicket(string ticket, Paging paging)
        {
            var processOrder = GetByTicket(ticket);

            return m_auditService.ObterRelatorio<ProcessOrder>(s_propertiesToLog, null, processOrder.Id, null, null, paging);
        }

        /// <summary>
        /// Obtém uma lista de nomes de processos conhecidos.
        /// </summary>
        /// <param name="createdUserId">O id do usuário criador.</param>
        /// <returns>
        /// Os nomes de processos e suas respectivas descrições.
        /// </returns>
        public IEnumerable<object> GetProcessNames(int? createdUserId)
        {
            return this.MainGateway
                .Find("ProcessName", "@CreatedUserId IS NULL OR CreatedUserId=@CreatedUserId", new { CreatedUserId = createdUserId })
                .Select(order => new
                {
                    Value = order.ProcessName,
                    Description = GlobalizationHelper.GetText("Process{0}".With(order.ProcessName), true)
                }).Distinct();
        }

        /// <summary>
        /// Remove ordens de execução antigas.
        /// </summary>
        /// <param name="createdMachineName">O nome do servidor, ou null para todos servidores.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Cleanup(string createdMachineName)
        {
            var toBeRemoved = this.MainGateway.FindAllReadyToBeRemoved(DateTime.Today - m_processingConfiguration.MaxAge, createdMachineName);

            IRuntimeContext startingContext = RuntimeContext.Current;

            foreach (var processOrder in toBeRemoved)
            {
                try
                {
                    var createdUserId = processOrder.CreatedUserId;

                    RuntimeContext.Current = new MemoryRuntimeContext() { User = new MemoryRuntimeUser { Id = createdUserId } };

                    this.Delete(processOrder.Ticket);
                }
                catch (Exception ex)
                {
                    LogService.Error("ProcessOrder: unable to remove {0} ({1}).", processOrder.Ticket, ex.Message);
                }
                finally
                {
                    RuntimeContext.Current = startingContext;
                }
            }
        }

        /// <summary>
        /// Localiza o método do serviço a partir das informações do processamento.
        /// </summary>
        /// <param name="serviceType">O tipo do serviço.</param>
        /// <param name="methodName">O nome do método.</param>
        /// <param name="parameterNames">Os nomes dos parâmetros disponíveis no processamento.</param>
        /// <param name="parameters">Os valores dos parâmetros disponíveis no processamento.</param>
        /// <returns>O MethodInfo do método de serviço.</returns>
        private static System.Reflection.MethodInfo FindServiceMethod(Type serviceType, string methodName, IEnumerable<string> parameterNames, IReadOnlyDictionary<string, object> parameters)
        {
            return (from method in serviceType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    where method.Name == methodName
                          && method.GetParameters()
                              .Select(pi => new { ParameterName = pi.Name, ParameterType = pi.ParameterType })
                              .Where(pd => !parameterNames.Contains(pd.ParameterName) || (null != parameters && parameters[pd.ParameterName] != null && !pd.ParameterType.IsAssignableFrom(parameters[pd.ParameterName].GetType())))
                              .Count() == 0
                    select method).Single();
        }

        /// <summary>
        /// Identifica a chamada de serviço e seus parâmetros via Expression, e cria o processamento.
        /// </summary>
        /// <typeparam name="TResult">O tipo do retorno.</typeparam>
        /// <param name="exposedParameterNames">Os parâmetros expostos (visíveis em tela).</param>
        /// <param name="serviceType">O tipo do serviço.</param>
        /// <param name="methodCall">A expressão que representa a chamada do método de serviço.</param>
        /// <returns>O resultado.</returns>
        private ProcessOrderResult<TResult> InternalDispatchByMethodCall<TResult>(IReadOnlyList<string> exposedParameterNames, Type serviceType, MethodCallExpression methodCall)
        {
            var methodName = methodCall.Method.Name;
            ////var returnType = methodCall.Method.ReturnType;
            var parameters = methodCall.Method.GetParameters();

            Dictionary<string, object> arguments = new Dictionary<string, object>();
            List<string> defaultExposedParameters = new List<string>();

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var parameterName = parameter.Name;
                var parameterType = parameter.ParameterType;
                var argument = methodCall.Arguments[i];
                var argumentLambda = LambdaExpression.Lambda<Func<object>>(argument, null);
                var argumentDelegate = argumentLambda.Compile();
                var argumentValue = Convert.ChangeType(argumentDelegate(), parameterType, System.Globalization.CultureInfo.InvariantCulture);
                ExposedParameterAttribute epa = parameter.GetCustomAttributes(typeof(ExposedParameterAttribute), true).Cast<ExposedParameterAttribute>().FirstOrDefault() ?? ExposedParameterAttribute.Default;

                arguments[parameterName] = argumentValue;

                if (epa.IsExposed)
                {
                    defaultExposedParameters.Add(parameterName);
                }
            }

            var bgr = Dispatch(serviceType, methodName, arguments, exposedParameterNames ?? defaultExposedParameters.ToArray());

            return new ProcessOrderResult<TResult>(bgr);
        }

        /// <summary>
        /// Dispara a chamada para o serviço.
        /// </summary>
        /// <param name="processOrder">O processamento.</param>
        /// <returns>O retorno do serviço, ou nulo em caso de erro (detalhes do erro serão adicionados no próprio processamento).</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private object InternalRun(ProcessOrder processOrder)
        {
            System.Reflection.MethodInfo methodInfo = null;
            object serviceInstance = null;
            object[] arguments = null;
            object result = null;

            bool success = false;

            try
            {
                var methodName = processOrder.Service.ServiceMethodName;
                var serviceTypeName = processOrder.Service.ServiceTypeName;

                Type serviceType = Type.GetType(serviceTypeName);

                serviceInstance = m_resolver(serviceType);

                string[] parameterNames = processOrder.Arguments.GetParameterNames().ToArray();

                methodInfo = FindServiceMethod(serviceType, methodName, parameterNames, null);
                var parameterTypes = methodInfo.GetParameters().ToDictionary(x => x.Name, x => x.ParameterType);

                var parameters = processOrder.Arguments.ToServiceParameters(parameterTypes, inputFilePath => TicketForInputFile(inputFilePath));

                arguments = (from parameter in methodInfo.GetParameters()
                             select parameters.ContainsKey(parameter.Name) ? parameters[parameter.Name] : null).ToArray();

                processOrder.TotalProgress = 1;
                processOrder.CurrentProgress = 0;
                UpdateOrderSetIsExecuting(processOrder);
            }
            catch (Exception ex)
            {
                UpdateOrderSetError(processOrder, ex, ProcessOrderState.Error);
                return null;
            }

            List<bool> semaphore = new List<bool>();
            semaphore.Add(true);

            try
            {
                // Aqui é usado um lambda para capturar as variáveis locais semaphore, processOrder, e a instancia atual que possui o UpdateProgress()
                ProcessingContext.Current = new MemoryProcessingContext((currentProgress, totalProgress, message) =>
                {
                    // verifica se o contexto capturado ainda está executando (não passou pelo finally deste try block)
                    if (semaphore.Count == 0)
                    {
                        return;
                    }

                    if (currentProgress.HasValue)
                    {
                        processOrder.CurrentProgress = currentProgress.Value;
                    }

                    if (totalProgress.HasValue)
                    {
                        processOrder.TotalProgress = totalProgress.Value;
                    }

                    if (null != message)
                    {
                        processOrder.Message = message;
                    }

                    if (currentProgress.HasValue || totalProgress.HasValue || null != message)
                    {
                        UpdateProgress(processOrder);
                    }
                });

                result = methodInfo.Invoke(serviceInstance, arguments);

                success = true;
            }
            catch (Exception ex)
            {
                UpdateOrderSetError(processOrder, ex.GetBaseException(), ProcessOrderState.Failed);
                return null;
            }
            finally
            {
                semaphore.Clear();
                ProcessingContext.Current = new MemoryProcessingContext(null);
            }

            try
            {
                if (success)
                {
                    processOrder.CurrentProgress = processOrder.TotalProgress;

                    UpdateOrderSetFinished(processOrder);
                }
            }
            catch (Exception ex)
            {
                UpdateOrderSetError(processOrder, ex, ProcessOrderState.Error);
                return null;
            }

            return result;
        }

        /// <summary>
        /// Localiza o tipo do serviço que deve ser solicitado ao LightInject a partir do tipo concreto, ou utilizado em uma Expression, do serviço.
        /// </summary>
        /// <param name="type">O tipo do serviço.</param>
        /// <returns>O tipo do serviço conforme o LightInject.</returns>
        private Type FindServiceType(Type type)
        {
            if (null != m_resolver(type))
            {
                return type;
            }

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (null != m_resolver(interfaceType))
                {
                    return interfaceType;
                }
            }

            return null;
        }

        /// <summary>
        /// Remove a ordem de execução.
        /// </summary>
        /// <param name="ticket">O ticket.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Delete(string ticket)
        {
            var processOrder = this.GetDetailsByTicket(ticket);

            UpdateOrderSetResultsExpunged(processOrder);

            foreach (var argument in processOrder.Arguments)
            {
                FileVaultTicket theTicket;
                if (FileVaultTicket.TryParse(argument.Value, out theTicket))
                {
                    try
                    {
                        m_fileVaultService.Discard(theTicket);

                        string directoryName = GetInputDirectory(processOrder, theTicket);
                        string filePath = Path.Combine(directoryName, theTicket.FileName);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        DirectoryHelper.DeleteDirectoriesWhenEmpty(directoryName, 5);
                    }
                    catch
                    {
                        UpdateOrderSetCouldNotRemoveFile(processOrder, theTicket);
                    }
                }
            }

            if (null != processOrder.Service && !string.IsNullOrWhiteSpace(processOrder.Service.ResultFilePath))
            {
                try
                {
                    File.Delete(processOrder.Service.ResultFilePath);
                    DirectoryHelper.DeleteDirectoriesWhenEmpty(Path.GetDirectoryName(processOrder.Service.ResultFilePath), 4);
                }
                catch
                {
                    UpdateOrderSetCouldNotRemoveFile(processOrder, processOrder.Service.ResultFilePath);
                }
            }

            m_auditService.LogDelete(processOrder, s_propertiesToLog);

            MainGateway.Delete(processOrder.Id);
        }

        private string MoveToInputFile(ProcessOrder processOrder, FileVaultTicket fvt)
        {
            string directoryName = GetInputDirectory(processOrder, fvt);

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            string filePath = Path.Combine(directoryName, fvt.FileName);

            if (File.Exists(filePath))
            {
                throw new InvalidOperationException(Texts.IntermediateFileAlreadyExists.With(filePath));
            }

            return m_fileVaultService.SavePermanently(fvt, directoryName);
        }

        private FileVaultTicket TicketForInputFile(string filePath)
        {
            return m_fileVaultService.Store(new FileWrapper(filePath));
        }

        private string GetInputDirectory(ProcessOrder processOrder, FileVaultTicket fvt)
        {
            return Path.Combine(m_processingConfiguration.InputFilesPath, @"{0:yyyy}\{0:MM}\{0:dd}\{0:HH}\{1}\{2}".With(processOrder.CreatedDate, processOrder.Ticket, Path.GetExtension(fvt.Id).Substring(1)));
        }

        #region Atualização de estado

        private void UpdateProgress(ProcessOrder processOrder)
        {
            // Aguarda 2ms para que o log não fique no mesmo horário+milissegundo e gere relatorio fora de ordem
            System.Threading.Thread.Sleep(2);

            processOrder.StampUpdate();

            Assert(processOrder, new ProcessOrderSpec());

            this.MainGateway.Update(processOrder);

            m_auditService.LogUpdate(processOrder, s_propertiesToLog);
        }

        private void UpdateOrderSetQueued(ProcessOrder processOrder)
        {
            processOrder.Message = Texts.ProcessOrderStateQueuedMessage;
            processOrder.State = ProcessOrderState.Queued;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Queued {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName);
        }

        private void UpdateOrderSetIsExecuting(ProcessOrder processOrder)
        {
            processOrder.Message = Texts.ProcessOrderStateIsExecutingMessage;
            processOrder.State = ProcessOrderState.IsExecuting;
            processOrder.StartDate = DateTime.Now;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Is executing {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName);
        }

        private void UpdateOrderSetError(ProcessOrder processOrder, Exception ex, ProcessOrderState failureType)
        {
            if (failureType == ProcessOrderState.Failed)
            {
                processOrder.Message = ex.Message;
                processOrder.EndDate = DateTime.Now;
            }
            else
            {
                processOrder.Message = GlobalizationHelper.GetText("ProcessOrderState{0}Message".With(failureType.ToString())).With((null != ex) ? ex.Message : Texts.NotDefined, (null != ex) ? ex.StackTrace : Texts.NotDefined);
            }

            processOrder.State = failureType;
            UpdateProgress(processOrder);

            LogService.Error("ProcessOrder: Error in {0} (state {4}, id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName, failureType.ToString());

            if (null != ex)
            {
                LogService.Error("ProcessOrder: Error in {0} - Exception type: {1}", processOrder.Ticket, ex.GetType().FullName);
                LogService.Error("ProcessOrder: Error in {0} - Exception message: {1}", processOrder.Ticket, ex.Message);
                LogService.Error("ProcessOrder: Error in {0} - StackTrace: {1}", processOrder.Ticket, ex.StackTrace);
            }
        }

        private void UpdateOrderSetFinished(ProcessOrder processOrder)
        {
            processOrder.Message = Texts.ProcessOrderStateFinishedMessage;
            processOrder.State = ProcessOrderState.Finished;
            processOrder.EndDate = DateTime.Now;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Finished {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName);
        }

        private void UpdateOrderSetResultsAvailable(ProcessOrder processOrder)
        {
            processOrder.Message = Texts.ProcessOrderStateResultsAvailableMessage;
            processOrder.State = ProcessOrderState.ResultsAvailable;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Results for {0} are available at {4} (type {5}) (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName, processOrder.Service.ResultFilePath, processOrder.Service.ResultTypeFullName);
        }

        private void UpdateOrderSetResultsExpunged(ProcessOrder processOrder)
        {
            processOrder.Message = Texts.ProcessOrderStateResultsExpungedMessage;
            processOrder.State = ProcessOrderState.ResultsExpunged;
            processOrder.WorkerName = processOrder.WorkerName ?? ProcessOrder.ExpungeWorkerName;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Expunging {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName);
        }

        private void UpdateOrderSetCouldNotRemoveFile(ProcessOrder processOrder, FileVaultTicket fileVaultTicket)
        {
            processOrder.Message = Texts.CouldNotRemoveProcessOrderFile.With(fileVaultTicket.FileName);
            processOrder.State = ProcessOrderState.Error;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Failed to remove file \"{4}\" of {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName, fileVaultTicket.FileName);
        }

        private void UpdateOrderSetCouldNotRemoveFile(ProcessOrder processOrder, string filePath)
        {
            processOrder.Message = Texts.CouldNotRemoveProcessOrderResultsFile.With(filePath);
            processOrder.State = ProcessOrderState.Error;
            UpdateProgress(processOrder);

            LogService.Info("ProcessOrder: Failed to remove results file \"{4}\" of {0} (id {1}, owner {2}, server affinity {3})", processOrder.Ticket, processOrder.Id, processOrder.CreatedUserId, processOrder.CreatedMachineName, filePath);
        }

        #endregion

        #endregion
    }
}

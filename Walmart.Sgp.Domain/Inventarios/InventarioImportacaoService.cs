using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Helpers;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Serviço de domínio relacionado a importação de inventário.
    /// </summary>
    public class InventarioImportacaoService : IInventarioImportacaoService
    {
        #region Fields
        private readonly IInventarioGateway m_inventarioGateway;
        private readonly IBandeiraService m_bandeiraService;
        private readonly ILojaService m_lojaService;
        private readonly ILeitorArquivoInventario m_leitorArquivoInventario;
        private readonly ITransferidorArquivosInventario m_transferidorArquivosInventario;
        private readonly ICategoriaService m_categoriaService;
        private readonly IParametroService m_parametroService;
        private readonly IInventarioItemGateway m_inventarioItemGateway;
        private readonly IItemDetalheGateway m_itemDetalheGateway;
        private readonly ILeitorLogger m_logger;
        private readonly IPermissaoService m_permissaoService;
        private readonly IDepartamentoService m_departamentoService;
        private readonly ConfiguracaoArquivosInventario m_configuracaoArquivosInventario;
        #endregion

        #region Constructor

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioImportacaoService"/>.
        /// </summary>
        /// <param name="inventarioGateway">O serviço de inventario.</param>
        /// <param name="parametroService">O serviço de parâmetros do sistema.</param>
        /// <param name="bandeiraService">O serviço de bandeira.</param>
        /// <param name="lojaService">O serviço de loja.</param>
        /// <param name="leitorArquivoInventario">O serviço de IO para leitura dos arquivos de inventário.</param>
        /// <param name="transferidorArquivosInventario">O serviço de IO para transferência dos arquivos de inventário do servidor FTP.</param>
        /// <param name="categoriaService">O serviço de categoria.</param>
        /// <param name="inventarioItemGateway">O gateway de InventarioItem.</param>
        /// <param name="itemDetalheGateway">O gateway de ItemDetalhe.</param>
        /// <param name="logger">O serviço de log de importação.</param>
        /// <param name="permissaoService">O serviço de permissões.</param>
        /// <param name="departamentoService">O serviço de departamentos.</param>
        /// <param name="configuracaoArquivosInventario">A configuração de arquivos de inventário.</param>
        public InventarioImportacaoService(IInventarioGateway inventarioGateway, IParametroService parametroService, IBandeiraService bandeiraService, ILojaService lojaService, ILeitorArquivoInventario leitorArquivoInventario, ITransferidorArquivosInventario transferidorArquivosInventario, ICategoriaService categoriaService, IInventarioItemGateway inventarioItemGateway, IItemDetalheGateway itemDetalheGateway, ILeitorLogger logger, IPermissaoService permissaoService, IDepartamentoService departamentoService, ConfiguracaoArquivosInventario configuracaoArquivosInventario)
        {
            m_inventarioGateway = inventarioGateway;
            m_bandeiraService = bandeiraService;
            m_lojaService = lojaService;
            m_leitorArquivoInventario = leitorArquivoInventario;
            m_transferidorArquivosInventario = transferidorArquivosInventario;
            m_categoriaService = categoriaService;
            m_parametroService = parametroService;
            m_inventarioItemGateway = inventarioItemGateway;
            m_itemDetalheGateway = itemDetalheGateway;
            m_logger = logger;
            m_permissaoService = permissaoService;
            m_departamentoService = departamentoService;
            m_configuracaoArquivosInventario = configuracaoArquivosInventario;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Realiza importação automática.
        /// </summary>
        /// <param name="request">Os parâmetros da importação.</param>
        /// <param name="tipoOrigemImportacao">O tipo de origem do processo de importação (Loja ou HO).</param>
        /// <returns>
        /// O resultado da operação.
        /// </returns>
        public ImportarInventarioResponse ImportarAutomatico(ImportarInventarioAutomaticoRequest request, TipoOrigemImportacao tipoOrigemImportacao)
        {
            TipoProcessoImportacao tipoProcesso = TipoProcessoImportacao.Automatico;

            SpecService.Assert(new { cdSistema = request.CdSistema, idBandeira = request.IdBandeira, idLoja = request.IdLoja }, new AllMustBeInformedSpec());

            Bandeira bandeira = m_bandeiraService.ObterPorId(request.IdBandeira);

            if (!bandeira.BlImportarTodos)
            {
                SpecService.Assert(new { idDepartamento = request.CdDepartamento }, new AllMustBeInformedSpec());
            }

            CarregarCategoriaAtacado(request);

            // frmImportarInventario.aspx.cs linha 198
            var datasInventarios = m_inventarioGateway.ObterInventariosAbertosParaImportacao(request.IdLoja, null, request.CdSistema != 2 ? request.IdDepartamento : null, request.IdCategoria);

            if (!datasInventarios.Any())
            {
                return NenhumInventarioAberto();
            }
            else
            {
                // frmImportarInventario.aspx.cs linha 222
                // A data que seria parseada é a que veio da tela, que por sua vez veio do ObterInventariosAbertosParaImportacao().
                // Não acata o que veio da tela para evitar erros; obtém a data internamente.
                request.DataInventario = datasInventarios.Select(i => i.dhInventario).OrderBy(i => i).First();
            }

            Inventario inventHoje = m_inventarioGateway.ObterInventarioAprovadoFinalizadoMesmaData(request);

            if (inventHoje != null)
            {
                return InventarioJaExiste(request, inventHoje);
            }

            Loja loja = m_lojaService.ObterPorId(request.IdLoja);

            // Config.BL.cs linhas 118 ou 136
            // aparentemente é lido para uma propriedade publica da classe config.bl mas não é usado
            // era passado para o ObterArquivosViaFtp()
            // DateTime? dataUltimaImportacaoLoja = m_inventarioGateway.ObterUltimaImportacaoInventarioDaLoja(loja.IDLoja);            
            var arquivosTransferidos = m_transferidorArquivosInventario.ObterArquivosViaFtp(request.CdSistema, request.CdDepartamento, request.DataInventario, tipoOrigemImportacao, tipoProcesso, loja);

            return RealizarImportacao(request, tipoOrigemImportacao, tipoProcesso, loja, arquivosTransferidos);
        }

        /// <summary>
        /// Realiza importação manual.
        /// </summary>
        /// <param name="request">Os parâmetros da importação.</param>
        /// <param name="tipoOrigemImportacao">O tipo de origem do processo de importação (Loja ou HO).</param>
        /// <returns>
        /// O resultado da operação.
        /// </returns>
        public ImportarInventarioResponse ImportarManual(ImportarInventarioManualRequest request, TipoOrigemImportacao tipoOrigemImportacao)
        {
            TipoProcessoImportacao tipoProcesso = TipoProcessoImportacao.Manual;

            SpecService.Assert(new { cdSistema = request.CdSistema, idBandeira = request.IdBandeira, idLoja = request.IdLoja, dataInventario = request.DataInventario, }, new AllMustBeInformedSpec());

            // TODO: transformar isso numa spec
            bool temPermissao = m_permissaoService.PossuiPermissaoLoja(RuntimeContext.Current.User.Id, request.IdLoja);

            Loja loja = m_lojaService.ObterPorId(request.IdLoja);

            if (!temPermissao)
            {
                throw new UserInvalidOperationException(Texts.NoPermissionToAccess.With(loja.nmLoja));
            }

            if (!PossuiAlgumDepartamento(request))
            {
                return new ImportarInventarioResponse { Mensagem = Texts.CannotImportAnyDepartment };
            }

            IEnumerable<string> arquivosTransferidos = m_transferidorArquivosInventario.CopiarArquivosParaImportar(request.Arquivos, loja);

            var autoRequest = new ImportarInventarioAutomaticoRequest
            {
                CdCategoria = null,
                CdDepartamento = null,
                CdSistema = request.CdSistema,
                DataInventario = request.DataInventario,
                IdBandeira = request.IdBandeira,
                IdCategoria = null,
                IdDepartamento = null,
                IdLoja = request.IdLoja
            };

            return RealizarImportacao(autoRequest, tipoOrigemImportacao, tipoProcesso, loja, arquivosTransferidos);
        }

        /// <summary>
        /// Caso atacado, carrega a categoria correspondente ao departamento selecionado (atacado / cdSistema=2 não possui departamentos, então departamento=categoria)
        /// </summary>
        /// <param name="request">Os parâmetros de processamento.</param>
        /// <remarks>Quando cdSistema=2, a tabela categoria possui registros onde cdCategoria e dsCategoria correspondem aos registros na tabela departamento (cdDepartamento, dsDepartamento).</remarks>
        /// <returns>O id da categoria equivalente do departamento no atacado, quando aplicado.</returns>
        public int? CarregarCategoriaAtacado(ImportarInventarioAutomaticoRequest request)
        {
            if (request.CdSistema == 2 && request.CdDepartamento.HasValue)
            {
                Categoria categoria = m_categoriaService.ObterPorCategoriaESistema(request.CdDepartamento.Value, request.CdDepartamento, (byte)request.CdSistema);
                request.IdCategoria = categoria.IDCategoria;
                request.CdCategoria = (int)categoria.cdCategoria;
            }
            else
            {
                request.CdCategoria = null;
                request.IdCategoria = null;
            }

            return request.IdCategoria;
        }

        /// <summary>
        /// Obtém a lista de prefixos de arquivos de importação considerados válidos conforme a parametrização atual do sistema.
        /// </summary>
        /// <returns>Lista de prefixos válidos.</returns>
        public IEnumerable<string> ObterPrefixosArquivos()
        {
            Parametro parametro = m_parametroService.Obter();

            if (parametro.TpArquivoInventario == TipoFormatoArquivoInventario.Pipe)
            {
                return new string[] { m_configuracaoArquivosInventario.PrefixoArquivoPipe };
            }
            else
            {
                return new string[] { m_configuracaoArquivosInventario.PrefixoArquivoRtl };
            }
        }

        private static ImportarInventarioResponse NenhumInventarioAberto()
        {
            return new ImportarInventarioResponse() { Mensagem = Texts.ImportLogNoSchedule };
        }

        private bool PossuiAlgumDepartamento(ImportarInventarioManualRequest request)
        {
            var departamentos = m_departamentoService.PesquisarPorDivisaoESistema(null, null, true, null, request.CdSistema, null);

            return departamentos.Select(departamento =>
            {
                if (!m_inventarioGateway.ObterInventariosAbertosParaImportacao(request.IdLoja, request.DataInventario, null, null).Any())
                {
                    return false;
                }

                var inventHoje = m_inventarioGateway.ObterInventarioAprovadoFinalizadoMesmaData(new ImportarInventarioAutomaticoRequest { CdSistema = request.CdSistema, CdDepartamento = departamento.cdDepartamento, DataInventario = request.DataInventario });

                if (inventHoje != null)
                {
                    m_logger.InserirInventarioCritica(request.IdLoja, Texts.ImportLogErrorAlreadyExists, 2, null, null, null, request.DataInventario);
                    return false;
                }

                return true;
            }).Any(r => r);
        }

        private ImportarInventarioResponse RealizarImportacao(ImportarInventarioAutomaticoRequest request, TipoOrigemImportacao tipoOrigem, TipoProcessoImportacao tipoProcesso, Loja loja, IEnumerable<string> arquivosTransferidos)
        {
            var arquivos = LerArquivos(arquivosTransferidos, request, tipoOrigem, tipoProcesso, loja.TipoArquivoInventario).Where(a => a.IsArquivoValido);

            if (arquivos.Any())
            {
                var criticas = ProcessarArquivos(request, loja, arquivos).Select(a => new { NomeArquivo = a.Item1, QtdCriticas = a.Item2 });

                var qtdArquivosTransferidos = arquivos.Count();
                var arquivosUsados = criticas.Select(c => c.NomeArquivo).Distinct().Count();
                var qtdCriticas = criticas.Select(c => c.QtdCriticas).Sum();

                var arquivosNaoProcessados = arquivosTransferidos.Except(arquivosTransferidos.Where(a => criticas.Any(c => c.NomeArquivo == Path.GetFileName(a))).ToArray()).ToArray();

                m_transferidorArquivosInventario.ExcluirArquivosNaoProcessados(loja.cdLoja, arquivosNaoProcessados.Select(a => new ArquivoInventario(0, Path.GetFileName(a), DateTime.MinValue)).ToArray());

                return new ImportarInventarioResponse() { Mensagem = Texts.ImportLogSucessMessage.With(arquivosUsados, qtdCriticas), QtdArquivosTransferidos = qtdArquivosTransferidos, QtdCriticas = qtdCriticas, QtdArquivosUsados = arquivosUsados, Sucesso = true, Arquivos = criticas };
            }
            else
            {
                m_transferidorArquivosInventario.ExcluirArquivosNaoProcessados(loja.cdLoja, arquivosTransferidos.Select(a => new ArquivoInventario(0, Path.GetFileName(a), DateTime.MinValue)).ToArray());

                var qtdArquivosTransferidos = arquivos.Count();
                
                return new ImportarInventarioResponse() { Mensagem = Texts.ImportLogSuccessNoFiles, QtdArquivosTransferidos = qtdArquivosTransferidos, QtdArquivosUsados = 0, Sucesso = true };
            }
        }

        private IEnumerable<Tuple<string, int>> ProcessarArquivos(ImportarInventarioAutomaticoRequest request, Loja loja, IEnumerable<ArquivoInventario> arquivos)
        {
            m_logger.ExcluirInventarioCritica(loja.IDLoja, request.DataInventario, request.IdDepartamento, request.IdCategoria);

            var inventariosAbertos = m_inventarioGateway
                .ObterInventariosAbertosParaImportacao(loja.IDLoja, null, request.CdSistema != 2 ? request.IdDepartamento : null, request.IdCategoria)
                .Select(i => m_inventarioGateway.ObterEstruturadoPorId(i.IDInventario))
                .Cast<Inventario>()
                .ToArray();

            // frmImportarInventario.aspx.cs linhas 346-356 e 376-386
            // Para cada registro de inventario, processa cada arquivo caso { cdLoja, cdDepartamento } sejam iguais
            var paraProcessar = (from inventario in inventariosAbertos
                            join arquivo in arquivos
                            on new { cdLoja = inventario.Loja.cdLoja, cdDepartamento = inventario.Departamento.cdDepartamento }
                            equals new { cdLoja = arquivo.CdLoja.Value, cdDepartamento = arquivo.UltimoCdDepartamentoLido ?? -1 }  // arquivos sem itens ficam de fora
                            select new { inventario, arquivo }).Distinct().ToList();

            var criticas = paraProcessar.Select(pp => DistribuirCargaProcessamento(request.CdSistema, request.DataInventario, pp.inventario, pp.arquivo)).ToArray();

            m_transferidorArquivosInventario.RemoverBackupsAntigos(loja.cdLoja, request.DataInventario);

            return criticas.ToArray();
        }

        private Tuple<string, int> DistribuirCargaProcessamento(byte cdSistema, DateTime dataInventario, Inventario inventario, ArquivoInventario arquivo)
        {
            try
            {
                // A critica de "o departamento x nao existe no cadastro" nunca é lançada pois os ifs estão invertidos (causaria um NullReferenceException antes de testar a condição)
                // Nao é necessário testar cdCategoria ou cdDepartamento aqui pois isso já foi garantido no join em ProcessarArquivos(), e CarregarCategoriaAtacado() garante que cdCategoria=cdDepartamento caso cdSistema=2
                // Resta testar se é perecível - David confirmou que no atacado também deve testar se é perecível direto na tabela de depto - Slack #reescrita 2016-25-04 19:51
                AssertCritica(inventario.Departamento, new DepartamentoDeveSerPerecivelSpec(arquivo.NomeArquivo), inventario.IDLoja, 5, inventario.IDInventario, dataInventario);

                // Nao valida a quantidade de dias do arquivo pois isso ja foi validado OU durante a transferencia ou durante a leitura dos arquivos
                // TODO: Talvez seja necessário validar a qtd de dias no futuro, se este método for consumido por outro serviço (importação manual)
                ////Parametro parametro = m_parametroService.Obter();
                ////var qtdDias = inventario.Departamento.cdSistema == 1 ? parametro.qtdDiasArquivoInventarioVarejo : parametro.qtdDiasArquivoInventarioAtacado;
                ////AssertCritica(arquivo, new DataArquivoInventarioDeveSerValidaSpec(dataInventario, qtdDias, (inventario.Departamento.cdSistema == 1 ? "departamento {0}" : "categoria {0}").With(inventario.Departamento.cdDepartamento)), inventario.IDLoja, 2, inventario.IDInventario, dataInventario);

                // TODO: talvez valha para ambos sistemas?
                if (inventario.Loja.cdSistema == 2)
                {
                    // Caso já tenha importado um arquivo mais recente que o arquivo que está sendo importado neste momento
                    AssertCritica(arquivo, new ArquivoInventarioDeveSerOMaisRecenteSpec(inventario, (inventario.Departamento.cdSistema == 1 ? "Departamento: {0}" : "Categoria: {0}").With(inventario.Departamento.cdDepartamento)), inventario.IDLoja, 5, inventario.IDInventario, dataInventario);

                    // Quando tipo de arquivo de inventário for parcial, apaga os itens existem antes da importação
                    if (inventario.Loja.TipoArquivoInventario == TipoArquivoInventario.Parcial)
                    {
                        m_inventarioItemGateway.Delete("IDInventario=@IDInventario", new { inventario.IDInventario });
                    }
                }
            }
            catch (NotSatisfiedSpecException)
            {
                // Aqui já logou a crítica
                return new Tuple<string, int>(arquivo.NomeArquivo, 1);
            }

            var inventarioItens = m_inventarioItemGateway.Find("IDInventario=@IDInventario", new { inventario.IDInventario });

            // Aqui precisa consultar os ItemDetalhes como forma de validar se o CdItem está cadastrado no banco (ao invés de comparar o CdItem do arquivo direto contra o CdItem do Inventarioitem)
            // O mesmo CdItem pode estar em CdSistemas diferentes como produtos totalmente diferentes.
            // TODO: cachear entre inventarios/arquivos?
            var itensDetalhe = arquivo.Itens.Select(item => item.CdItem).Distinct().Where(cdItem => cdItem != 0).Select(cdItem => new { CdItem = cdItem, ItemDetalhe = m_itemDetalheGateway.ObterPorItemESistema(cdItem, cdSistema) }).ToDictionary(tupla => tupla.CdItem, tupla => tupla.ItemDetalhe);

            int qtdCriticas = DistribuirCargaProcessamentoItens(dataInventario, inventario, arquivo, inventarioItens, itensDetalhe);

            inventario.stInventario = InventarioStatus.Importado;
            inventario.dhImportacao = DateTime.Now;
            inventario.dhInventarioArquivo = arquivo.DataArquivo;
            inventario.cdUsuarioImportacao = RuntimeContext.Current.User.Id;

            m_inventarioGateway.Update(inventario);

            qtdCriticas += m_logger.ApurarCriticaInventarioSemCusto(inventario.IDInventario);

            return new Tuple<string, int>(arquivo.NomeArquivo, qtdCriticas);
        }

        private int DistribuirCargaProcessamentoItens(DateTime dataInventario, Inventario inventario, ArquivoInventario arquivo, IEnumerable<InventarioItem> inventarioItens, Dictionary<long, ItemDetalhe> itensDetalhe)
        {
            var dados = (from arquivoItem in arquivo.Itens
                         let itemDetalhe = itensDetalhe.ContainsKey((int)arquivoItem.CdItem) ? itensDetalhe[(int)arquivoItem.CdItem] : null
                         let inventarioItem = null != itemDetalhe ? inventarioItens.Where(ii => ii.IDItemDetalhe == itemDetalhe.IDItemDetalhe).DefaultIfEmpty(new InventarioItem { IDInventario = inventario.IDInventario, IDItemDetalhe = itemDetalhe.IDItemDetalhe }).ToList() : new List<InventarioItem>()
                         select new { ArquivoItem = arquivoItem, ItemDetalhe = itemDetalhe, InventarioItems = inventarioItem }).ToList();

            var criticas = dados.Where(x => x.ItemDetalhe == null).ToList();

            criticas.ForEach(c =>
            {
                m_logger.InserirInventarioCritica(inventario.IDLoja, Texts.InventoryImportCannotFindItem.With(c.ArquivoItem.DescricaoItem, c.ArquivoItem.CdItem), 9, inventario.IDInventario, null, null, dataInventario);
            });

            int qtdCriticas = criticas.Count;

            var naoCriticados = dados.Except(criticas).ToList();

            naoCriticados.ForEach(dado =>
            {
                dado.InventarioItems.ForEach(inventarioItem =>
                {
                    inventarioItem.qtItem = dado.ArquivoItem.QtItem;
                    inventarioItem.dhUltimaContagem = dado.ArquivoItem.UltimaContagem;
                    inventarioItem.qtItemInicial = inventarioItem.IsNew ? dado.ArquivoItem.QtItem : inventarioItem.qtItemInicial;
                });
            });

            var alterados = naoCriticados.SelectMany(dado => dado.InventarioItems);

            m_inventarioItemGateway.Insert(alterados.Where(i => i.IsNew).ToArray());

            alterados.Where(i => !i.IsNew).ToList().ForEach(i =>
            {
                m_inventarioItemGateway.Update(i);
            });

            return qtdCriticas;
        }

        private void AssertCritica<TTarget>(TTarget target, ISpec<TTarget> spec, int idLoja, short idInventarioCriticaTipo, int? idInventario, DateTime? dataInventario)
        {
            var specResult = spec.IsSatisfiedBy(target);

            if (!specResult)
            {
                m_logger.InserirInventarioCritica(idLoja, specResult.Reason, idInventarioCriticaTipo, idInventario, null, null, dataInventario);

                throw new NotSatisfiedSpecException(specResult.Reason);
            }
        }

        private IEnumerable<ArquivoInventario> LerArquivos(IEnumerable<string> arquivosLocais, ImportarInventarioAutomaticoRequest request, TipoOrigemImportacao tipoOrigem, TipoProcessoImportacao tipoProcesso, TipoArquivoInventario tipoArquivoInventario)
        {
            Parametro parametro = m_parametroService.Obter();

            if (parametro.TpArquivoInventario == TipoFormatoArquivoInventario.Rtl)
            {
                if (request.CdSistema == 1)
                {
                    return m_leitorArquivoInventario.LerArquivosRtlSupercenter(tipoProcesso, tipoOrigem, request.IdLoja, arquivosLocais, request.DataInventario);
                }
                else
                {
                    return m_leitorArquivoInventario.LerArquivosRtlSams(tipoProcesso, tipoOrigem, request.IdLoja, arquivosLocais, tipoArquivoInventario, request.DataInventario);
                }
            }
            else
            {
                // frmImportarInventario.aspx.cs linha 319 - se não é Rtl, é Pipe
                return m_leitorArquivoInventario.LerArquivosPipe(tipoProcesso, tipoOrigem, request.CdSistema, request.IdLoja, arquivosLocais, request.DataInventario);
            }
        }

        private ImportarInventarioResponse InventarioJaExiste(ImportarInventarioAutomaticoRequest request, Inventario inventHoje)
        {
            m_logger.InserirInventarioCritica(request.IdLoja, Texts.ImportLogErrorAlreadyExists, 2, null, null, null, request.DataInventario);

            if (!request.CdDepartamento.HasValue)
            {
                // TODO: throw?
                return new ImportarInventarioResponse() { Mensagem = Texts.ImportLogErrorAlreadyExistsStatus.With(inventHoje.stInventario.Description) };
            }
            else
            {
                // TODO: throw?
                return new ImportarInventarioResponse() { Mensagem = Texts.ImportLogErrorAlreadyExistsStatusDept.With(request.CdDepartamento, inventHoje.stInventario.Description) };
            }
        }

        #endregion
    }
}

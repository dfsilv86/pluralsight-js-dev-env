using System;
using System.Linq;
using System.Linq.Expressions;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Infrastructure.Data.Dtos
{
    /// <summary>
    /// Representa o result set da proc PR_SelecionarItemInformacoes.
    /// </summary>
    public class SelecionarItemInformacoesResult
    {
        /// <summary>
        /// Obtém ou define o IdItemDetalhe.
        /// </summary>
        public long IdItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o código do sistema.
        /// </summary>
        public int CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define o CdItem.
        /// </summary>
        public int CdItem { get; set; }

        /// <summary>
        /// Obtém ou define o Descricao.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define o Departamento.
        /// </summary>
        public string Departamento { get; set; }

        /// <summary>
        /// Obtém ou define o Categoria.
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Obtém ou define o idBandeiraSubcategoria.
        /// </summary>
        public string Subcategoria { get; set; }

        /// <summary>
        /// Obtém ou define o Fineline.
        /// </summary>
        public string Fineline { get; set; }

        /// <summary>
        /// Obtém ou define o Divisao.
        /// </summary>
        public string Divisao { get; set; }

        /// <summary>
        /// Obtém ou define o TpUnidadeMedida.
        /// </summary>
        public string TpUnidadeMedida { get; set; }

        /// <summary>
        /// Obtém ou define o CdPlu.
        /// </summary>
        public long? CdPlu { get; set; }

        /// <summary>
        /// Obtém ou define o CdUpc.
        /// </summary>
        public decimal? CdUpc { get; set; }

        /// <summary>
        /// Obtém ou define o DsHostItem.
        /// </summary>
        public string Descricao2 { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public string Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define o FatorConversao.
        /// </summary>
        public decimal? FatorConversao { get; set; }

        /// <summary>
        /// Obtém ou define o Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Obtém ou define o DsFormato.
        /// </summary>
        public string DsFormato { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se o item é de transferência.
        /// </summary>
        public bool BlItemTransferencia { get; set; }

        /// <summary>
        /// Obtém ou define o tpIndPesoReal.
        /// </summary>
        public string TpIndPesoReal { get; set; }

        /// <summary>
        /// Obtém ou define o tpCaixaFornecedor.
        /// </summary>
        public string TpCaixaFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define o vlPesoLiquido.
        /// </summary>
        public decimal VlPesoLiquido { get; set; }

        /// <summary>
        /// Obtém ou define o tpAlinhamentoCD.
        /// </summary>
        public string TpAlinhamentoCD { get; set; }

        /// <summary>
        /// Obtém ou define o dsAreaCD.
        /// </summary>
        public string DsAreaCD { get; set; }

        /// <summary>
        /// Obtém ou define o dsRegiaoCompra.
        /// </summary>
        public string DsRegiaoCompra { get; set; }

        /// <summary>
        /// Obtém ou define o tempoMinimoCD.
        /// </summary>
        public int? TempoMinimoCD { get; set; }

        /// <summary>
        /// Converte para item detalhe.
        /// </summary>
        /// <param name="source">A origem.</param>
        /// <returns>O item detalhe convertido.</returns>
        public static ItemDetalhe ConverterParaItemDetalhe(SelecionarItemInformacoesResult source)
        {
            var target = new ItemDetalhe();
            target.IDItemDetalhe = (int)source.IdItemDetalhe;
            target.CdItem = source.CdItem;
            target.CdSistema = source.CdSistema;
            target.DsItem = source.Descricao;
            target.Departamento = DtoHelper.DefinirCodigoDescricao<Departamento>(source.Departamento, t => t.cdDepartamento, t => t.dsDepartamento);
            target.Categoria = DtoHelper.DefinirCodigoDescricao<Categoria>(source.Categoria, t => t.cdCategoria, t => t.dsCategoria);
            target.Subcategoria = DtoHelper.DefinirCodigoDescricao<Subcategoria>(source.Subcategoria, t => t.cdSubcategoria, t => t.dsSubcategoria);
            target.FineLine = DtoHelper.DefinirCodigoDescricao<FineLine>(source.Fineline, t => t.cdFineLine, t => t.dsFineLine);
            DtoHelper.DefinirCodigoDescricao(
                source.Divisao,
                codigoDescricao =>
                {
                    if (target.Departamento == null)
                    {
                        target.Departamento = new Departamento();
                    }

                    target.Departamento.Divisao = new Divisao
                    {
                        cdDivisao = codigoDescricao.Item1,
                        dsDivisao = codigoDescricao.Item2
                    };
                });

            target.TpUnidadeMedida = source.TpUnidadeMedida;
            target.CdPLU = source.CdPlu;
            target.CdUPC = source.CdUpc;
            target.DsHostItem = source.Descricao2;
            target.VlFatorConversao = (float?)source.FatorConversao;
            target.TpStatus = source.Status;
            target.BlItemTransferencia = source.BlItemTransferencia;
            target.TpIndPesoReal = source.TpIndPesoReal;
            target.TpCaixaFornecedor = source.TpCaixaFornecedor;
            target.VlPesoLiquido = source.VlPesoLiquido;
            target.TpAlinhamentoCD = source.TpAlinhamentoCD;
            target.RegiaoCompra = new RegiaoCompra() { dsRegiaoCompra = source.DsRegiaoCompra };
            target.AreaCD = new AreaCD() { dsAreaCD = source.DsAreaCD };
            target.TempoMinimoCD = source.TempoMinimoCD;
            return target;
        }

        /// <summary>
        /// Converte para bandeira.
        /// </summary>
        /// <param name="source">A origem.</param>
        /// <returns>A bandeira convertida.</returns>
        public static Bandeira ConverterParaBandeira(SelecionarItemInformacoesResult source)
        {
            return new Bandeira
            {
                DsBandeira = source.Bandeira,
                Formato = new Formato
                {
                    dsFormato = source.DsFormato
                }
            };
        }
    }
}
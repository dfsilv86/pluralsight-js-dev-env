using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class RelacaoItemLojaCDConsolidadoTest
    {
        [Test]
        public void blPossuiItensXDockDisponiveis_ItensDetalheXDock_True()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking3
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 2,
                        VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking33
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 3,
                        VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking94
                    }
                }
            };

            Assert.IsTrue(target.blPossuiItensXDockDisponiveis);
        }

        [Test]
        public void blPossuiItensXDockDisponiveis_SemItensDetalheXDock_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
                    }
                }
            };

            Assert.IsFalse(target.blPossuiItensXDockDisponiveis);
        }

        [Test]
        public void blPossuiItensXDockDisponiveis_SemItensDisponiveisXDock_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },
                ItensDisponiveis = null
            };

            Assert.IsFalse(target.blPossuiItensXDockDisponiveis);
        }

        [Test]
        public void blPossuiItensDSDDisponiveis_ItensDetalheDSD_True()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.Dsd7
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 2,
                        VlTipoReabastecimento = ValorTipoReabastecimento.Dsd37
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 3,
                        VlTipoReabastecimento = ValorTipoReabastecimento.Dsd97
                    }
                }
            };

            Assert.IsTrue(target.blPossuiItensDSDDisponiveis);
        }

        [Test]
        public void blPossuiItensDSDDisponiveis_SemItensDetalheDSD_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking3
                    }
                }
            };

            Assert.IsFalse(target.blPossuiItensDSDDisponiveis);
        }

        [Test]
        public void blPossuiItensDSDDisponiveis_SemItensDisponiveisDSD_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },
                ItensDisponiveis = null
            };

            Assert.IsFalse(target.blPossuiItensDSDDisponiveis);
        }

        [Test]
        public void blPossuiItensStapleDisponiveis_ItensDetalheStaple_True()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 7 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 7 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock20
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 2,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock22
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 3,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock40
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 4,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock42
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 5,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock43
                    },

                    new ItemDetalhe
                    {
                        IDItemDetalhe = 6,
                        VlTipoReabastecimento = ValorTipoReabastecimento.StapleStock81
                    }
                }
            };

            Assert.IsTrue(target.blPossuiItensStapleDisponiveis);
        }

        [Test]
        public void blPossuiItensStapleisponiveis_SemItensDetalheStaple_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },

                ItensDisponiveis = new[] 
                { 
                    new ItemDetalhe
                    {
                        IDItemDetalhe = 1,
                        VlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking3
                    }
                }
            };

            Assert.IsFalse(target.blPossuiItensStapleDisponiveis);
        }

        [Test]
        public void blPossuiItensStapleisponiveis_SemItensDisponiveisStaple_False()
        {
            var target = new RelacaoItemLojaCDConsolidado
            {
                RelacaoItemLojaCD = new RelacaoItemLojaCD { IdItemEntrada = 4 },
                ItemEntrada = new ItemDetalhe { IDItemDetalhe = 4 },
                ItensDisponiveis = null
            };

            Assert.IsFalse(target.blPossuiItensStapleDisponiveis);
        }
    }
}
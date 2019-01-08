/*
DECLARE @cdLoja INT, @cdItem INT, @dtSolicitacao DATETIME
--*/

DECLARE @DataUltimoDiaMesAnterior	DATETIME
DECLARE @IDLoja						INT
SET @DataUltimoDiaMesAnterior = convert(date, dateadd(d, -1 * (datepart(d, @dtSolicitacao) ), @dtSolicitacao))
SET @IDLoja = (select IDLoja from Loja where cdLoja = @cdLoja)

SELECT ISNULL((SELECT TOP 1 e.vlCustoGerencialAtual
                FROM Estoque e WITH (NOLOCK)
               WHERE e.IDItemDetalhe = id.IDItemDetalhe
                 AND e.IDLoja = @IDLoja
                 AND e.dtRecebimento <= @dtSolicitacao
            ORDER BY e.dtRecebimento DESC), 0) as ultimoCustoAtual,
      ISNULL((SELECT TOP 1 e.vlCustoGerencialAtual
                FROM Estoque e WITH (NOLOCK)
               WHERE e.IDItemDetalhe = id.IDItemDetalhe
                 AND e.IDLoja = @IDLoja
                 AND e.dtRecebimento <= (SELECT TOP 1 i.dhInventario
                                           FROM Inventario i WITH (NOLOCK)
                                          WHERE i.IDLoja = e.IDLoja
                                            AND i.IdDepartamento = id.IDDepartamento
                                            AND i.stInventario = 3
                                       ORDER BY i.dhInventario DESC)
            ORDER BY e.dtRecebimento DESC), 0) as custoInventario,
	  dbo.fnObterPosicaoEstoque(id.IDItemDetalhe, @IDLoja, @dtSolicitacao) as posEstoqueAtual,
      ISNULL((SELECT TOP 1 e.vlCustoGerencialAtual
                FROM Estoque e WITH (NOLOCK)
               WHERE e.IDItemDetalhe = id.IDItemDetalhe
                 AND e.IDLoja = @IDLoja
                 AND e.dtRecebimento <= @DataUltimoDiaMesAnterior
            ORDER BY e.dtRecebimento DESC), 0) as ultCustoMesAnterior,
      (SELECT TOP 1 c.vlCustoUnitario
         FROM FechamentoFiscal f WITH (NOLOCK)
         JOIN Contabilizacao c WITH (NOLOCK) ON f.IDFechamentoFiscal = c.IdFechamentoFiscal
        WHERE f.IDLoja = @IDLoja
          AND f.nrMes = MONTH(@DataUltimoDiaMesAnterior)
          AND f.nrAno = YEAR(@DataUltimoDiaMesAnterior)
          AND c.IdItemDetalhe = id.IDItemDetalhe) as custoContab
 FROM ItemDetalhe id
WHERE id.cdItem = @cdItem
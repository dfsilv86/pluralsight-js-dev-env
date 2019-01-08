/*
DECLARE @cdItem BIGINT, @IDBandeira INT, @IDLoja INT, @IDUsuario INT, @TipoPermissao INT;
SET @cdItem = 9323540;
SET @IDBandeira = 1;
SET @IDUsuario = 2337;
--SET @TipoPermissao = 1;
--*/

DECLARE @TmpItemDetalheSelecionado TABLE ( 
	IDItemDetalhe BIGINT,
	cdItem INT,
	cdSistema TINYINT,
	tpVinculado CHAR(1),
	tpReceituario CHAR(1),
	tpManipulado CHAR(1),
	PRIMARY KEY (IDItemDetalhe)
);

DECLARE @TmpBandeirasComPermissao TABLE (
	IDBandeira INT,
	IDLoja INT,
	UNIQUE NONCLUSTERED (IDBandeira, IDLoja)
);

DECLARE @TmpNotaFiscalItemElegivel TABLE (
	IDLoja INT,
	IDItemDetalhe INT,
	IDNotaFiscalItem INT,
	dtRecebimento DATETIME,
	vlCusto DECIMAL,
	dhCriacao DATETIME,
	PRIMARY KEY (IDLoja, IDItemDetalhe, IDNotaFiscalItem),
	UNIQUE NONCLUSTERED ( IDNotaFiscalItem, dtRecebimento, dhCriacao )
);

DECLARE @TmpNotaFiscalItemLoja TABLE (
	IDLoja INT,
	vlCusto DECIMAL,
	dtRecebimento DATETIME,
	PRIMARY KEY (IDLoja)
);

DECLARE @TmpLojasPermitidas TABLE (
	IDLoja INT,
	IDBandeira INT,
	cdLoja INT,
	nmLoja VARCHAR(60),
	PRIMARY KEY (IDLoja)	
);

INSERT INTO @TmpItemDetalheSelecionado
SELECT IDItemDetalhe	  
     , cdItem
     , cdSistema
     , tpVinculado
     , tpReceituario
     , tpManipulado
  FROM ItemDetalhe WITH (NOLOCK)
 WHERE cdItem = @cdItem;

-------------------------------------------------------------------------

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
	  
)
INSERT INTO @TmpBandeirasComPermissao
---- Busca conforme permissao por bandeira
SELECT B.IDBandeira, null as IDLoja
  FROM Permissoes P WITH (NOLOCK)
	   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
			   ON PB.IDPermissao = P.IDPermissao
	   INNER JOIN Bandeira B WITH (NOLOCK)
	           ON B.IDBandeira = PB.IDBandeira
 WHERE @TipoPermissao IS NULL OR @TipoPermissao = 2
UNION
-- Busca conforme permissao por loja
SELECT null as IDBandeira, l.IDLoja
  FROM Permissoes P WITH (NOLOCK)
       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
	           ON PL.IDPermissao = P.IDPermissao
	   INNER JOIN Loja L WITH (NOLOCK)
	           ON L.IDLoja = PL.IDLoja
 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1);
  
-------------------------------------------------------------------------

WITH Entradas AS (

    SELECT RIS.IDItemDetalhe AS IDItemDetalheEntrada
      FROM @TmpItemDetalheSelecionado IDS
           INNER JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
                   ON RIP.IDItemDetalhe = IDS.IDItemDetalhe
           INNER JOIN RelacionamentoItemSecundario RIS  WITH (NOLOCK)
                   ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
		WHERE ((IDS.tpVinculado IS NOT NULL) 
		       OR (IDS.tpVinculado IS NULL AND IDS.tpManipulado IS NULL AND IDS.tpReceituario IS NULL))
	      AND RIP.IDTipoRelacionamento = 1

	UNION ALL
	
    SELECT RIS.IDItemDetalhe AS IDItemDetalheEntrada
      FROM @TmpItemDetalheSelecionado IDS
           INNER JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
                   ON RIP.IDItemDetalhe = IDS.IDItemDetalhe
           INNER JOIN RelacionamentoItemSecundario RIS  WITH (NOLOCK)
                   ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
     WHERE (IDS.tpVinculado IS NULL AND IDS.tpManipulado IS NOT NULL)
       AND RIP.IDTipoRelacionamento = 3
	
	UNION ALL
	
    SELECT RIS.IDItemDetalhe AS IDItemDetalheEntrada
      FROM @TmpItemDetalheSelecionado IDS
           INNER JOIN RelacionamentoItemPrincipal RIP WITH (NOLOCK)
                   ON RIP.IDItemDetalhe = IDS.IDItemDetalhe
           INNER JOIN RelacionamentoItemSecundario RIS  WITH (NOLOCK)
                   ON RIP.IDRelacionamentoItemPrincipal = RIS.IDRelacionamentoItemPrincipal
     WHERE (IDS.tpVinculado IS NULL AND IDS.tpManipulado IS NULL AND IDS.tpReceituario IS NOT NULL)
       AND RIP.IDTipoRelacionamento = 2

), EntradasReduce AS (

	SELECT DISTINCT IDItemDetalheEntrada 
	  FROM Entradas WITH (NOLOCK)

), NotaFiscalReduce AS (

	SELECT NFI.IDNotaFiscal
	     , NFI.IDNotaFiscalItem
	     , NFI.IDItemDetalhe
         , NFI.vlCusto
         , NFI.dhCriacao
      FROM EntradasReduce ER WITH (NOLOCK)
           INNER JOIN NotaFiscalItem NFI WITH (NOLOCK)
                   ON NFI.IDItemDetalhe = ER.IDItemDetalheEntrada
     WHERE (NFI.IdNotaFiscalItemStatus IS NULL OR NFI.IdNotaFiscalItemStatus <> 3)
       
)
INSERT INTO @TmpNotaFiscalItemElegivel
SELECT NF.IDLoja
     , NFR.IDItemDetalhe
     , NFR.IDNotaFiscalItem
     , NF.dtRecebimento
     , NFR.vlCusto
     , NFR.dhCriacao
  FROM NotaFiscalReduce NFR WITH (NOLOCK)
       INNER JOIN NotaFiscal NF WITH (NOLOCK)
               ON NF.IDNotaFiscal = NFR.IDNotaFiscal
	   INNER JOIN @TmpBandeirasComPermissao BCP
			   ON BCP.IDBandeira = NF.IDBandeira OR BCP.IDLoja = NF.IDLoja
 WHERE (NF.tpOperacao IS NULL OR NF.tpOperacao = 'E')
 GROUP BY NF.IDLoja
        , NFR.IDItemDetalhe
        , NFR.IDNotaFiscalItem
        , NF.dtRecebimento
        , NFR.vlCusto
        , NFR.dhCriacao;

-------------------------------------------------------------------------

INSERT INTO @TmpNotaFiscalItemLoja
SELECT DISTINCT IDLoja
     , (SELECT TOP 1 vlCusto FROM @TmpNotaFiscalItemElegivel WHERE IDLoja = TB1.IDLoja ORDER BY dtRecebimento DESC, dhCriacao DESC) vlCusto
     , (SELECT TOP 1 dtRecebimento FROM @TmpNotaFiscalItemElegivel WHERE IDLoja = TB1.IDLoja ORDER BY dtRecebimento DESC, dhCriacao DESC) dtRecebimento
  FROM @TmpNotaFiscalItemElegivel TB1;

-------------------------------------------------------------------------

INSERT INTO @TmpLojasPermitidas
SELECT DISTINCT L.IDLoja
     , L.IDBAndeira
     , L.cdLoja
     , L.nmLoja
  FROM Loja L WITH (NOLOCK)
       INNER JOIN @TmpBandeirasComPermissao BCP
               ON L.IDLoja = BCP.IDLoja OR L.IDBandeira = BCP.IDBandeira
 WHERE (@IDLoja IS NULL OR L.IDLoja = @IDLoja)
   AND (@IDBandeira IS NULL OR L.IDBandeira = @IDBandeira);
	 
-------------------------------------------------------------------------

WITH UltimosRecebimentosPorItemELoja AS (

    SELECT E.IDLoja
         , E.IDItemDetalhe
         , MAX(E.dtRecebimento) AS dtRecebimento
      FROM Estoque E WITH (NOLOCK)
           INNER JOIN @TmpItemDetalheSelecionado IDS
                   ON E.IDItemDetalhe = IDS.IDItemDetalhe
     WHERE E.IDLoja IN (SELECT IDLoja FROM @TmpLojasPermitidas)
     GROUP BY E.IDLoja
            , E.IDItemDetalhe
            
)
SELECT COUNT(1)
  FROM @TmpItemDetalheSelecionado IDS
   INNER JOIN Estoque E WITH (NOLOCK)
           ON E.IDItemDetalhe = IDS.IDItemDetalhe
   INNER JOIN UltimosRecebimentosPorItemELoja URPIEL WITH (NOLOCK)
           ON URPIEL.IDItemDetalhe = E.IDItemDetalhe
          AND URPIEL.IDLoja = E.IDLoja
          AND URPIEL.dtRecebimento = E.dtRecebimento
   INNER JOIN @TmpLojasPermitidas L
           ON L.IDLoja = E.IDLoja
    LEFT JOIN @TmpNotaFiscalItemLoja NFI
           ON NFI.IDLoja = E.IDLoja;
           
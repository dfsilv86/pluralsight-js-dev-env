/*DECLARE @relacionamentoSGP INT = 2;
DECLARE @cdDepartamento SMALLINT = 81;
DECLARE @cdItemDetalhe INT = 500211195;
DECLARE @cdV9D BIGINT = 41814811;
DECLARE @idRegiaoCompra INT = 1;
DECLARE @cdSistema INT = 1;
DECLARE @idReturnSheet INT = 1;*/
 
SELECT
      cdItem,
      dsItem,
      relacionamentoSGP,
      hasLojas,
      SplitOn1,
      cdV9D,
      SplitOn2,
      nmFornecedor,
      SplitOn3,
      dsRegiaoCompra,
      qtdLojasSelecionar
FROM (
      SELECT
            IDD.cdItem,
            IDD.dsItem,
            (CASE WHEN IDD.tpManipulado IS NULL AND IDD.tpVinculado IS NULL AND IDD.tpReceituario IS NULL THEN 0 ELSE 1 END) as relacionamentoSGP,
            (CASE WHEN (
                  SELECT COUNT(1)
                  FROM ReturnSheetItemPrincipal RSIP WITH(NOLOCK)
                  INNER JOIN ReturnSheetItemLoja RSIL WITH(NOLOCK)
                        ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
                  WHERE RSIL.blAtivo = 1
                  AND RSIP.IdReturnSheet = @idReturnSheet
                  AND RSIP.IdItemDetalhe = IDD.IDItemDetalhe) = 0 THEN 0 ELSE 1 END) as hasLojas,
            NULL as SplitOn1,
            FP.cdV9D,
            NULL as SplitOn2,
            F.nmFornecedor,
            NULL as SplitOn3,
            RC.dsRegiaoCompra,
            (SELECT COUNT(*)
            FROM RelacaoItemLojaCD RIL WITH(NOLOCK)
            JOIN LojaCDParametro LP WITH(NOLOCK) ON LP.IDLojaCDParametro = RIL.IDLojaCDParametro AND LP.blAtivo = 1
            JOIN (
                  SELECT id.IDItemDetalhe IDItemDetalheSaida
                        ,CASE
                             WHEN COALESCE(id.tpVinculado, id.tpReceituario, id.tpManipulado) IS NULL
                                   THEN id.IDItemDetalhe
                             ELSE rs.IDItemDetalhe
                             END IDItemDetalheEntrada
                        ,ide.IDFornecedorParametro
                  FROM ItemDetalhe id WITH (NOLOCK)
                  LEFT JOIN RelacionamentoItemPrincipal rp WITH (NOLOCK) ON rp.IDItemDetalhe = id.IDItemDetalhe
                  LEFT JOIN RelacionamentoItemSecundario rs WITH (NOLOCK) ON rs.IDRelacionamentoItemPrincipal = rp.IDRelacionamentoItemPrincipal
                  LEFT JOIN ItemDetalhe ide ON ide.IDItemDetalhe = rs.IDItemDetalhe
                  WHERE id.cdItem = IDD.cdItem) ID1 ON ID1.IDItemDetalheEntrada = RIL.IdItemEntrada
            WHERE RIL.blAtivo = 1) AS qtdLojasSelecionar
      FROM ItemDetalhe IDD WITH(NOLOCK)
      INNER JOIN FornecedorParametro FP WITH(NOLOCK)
            ON FP.IDFornecedorParametro = IDD.IDFornecedorParametro
      INNER JOIN Fornecedor F WITH(NOLOCK)
            ON F.IDFornecedor = FP.IDFornecedor
      LEFT JOIN RegiaoCompra RC WITH(NOLOCK)
            ON RC.IdRegiaoCompra = IDD.IdRegiaoCompra
      WHERE
            (IDD.tpVinculado = 'S'
                  OR (IDD.tpReceituario = 'I' AND IDD.tpVinculado IS NULL AND IDD.tpManipulado IS NULL)
                  OR (IDD.tpManipulado = 'P' AND IDD.tpReceituario IS NULL AND IDD.tpVinculado IS NULL))
            AND IDD.cdSistema = @cdSistema
            AND (@cdDepartamento IS NULL OR IDD.cdDepartamentoVendor = @cdDepartamento)
            AND (@cdItemDetalhe IS NULL OR IDD.cdItem = @cdItemDetalhe)
            AND (@cdV9D IS NULL OR FP.cdV9D = @cdV9D)
            AND (@idRegiaoCompra IS NULL OR RC.IdRegiaoCompra = @idRegiaoCompra)
            AND IDD.tpStatus = 'A'
      UNION
      SELECT
            IDD.cdItem,
            IDD.dsItem,
            (CASE WHEN IDD.tpManipulado IS NULL AND IDD.tpVinculado IS NULL AND IDD.tpReceituario IS NULL THEN 0 ELSE 1 END) as relacionamentoSGP,
            (CASE WHEN (
                  SELECT COUNT(1)
                  FROM ReturnSheetItemPrincipal RSIP WITH(NOLOCK)
                  INNER JOIN ReturnSheetItemLoja RSIL WITH(NOLOCK)
                        ON RSIP.IdReturnSheetItemPrincipal = RSIL.IdReturnSheetItemPrincipal
                  WHERE RSIL.blAtivo = 1
                  AND RSIP.IdReturnSheet = @idReturnSheet
                  AND RSIP.IdItemDetalhe = IDD.IDItemDetalhe) = 0 THEN 0 ELSE 1 END) as hasLojas,
            NULL as SplitOn1,
            FP.cdV9D,
            NULL as SplitOn2,
            F.nmFornecedor,
            NULL as SplitOn3,
            RC.dsRegiaoCompra,
            ( SELECT COUNT(*)
            FROM RelacaoItemLojaCD RIL WITH(NOLOCK)
            JOIN LojaCDParametro LP WITH(NOLOCK) ON LP.IDLojaCDParametro = RIL.IDLojaCDParametro AND LP.blAtivo = 1
            JOIN ItemDetalhe id on id.IDItemDetalhe = RIL.IDItem 
            WHERE id.cdItem = IDD.cdItem and RIL.blAtivo = 1) AS qtdLojasSelecionar
      FROM ItemDetalhe IDD WITH(NOLOCK)
      INNER JOIN FornecedorParametro FP WITH(NOLOCK)
            ON FP.IDFornecedorParametro = IDD.IDFornecedorParametro
      INNER JOIN Fornecedor F WITH(NOLOCK)
            ON F.IDFornecedor = FP.IDFornecedor
      LEFT JOIN RegiaoCompra RC WITH(NOLOCK)
            ON RC.IdRegiaoCompra = IDD.IdRegiaoCompra
      WHERE (IDD.tpVinculado IS NULL AND IDD.tpManipulado IS NULL AND IDD.tpReceituario IS NULL)
            AND IDD.cdSistema = @cdSistema
            AND (@cdDepartamento IS NULL OR IDD.cdDepartamentoVendor = @cdDepartamento)
            AND (@cdItemDetalhe IS NULL OR IDD.cdItem = @cdItemDetalhe)
            AND (@cdV9D IS NULL OR FP.cdV9D = @cdV9D)
            AND (@idRegiaoCompra IS NULL OR RC.IdRegiaoCompra = @idRegiaoCompra)
            AND IDD.tpStatus = 'A'
) as TMP
WHERE (@relacionamentoSGP = 2 OR TMP.relacionamentoSGP = @relacionamentoSGP) AND qtdLojasSelecionar > 0
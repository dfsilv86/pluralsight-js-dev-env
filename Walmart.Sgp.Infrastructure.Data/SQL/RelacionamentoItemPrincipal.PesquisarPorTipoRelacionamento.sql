/*
DECLARE @cdSistema SMALLINT, @cdItem BIGINT, @dsItem NVARCHAR(50), @idTipoRelacionamento INT, @cdDepartamento INT, @cdCategoria INT, @cdSubcategoria INT, @cdFineline INT, @idRegiaoCompra INT;
SET @cdSistema = 1;
--SET @idRegiaoCompra = 1;
*/
SELECT Relacionamentos.IDRelacionamentoItemPrincipal
     , RI.IDDepartamento
	 , RI.IDCategoria
     , NULL AS SplitOn1
     , ID.IDItemDetalhe
     , ID.cdOldNumber
     , ID.cdItem
     , ID.dsItem
     , ID.cdPLU
     , ID.dsTamanhoItem
	 , ID.IDSubcategoria
	 , ID.IDFineline
     , NULL AS SplitOn2
     , DE.cdDepartamento
     , DE.dsDepartamento
     , NULL AS SplitOn3
     , CA.cdCategoria
     , CA.dsCategoria
     , NULL AS SplitOn4
     , SC.cdSubcategoria
     , SC.dsSubcategoria
     , NULL AS SplitOn5
     , FL.cdFineline
     , FL.dsFineline
  FROM (
SELECT RIP.IDRelacionamentoItemPrincipal
		  FROM RelacionamentoItemPrincipal RIP WITH (NOLOCK)
			   INNER JOIN ItemDetalhe ID1 WITH (NOLOCK)
					   ON ID1.IDItemDetalhe = RIP.IDItemDetalhe
			   INNER JOIN Departamento DE WITH (NOLOCK)
					   ON DE.IDDepartamento = ID1.IDDepartamento
			   INNER JOIN Categoria CA WITH (NOLOCK)
					   ON CA.IDCategoria = ID1.IDCategoria
			   INNER JOIN Subcategoria SC WITH (NOLOCK)
			           ON SC.IDSubcategoria = ID1.IDSubcategoria
			   INNER JOIN Fineline FL WITH (NOLOCK)
			           ON FL.IDFineline = ID1.IDFineline
			   INNER JOIN RelacionamentoItemSecundario RIS WITH (NOLOCK)
					   ON RIS.IDRelacionamentoItemPrincipal = RIP.IDRelacionamentoItemPrincipal
			   INNER JOIN ItemDetalhe ID2 WITH (NOLOCK)
					   ON ID2.IDItemDetalhe = RIS.IDItemDetalhe
			   LEFT JOIN RegiaoCompra RC
			    	   ON RC.IdRegiaoCompra = ID1.idRegiaoCompra
		 WHERE RIP.cdSistema = @cdSistema
		   AND (@cdItem IS NULL OR ((ID1.cdItem = @cdItem AND ID1.cdSistema = @cdSistema) OR (ID2.cdItem = @cdItem AND ID2.cdSistema = @cdSistema)))
		   AND (@dsItem IS NULL OR (ID1.dsItem LIKE '%' + @dsItem + '%' OR ID1.dsItem LIKE '%' + @dsItem + '%'))
		   AND (@idTipoRelacionamento IS NULL OR RIP.IDTipoRelacionamento = @idTipoRelacionamento)
		   AND (@cdDepartamento IS NULL OR DE.cdDepartamento = @cdDepartamento)
		   AND (@cdCategoria IS NULL OR CA.cdCategoria = @cdCategoria)
		   AND (@cdSubcategoria IS NULL OR SC.cdSubcategoria = @cdSubcategoria)
		   AND (@cdFineline IS NULL OR FL.cdFineline = @cdFineline)
		   AND (@idRegiaoCompra IS NULL OR RC.IdRegiaoCompra = @idRegiaoCompra)
		 GROUP BY RIP.IDRelacionamentoItemPrincipal
	   ) Relacionamentos
	   INNER JOIN RelacionamentoItemPrincipal RI WITH (NOLOCK)
	           ON RI.IDRelacionamentoItemPrincipal = Relacionamentos.IDRelacionamentoItemPrincipal
	   INNER JOIN ItemDetalhe ID WITH (NOLOCK)
	           ON ID.IDItemDetalhe = RI.IDItemDetalhe
	   INNER JOIN Departamento DE WITH (NOLOCK)
	           ON DE.IDDepartamento = ID.IDDepartamento
	   INNER JOIN Categoria CA WITH (NOLOCK)
			   ON CA.IDCategoria = ID.IDCategoria
	   INNER JOIN Subcategoria SC WITH (NOLOCK)
			   ON SC.IDSubcategoria = ID.IDSubcategoria
	   INNER JOIN Fineline FL WITH (NOLOCK)
			   ON FL.IDFineline = ID.IDFineline
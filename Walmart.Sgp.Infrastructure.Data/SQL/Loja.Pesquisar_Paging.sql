/*
DECLARE @IDUsuario INT, @TipoPermissao INT, @cdSistema INT, @idBandeira INT, @cdLoja INT, @nmLoja NVARCHAR(50);
SET @IDUsuario = 20;
SET @TipoPermissao = 2;
SET @cdSistema = NULL;
SET @idBandeira = NULL;
SET @cdLoja = NULL;
SET @nmLoja = NULL;
--*/

-- TODO: mover para proc por performance?

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), Lojas AS (
	-- Busca conforme permissao por bandeira
	SELECT L.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
				  AND (@idBandeira IS NULL OR PB.IDBandeira = @idBandeira)
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDBandeira = PB.IDBandeira
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 2)
	UNION
	-- Busca conforme permissao por loja
	SELECT PL.IDLoja
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
				  AND (@idBandeira IS NULL OR L.IDBandeira = @idBandeira)
				  AND (@cdSistema IS NULL OR L.CdSistema = @cdSistema)
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
), LojasFiltradas AS (
	SELECT L.IDLoja
	  FROM Loja L WITH (NOLOCK)
	 WHERE (@cdLoja IS NULL OR L.cdLoja = @cdLoja)
	   AND (@nmLoja IS NULL OR L.nmLoja LIKE '%' + @nmLoja + '%')
	   AND EXISTS (SELECT TOP 1 1 FROM Lojas LS WITH (NOLOCK) WHERE LS.IDLoja = L.IdLoja)
)
SELECT RowConstrainedResult.*
  FROM ( 
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (

			SELECT L.IDLoja
				 , L.cdSistema
				 , L.IDBandeira
				 , L.cdLoja
				 , L.nmLoja
				 , L.dsEndereco
				 , L.dsCidade
				 , L.dsServidorSmartEndereco
				 , L.dsServidorSmartDiretorio
				 , L.dsServidorSmartNomeUsuario
				 , L.dsServidorSmartSenha
				 , L.blAtivo
				 , L.dhCriacao
				 , L.dhAtualizacao
				 , L.cdUsuarioCriacao
				 , L.cdUsuarioAtualizacao
				 , L.nmDatabase
				 , L.IDDistrito
				 , L.blEnvioBI
				 , L.blCarregaSGP
				 , L.blContabilizar
				 , L.blCorrecaoPLU
				 , L.dsEstado
				 , L.DataConversao
				 , L.TipoCusto
				 , L.TipoArquivoInventario
				 , L.DataEnvioBI
				 , L.cdUsuarioResponsavelLoja
				 , L.blCalculaSugestao
				 , L.blEmitePedido
				 , L.blAutorizaPedido
				 , NULL AS SplitOn1
				 , B.dsBandeira
			  FROM Loja L WITH (NOLOCK)
			       INNER JOIN Bandeira B WITH (NOLOCK)
				           ON B.IDBandeira = L.IDBandeira
				   INNER JOIN LojasFiltradas LS WITH (NOLOCK)
						   ON LS.IdLoja = L.IDLoja
			   ) __INTERNAL
       ) AS RowConstrainedResult        
WHERE   RowNum >= {0}
    AND RowNum < {1}
ORDER BY RowNum
﻿/*
DECLARE @cdSistema AS INT, @idBandeira AS INT, @idRegiao AS INT, @idDistrito AS INT, @idUsuario AS INT, @TipoPermissao INT;

SET @cdSistema = 1;
--SET @idBandeira = 1;
SET @idUsuario = 2337;
--*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), BandeirasComPermissao AS (
	---- Busca conforme permissao por bandeira
	SELECT B.IDBandeira AS IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
		   INNER JOIN PermissaoBandeira PB WITH (NOLOCK)
				   ON PB.IDPermissao = P.IDPermissao
		   INNER JOIN Bandeira B WITH (NOLOCK)
		           ON B.IDBandeira = PB.IDBandeira
	 WHERE @TipoPermissao IS NULL OR @TipoPermissao = 2
	   AND (@idBandeira IS NULL OR B.IDBandeira = @idBandeira)
	UNION
	-- Busca conforme permissao por loja
	SELECT L.IDBandeira AS IDBandeira
	  FROM Permissoes P WITH (NOLOCK)
	       INNER JOIN PermissaoLoja PL WITH (NOLOCK)
		           ON PL.IDPermissao = P.IDPermissao
		   INNER JOIN Loja L WITH (NOLOCK)
		           ON L.IDLoja = PL.IDLoja
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
	   AND (@idBandeira IS NULL OR L.IDBandeira = @idBandeira)
)
SELECT RowConstrainedResult.*
  FROM (
        SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS RowNum, __INTERNAL.*
		  FROM (
				SELECT DISTINCT	 
					 D.IDDistrito
					 , D.IDRegiao
					 , D.nmDistrito
					 , D.cdUsuarioResponsavelDistrito
					 , D.dhCriacao
					 , D.dhAtualizacao
					 , D.cdUsuarioCriacao
					 , D.cdUsuarioAtualizacao
					 , NULL AS SplitOn1
					 , R.nmRegiao
					 , NULL AS SplitOn2
					 , B.IDBandeira
					 , B.blAtivo
					 , B.cdSistema
					 , B.dsBandeira
				  FROM Distrito AS D WITH (NOLOCK)
					   INNER JOIN Regiao R WITH (NOLOCK)
       						   ON R.IDRegiao = D.IDRegiao
					   INNER JOIN Bandeira B WITH (NOLOCK)
       						   ON R.IDBandeira = B.IDBandeira
					   INNER JOIN BandeirasComPermissao BCP WITH (NOLOCK)
							   ON B.IDBandeira = BCP.IDBandeira
				 WHERE (@cdSistema IS NULL OR B.cdSistema = @cdSistema)
				   AND (@idRegiao IS NULL OR D.IDRegiao = @idRegiao)
				   AND (@idDistrito IS NULL OR D.IDDistrito = @idDistrito)
				   AND (@idBandeira IS NULL OR B.idBandeira = @idBandeira)
			   ) __INTERNAL
	   ) RowConstrainedResult
 WHERE RowNum >= {0}
   AND RowNum < {1}
 ORDER BY RowNum
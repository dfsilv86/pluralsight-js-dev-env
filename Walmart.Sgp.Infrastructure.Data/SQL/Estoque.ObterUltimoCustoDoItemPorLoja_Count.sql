﻿/*
DECLARE @idItemDetalhe BIGINT, @idLoja INT;
SET @idItemDetalhe = 16;
SET @idLoja = NULL;
--*/

WITH Permissoes AS (
	-- Busca as permissoes do usuário
	SELECT P.IDPermissao
	  FROM Permissao P WITH (NOLOCK)
	  WHERE P.IDUsuario = @IDUsuario
), BandeirasComPermissao AS (
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
	 WHERE (@TipoPermissao IS NULL OR @TipoPermissao = 1)
), UltimoRecebimento AS (
	SELECT E.IDLoja, E.IDItemDetalhe, MAX(E.dtRecebimento) AS DataUltimoRecebimento
	  FROM Estoque E WITH (NOLOCK)
		   INNER JOIN Loja L WITH (NOLOCK)
				   ON L.IDLoja = E.IDLoja
	       INNER JOIN BandeirasComPermissao BCP
	               ON BCP.IDBandeira = L.IDBandeira OR BCP.IDLoja = L.IDLoja
		   INNER JOIN ItemDetalhe ID WITH (NOLOCK)
				   ON ID.IDItemDetalhe = E.IDItemDetalhe
	 WHERE ID.IDItemDetalhe = @idItemDetalhe
	   AND (@idLoja IS NULL OR L.IDLoja = @idLoja)
	 GROUP BY E.IDLoja, E.IDItemDetalhe
)
SELECT COUNT(1)
	FROM UltimoRecebimento UR
		INNER JOIN Estoque E WITH (NOLOCK)
				ON E.IDLoja = UR.IDLoja
				AND E.IDItemDetalhe = UR.IDItemDetalhe
				AND E.dtRecebimento = UR.DataUltimoRecebimento

/*
DECLARE @IDUsuario INT, @idBandeira INT, @cdLoja INT;
SET @IDUsuario = 2;
SET @idBandeira = 2;
SET @cdLoja = 4;
--*/

-- TODO: mover para proc por performance?

WITH Lojas AS (
	SELECT L.IDLoja
	  FROM Loja l (nolock) 
	 WHERE l.cdLoja = @cdLoja
	   AND (@idBandeira IS NULL OR l.IDBandeira = @idBandeira)
	   AND (EXISTS (SELECT TOP 1 1 
				  FROM Permissao p 
				  JOIN PermissaoLoja pl ON pl.IDPermissao = p.IDPermissao 
				  WHERE p.IDUsuario = @IDUsuario
				  AND pl.IDLoja = l.IDLoja) 
		  OR 
		  EXISTS (SELECT TOP 1 1 
				  FROM Permissao p 
				  JOIN PermissaoBandeira pb ON pb.IDPermissao = p.IDPermissao 
				  WHERE p.IDUsuario = @IDUsuario
				  AND pb.IDBandeira = l.IDBandeira))
)
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
	 , NULL SplitOn1
	 , B.IDBandeira
	 , B.dsBandeira
	 , B.cdSistema
  FROM Loja L WITH (NOLOCK)
       INNER JOIN Lojas LS WITH (NOLOCK)
	           ON LS.IdLoja = L.IDLoja
	   INNER JOIN Bandeira B WITH (NOLOCK)
			   ON B.IDBandeira = L.IDBandeira
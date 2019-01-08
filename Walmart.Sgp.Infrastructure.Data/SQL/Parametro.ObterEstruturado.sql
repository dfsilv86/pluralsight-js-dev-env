/*
TODO: FKs faltantes: em cdUsuarioAdministrador e cdUsuarioAlteracao
--*/

SELECT P.IDParametro
	 , P.cdUsuarioAdministrador
	 , P.dsServidorSmartEndereco
	 , P.dsServidorSmartDiretorio
	 , P.dsServidorSmartNomeUsuario
	 , P.dsServidorSmartSenha
	 , P.dhAlteracao
	 , P.cdUsuarioAlteracao
	 , P.pcDivergenciaCustoCompra
	 , P.qtdDiasSugestaoInventario
	 , P.PercentualAuditoria
	 , P.qtdDiasArquivoInventarioVarejo
	 , P.qtdDiasArquivoInventarioAtacado
	 , P.TpArquivoInventario
	 , NULL AS SplitOn1
	 , UAdm.Username
	 , UAdm.FullName
     , NULL AS SplitOn2
     , UAlt.Username
     , UAlt.FullName
  FROM Parametro P
       INNER JOIN CWIUser UAdm WITH (NOLOCK)
               ON P.cdUsuarioAdministrador = UAdm.Id
       INNER JOIN CWIUser UAlt WITH (NOLOCK)
               ON P.cdUsuarioAlteracao = UAlt.Id
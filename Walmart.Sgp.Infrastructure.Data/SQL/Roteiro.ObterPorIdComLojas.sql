/*
DECLARE @idRoteiro INT;
SET @idRoteiro = NULL;
--*/
	
SELECT
	R.idRoteiro, R.Descricao, R.vlCargaMinima, R.blKgCx, R.idUsuarioCriacao, R.dhCriacao, R.idUsuarioAtualizacao, R.dhAtualizacao, R.blAtivo, R.cdV9D,
	NULL AS SplitOn1,
	RL.idRoteiroLoja, RL.idRoteiro, RL.idloja, RL.blativo
FROM Roteiro R WITH (NOLOCK)
JOIN RoteiroLoja RL  WITH (NOLOCK) ON RL.idRoteiro = R.idRoteiro
WHERE R.idRoteiro = @idRoteiro
CREATE TABLE WLMSLP_STAGE..SugestaoPedidoCdItemRelacionado
(
	IdItemDetalheS BIGINT,
	--dsItemS VARCHAR(100),
	cdItemS INT,
	IdItemDetalheP BIGINT,
	--dsItemP VARCHAR(100),
	cdItemP INT,
	tpVinculado CHAR,
	tpManipulado CHAR,
	tpReceituario CHAR,
	TotalParticipacaoReceita FLOAT,
	TotalRendimentoReceita FLOAT,
	PesoUnitatio DECIMAL(18,3),
	TotalParticipacaoReceitaOriginal FLOAT,
	TotalRendimentoReceitaOriginal FLOAT,
	--IDRelacionamentoItemSecundario INT NOT NULL,
	idCd INT NOT NULL,
	cdCd INT NOT NULL
)
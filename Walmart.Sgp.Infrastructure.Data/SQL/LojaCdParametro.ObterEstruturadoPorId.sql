/*
DECLARE @@idLojaCDParametro INT, @tpReabastecimento VARCHAR

SET @idLojaCDParametro = 1;
SET @tpReabastecimento = 'S';
--*/


SELECT 
	LCPD.*,
	NULL AS SplitOn1,
	LO.cdLoja,
    LO.nmLoja,
	LO.IDBandeira,
	NULL AS SplitOn2,
	CD.IDCD,
    CD.cdCD,
    CD.nmNome,
	NULL AS SplitOn3,      
	DP.cdDepartamento,
	DP.dsDepartamento,
	NULL AS SplitOn4,
	RD.cdReviewDate, 
    RD.tpReabastecimento
FROM 
	LojaCDParametro LCPD INNER JOIN Loja LO  WITH (NOLOCK)
                            ON LCPD.IDLoja = LO.IDLoja 
                    INNER JOIN CD CD  WITH (NOLOCK)
                            ON LCPD.IDCD = CD.IDCD 
                    INNER JOIN ReviewDateCD RD WITH (NOLOCK)
                            ON RD.IDLojaCDParametro = LCPD.IDLojaCDParametro
                    INNER JOIN Departamento DP WITH (NOLOCK)
                            ON DP.IDDepartamento = RD.IDDepartamento
WHERE    
	LCPD.IdLojaCDParametro = @idLojaCDParametro
AND RD.tpReabastecimento = @tpReabastecimento
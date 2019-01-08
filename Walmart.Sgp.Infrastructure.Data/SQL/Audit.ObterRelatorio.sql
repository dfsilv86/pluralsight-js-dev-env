/*
{0}-Nome da tabela
{1}-Pk da tabela
{2}-lista de campos do log (L.Foo, L.Bar, ...)
*/

SELECT TOP 100 PERCENT AuditRecord.*
	FROM (
		SELECT L.IdAuditRecord,
				0 AS blAtual,
				L.CdAuditKind,
				L.DhAuditStamp,
				L.IdAuditUser,
				NULL AS SplitOn1,
				L.{2},
				NULL AS SplitOn2,
				U.Id,
				U.UserName,
				U.FullName
			FROM {0}Log L WITH (NOLOCK)
				INNER JOIN CWIUser U WITH (NOLOCK)
					ON L.IdAuditUser = U.Id

 --Para trazer o registro atual
		--UNION ALL

		--SELECT 0 AS IdAuditRecord,
		--		1 AS blAtual,
		--		NULL AS CdAuditKind,
		--		NULL AS DhAuditStamp,
		--		0 AS IdAuditUser,
		--		--NULL AS DataOperacaoProximo,
		--		NULL AS SplitOn1,
		--		L.{2}
		--	FROM {0} L
    ) AuditRecord 
WHERE (@IdUsuario IS NULL OR AuditRecord.IdAuditUser = @IdUsuario)
  AND (@IdEntidade IS NULL OR AuditRecord.{1} = @IdEntidade)
  AND (@IntervaloInicio IS NULL OR AuditRecord.DhAuditStamp >= @IntervaloInicio)
  AND (@IntervaloFim IS NULL OR AuditRecord.DhAuditStamp <= @IntervaloFim)
ORDER BY AuditRecord.{1} ASC, AuditRecord.blAtual ASC, CdAuditKind ASC, AuditRecord.IdAuditRecord ASC
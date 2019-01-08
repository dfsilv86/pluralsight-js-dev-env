/*
DECLARE @cdSistema INT, @cdItem INT;
SET @cdSistema = 1;
SET @cdItem = 500051463;
--*/

   SELECT L.dsEstado
     FROM Loja L WITH (NOLOCK)
     JOIN LojaCDParametro LP WITH (NOLOCK) ON LP.IDLoja = L.IDLoja
     JOIN RelacaoItemLojaCD RIL WITH (NOLOCK) ON RIL.IDLojaCDParametro = LP.IDLojaCDParametro
     JOIN ItemDetalhe ID ON ID.idItemDetalhe = RIL.idItem
    WHERE LP.blAtivo = 1
      AND RIL.blAtivo = 1
      AND ID.cdItem = @cdItem
      AND LP.cdSistema = @cdSistema
 GROUP BY L.dsEstado
 ORDER BY L.dsEstado
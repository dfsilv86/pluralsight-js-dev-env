/*
DECLARE @idBandeira INT, @idLoja INT, @idDepartamento INT, @cdSistema INT, @vlHoraLimite INT;
SET @cdSistema = 1;
SET @idBandeira = 40;
SET @idDepartamento = 5;
SET @idLoja = 238; -- cdLoja 966
SET @vlHoraLimite = 1130;
--*/

SELECT TOP 1 IDGradeSugestao
     , IDBandeira
     , IDDepartamento
     , IDLoja
     , cdSistema
     , vlHoraInicial
     , vlHoraFinal
  FROM (
        SELECT GS.*
             , 1 AS ORDEM
          FROM GradeSugestao GS WITH (NOLOCK)
         WHERE GS.IDBandeira = @idBandeira
           AND GS.IDLoja = @idLoja
           AND GS.IDDepartamento = @idDepartamento
           AND GS.cdSistema = @cdSistema
           AND vlHoraFinal < @vlHoraLimite
        UNION ALL 
        SELECT GS.*
             , 2 AS ORDEM 
          FROM GradeSugestao GS WITH (NOLOCK)
         WHERE GS.IDBandeira = @idBandeira
           AND GS.IDLoja IS NULL 
           AND GS.IDDepartamento = @idDepartamento
           AND GS.cdSistema = @cdSistema
           AND vlHoraFinal < @vlHoraLimite
       UNION ALL 
        SELECT GS.*
             , 3 AS ORDEM
          FROM GradeSugestao GS WITH (NOLOCK)
         WHERE GS.IDBandeira = @idBandeira
           AND GS.IDLoja = @idLoja
           AND GS.IDDepartamento IS NULL
           AND GS.cdSistema = @cdSistema
           AND vlHoraFinal < @vlHoraLimite
        UNION ALL 
        SELECT GS.*
             , 4 AS ORDEM
          FROM GradeSugestao GS WITH (NOLOCK)
         WHERE GS.IDBandeira = @idBandeira
           AND GS.IDLoja IS NULL
           AND GS.IDDepartamento IS NULL
           AND GS.cdSistema = @cdSistema
           AND vlHoraFinal < @vlHoraLimite
       ) X
 ORDER BY X.ORDEM
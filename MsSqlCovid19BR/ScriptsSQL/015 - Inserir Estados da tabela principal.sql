USE [Covid19Br]
GO

INSERT INTO [dbo].[Estados]
           ([state]
           ,[city_ibge_code]
           ,[place_type])
SELECT	 [state]
		,[city_ibge_code]
        ,[place_type]
  FROM [Covid19Br].[dbo].[OsDadosDoCovid]
  Where [city] = '' AND [place_type] = 'state'
  GROUP BY [state], [city_ibge_code], [place_type]
  ORDER BY [state]

GO



USE [Covid19Br]
GO

INSERT INTO [dbo].[Cidades]
           ([city]
           ,[city_ibge_code]
           ,[place_type]
           ,[state])
SELECT [city]
      ,[city_ibge_code]
	  ,[place_type]
      ,[state]
		FROM [Covid19Br].[dbo].[OsDadosDoCovid]
		Where [city] != '' AND [place_type] != 'state'
		GROUP BY [city], [city_ibge_code], [place_type], [state]
		ORDER BY [state], [city] 
GO



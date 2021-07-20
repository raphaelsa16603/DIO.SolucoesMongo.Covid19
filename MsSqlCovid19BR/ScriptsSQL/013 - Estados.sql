/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT [city_ibge_code]
      ,[state]
	  ,[place_type]
  FROM [Covid19Br].[dbo].[OsDadosDoCovid]
  Where [city] = '' AND [place_type] = 'state'
  GROUP BY [state], [city_ibge_code], [place_type]
  ORDER BY [state]
  
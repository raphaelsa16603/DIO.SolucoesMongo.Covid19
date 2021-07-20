/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT [city]
      ,[city_ibge_code]
	  ,[place_type]
      ,[state]
  FROM [Covid19Br].[dbo].[OsDadosDoCovid]
  Where [city] != '' AND [place_type] != 'state'
  GROUP BY [city], [city_ibge_code], [place_type], [state]
  ORDER BY [state], [city] 
  
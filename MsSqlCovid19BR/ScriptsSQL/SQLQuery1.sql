/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT TOP (1000) [Id]
      ,[city]
      ,[city_ibge_code]
      ,[date]
      ,[epidemiological_week]
      ,[estimated_population]
      ,[estimated_population_2019]
      ,[is_last]
      ,[is_repeated]
      ,[city_ibglast_available_confirmede_code]
      ,[last_available_confirmed_per_100k_inhabitants]
      ,[last_available_date]
      ,[last_available_death_rate]
      ,[last_available_deaths]
      ,[order_for_place]
      ,[place_type]
      ,[state]
      ,[new_confirmed]
      ,[new_deaths]
      ,[uId]
      ,[dadosNovos]
      ,[dadosAtualizados]
  FROM [Covid19Br].[dbo].[OsDadosDoCovid]
  ORDER BY [Id] DESC
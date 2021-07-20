DELETE FROM [dbo].[OsDadosDoCovid] where [Id] in(
	Select Max("Id")
	From [dbo].[OsDadosDoCovid]
	Group By [city], [state], [city_ibge_code], [date]
	HAVING Count(*) > 1
	)
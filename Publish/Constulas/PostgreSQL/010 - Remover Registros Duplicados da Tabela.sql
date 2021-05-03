DELETE FROM public."OsDadosDoCovid" where "Id" in(
	Select Max("Id")
	From public."OsDadosDoCovid"
	Group By "city", "state", "city_ibge_code", "date"
	HAVING Count(*) > 1
	)
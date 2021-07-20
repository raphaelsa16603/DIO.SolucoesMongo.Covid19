Select "city", "state", "city_ibge_code", "date", count(*) as Duplicados
From [dbo].[OsDadosDoCovid]
Group By "city", "state", "city_ibge_code", "date"
HAVING Count(*) > 1
Order By Duplicados desc, "state" asc, "city" asc, "date" desc

Select "Id", "city", "city_ibge_code", "date", "state", "new_confirmed", "new_deaths"
From public."OsDadosDoCovid"
Where "state" = 'TO'
Order By "city" asc, "date" desc
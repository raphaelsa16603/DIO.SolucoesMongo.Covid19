Select "city", "state", Max("estimated_population") as Populacao, 
       Sum("new_confirmed") as Infectados, Sum("new_deaths") as Mortes
From public."OsDadosDoCovid"
Where "state" = 'PB'
Group By "city", "state"
Order By Mortes desc
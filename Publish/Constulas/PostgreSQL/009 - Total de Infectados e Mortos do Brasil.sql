Select Sum("new_confirmed") as Infectados, Sum("new_deaths") as Mortes
From public."OsDadosDoCovid"
Where "city" = ''

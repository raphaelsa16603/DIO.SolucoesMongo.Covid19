## Como você importa um arquivo .sql grande do MS SQL


No prompt de comando, inicie sqlcmd:

sqlcmd -S <server> -i C:\<your file here>.sql 

Apenas substitua <server>pela localização da sua caixa SQL e <your file here>pelo nome do seu script. Não se esqueça, se você estiver usando uma instância SQL, a sintaxe é:

sqlcmd -S <server>\instance.

Aqui está a lista de todos os argumentos que você pode transmitir ao sqlcmd:

Sqlcmd            [-U login id]          [-P password]
  [-S server]            [-H hostname]          [-E trusted connection]
  [-d use database name] [-l login timeout]     [-t query timeout] 
  [-h headers]           [-s colseparator]      [-w screen width]
  [-a packetsize]        [-e echo input]        [-I Enable Quoted Identifiers]
  [-c cmdend]            [-L[c] list servers[clean output]]
  [-q "cmdline query"]   [-Q "cmdline query" and exit] 
  [-m errorlevel]        [-V severitylevel]     [-W remove trailing spaces]
  [-u unicode output]    [-r[0|1] msgs to stderr]
  [-i inputfile]         [-o outputfile]        [-z new password]
  [-f  | i:[,o:]] [-Z new password and exit] 
  [-k[1|2] remove[replace] control characters]
  [-y variable length type display width]
  [-Y fixed length type display width]
  [-p[1] print statistics[colon format]]
  [-R use client regional setting]
  [-b On error batch abort]
  [-v var = "value"...]  [-A dedicated admin connection]
  [-X[1] disable commands, startup script, environment variables [and exit]]
  [-x disable variable substitution]
  [-? show syntax summary] 

#### sqlcmd -S localhost -i ./2021-06-21_23-49\ -\ Script\ de\ INSERTs\ SqlServer.sql
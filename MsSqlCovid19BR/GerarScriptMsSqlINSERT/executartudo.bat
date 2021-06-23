@ECHO OFF 

FOR /d %%a IN (*) DO (
    echo Diretorio [%%a]
    cd %%a
    FOR %%b IN (*) DO (
        SETLOCAL ENABLEDELAYEDEXPANSION
        set str=%%b
        call echo Processando o arquivo [%%b]
        call sqlcmd -S localhost -i %%b
        ENDLOCAL
    )
    cd ..
)

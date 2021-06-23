@ECHO OFF 


FOR /d %%a IN (*) DO (
    echo Diretorio [%%a]
    cd %%a
    FOR %%b IN (*) DO (
        SETLOCAL ENABLEDELAYEDEXPANSION
        set str=%%b
        call echo %%str:a=A%% - %%b
        ENDLOCAL
    )
    cd ..
)
@echo off
cls

set /p remoteip="Enter remote ip (where Motiv is running): "
set /p localip="Enter local ip (this computer, don't use localhost): "

if %remoteip%== goto err_noremoteip
if %localip%=="" goto err_nolocalip

:run
echo.
echo Remote IP: %remoteip%
echo Local IP: %localip%
UnitySample.exe %remoteip% %localip%

goto end

:err_noremoteip
echo Error: No remot eip provided
goto end

:err_nolocalip
echo Error: No local ip provided
goto end

:end
echo.
pause
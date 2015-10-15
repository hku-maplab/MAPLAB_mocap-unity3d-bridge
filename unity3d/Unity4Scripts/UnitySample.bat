@echo off
cls

:args
if "%1"=="" goto setup
if "%2"=="" goto setup
if "%3"=="" goto setup
goto hasargs

:hasargs
set remoteip=%1
set localip=%2
set drive=%3
set path=%4
goto changedir

:changedir
%drive%
cd %path%
echo Drive: %drive%
echo Path: %path%
echo %cd%
goto run

:setup
set /p remoteip="Enter remote ip (where Motiv is running): "
set /p localip="Enter local ip (this computer, don't use localhost): "

if "%remoteip%"=="" goto err_noremoteip
if "%localip%"=="" goto err_nolocalip

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
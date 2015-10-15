@echo off
cls

:findffmpeg
set ffmpeg=ffmpeg.exe
if not exist %ffmpeg% set ffmpeg=c:\\ffmpeg\bin\ffmpeg.exe
if not exist %ffmpeg% goto err

:getremoteip
set /p remoteip="Enter remote ip of videoplayer: "
goto run

:examples
REM %ffmpeg% -f dshow -i video="screen-capture-recorder" -vf scale=640:-1 -r 12 -vcodec mpeg4 -q 12 -f mpegts udp://192.168.0.157:5000?pkt_size=188?buffer_size=65535

REM %ffmpeg% -f dshow -i video="screen-capture-recorder" -vf scale=640:-1 -r 60 -vcodec libx264 -pix_fmt yuv420p -tune zerolatency -preset ultrafast -f mpegts udp://192.168.0.157:5000

:run
%ffmpeg% -f dshow -i video="screen-capture-recorder" -vf scale=640:-1 -r 12 -vcodec mpeg4 -q 0 -f mpegts udp://%remoteip%:5000?pkt_size=188?buffer_size=65535
goto end

:err
echo.
echo An error occured :(

:end
echo.
pause
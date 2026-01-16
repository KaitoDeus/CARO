@echo off
echo Dang build GameCaro...
"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" GameCaro.csproj -t:Rebuild -p:Configuration=Debug
if %errorlevel% neq 0 (
    echo Build THAT BAI!
    pause
    exit /b %errorlevel%
)
echo Build THANH CONG!
echo Dang chay game...
start bin\Debug\GameCaro.exe
pause

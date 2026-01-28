@echo off
echo Dang build GameCaro (Release)...
"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" GameCaro.csproj -t:Rebuild -p:Configuration=Release
if %errorlevel% neq 0 (
    echo Build THAT BAI!
    pause
    exit /b %errorlevel%
)
echo Build THANH CONG!
echo .
echo Ban gio co the su dung Inno Setup de build file setup.iss
pause

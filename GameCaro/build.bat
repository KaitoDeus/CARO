@echo off
rem Lay duong dan tuyet doi cua thu muc Solution (thu muc cha) - khong co dau backslash cuoi
pushd ..
set "SOLUTION_DIR=%CD%"
popd

echo Dang tat GameCaro.exe (neu dang chay)...
taskkill /IM GameCaro.exe /F >nul 2>&1

echo Dang restore NuGet packages...
"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" GameCaro.csproj -t:Restore -p:RestorePackagesConfig=true -p:SolutionDir="%SOLUTION_DIR%"
if %errorlevel% neq 0 (
    echo Restore THAT BAI!
    pause
    exit /b %errorlevel%
)

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

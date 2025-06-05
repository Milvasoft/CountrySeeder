@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

:: === CONFIG ===
set VERSION=1.0.0
set TAG=v%VERSION%
set PUBLISH_DIR=publish
set WIN_DIR=%PUBLISH_DIR%\win
set LINUX_DIR=%PUBLISH_DIR%\linux
set ZIP_DIR=release-zips
set WIN_ZIP=%ZIP_DIR%\CountrySeeder-Windows.zip
set LINUX_ZIP=%ZIP_DIR%\CountrySeeder-Linux.zip
set JSON_FILE=countries_states_cities.json
set PROJECT=CountrySeeder.csproj

:: === CLEAN ===
echo 🧹 Cleaning old artifacts...
rd /s /q %PUBLISH_DIR% 2>nul
rd /s /q %ZIP_DIR% 2>nul
mkdir %PUBLISH_DIR%
mkdir %ZIP_DIR%

:: === BUILD WINDOWS ===
echo 🛠 Publishing Windows binary...
dotnet publish %PROJECT% -c Release -r win-x64 --self-contained true -o %WIN_DIR% /p:PublishSingleFile=true
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Windows build failed!
    goto END
)

:: === BUILD LINUX ===
echo 🛠 Publishing Linux binary...
dotnet publish %PROJECT% -c Release -r linux-x64 --self-contained true -o %LINUX_DIR% /p:PublishSingleFile=true
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Linux build failed!
    goto END
)

:: === COPY JSON ===
copy %JSON_FILE% %WIN_DIR%
copy %JSON_FILE% %LINUX_DIR%

:: === ZIP ===
echo 📦 Zipping outputs...
powershell Compress-Archive -Path "%WIN_DIR%\*" -DestinationPath %WIN_ZIP%
powershell Compress-Archive -Path "%LINUX_DIR%\*" -DestinationPath %LINUX_ZIP%

:: === GITHUB AUTH CHECK ===
echo 🔐 Checking GitHub CLI authentication...
gh auth status >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ❌ GitHub CLI not authenticated. Run: gh auth login
    goto END
)

:: === DELETE EXISTING RELEASE ===
echo 🔄 Deleting previous release (if exists)...
gh release delete %TAG% --yes >nul 2>&1

:: === CREATE RELEASE ===
echo 🚀 Creating GitHub Release: %TAG%
gh release create %TAG% ^
  %WIN_ZIP% ^
  %LINUX_ZIP% ^
  --title "%VERSION% Release" ^
  --notes "🚀 CountrySeeder %VERSION% with Windows and Linux binaries as ZIP."

if %ERRORLEVEL% NEQ 0 (
    echo ❌ Failed to create GitHub Release
    goto END
)

echo ✅ Release %TAG% published successfully!

:END
echo.
pause
endlocal

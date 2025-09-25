@echo off

REM Change directory to D:\git\CloudbiteBackend-qa\SeleniumTests\bin\Debug\net8.0
cd /d D:\git\CloudbiteBackend-qa\SeleniumTests\bin\Debug\net8.0

REM Delete the allure-results folder and its contents
if exist allure-results (
    rmdir /s /q allure-results
    echo "Deleted allure-results folder."
) else (
    echo "No allure-results folder found."
)

REM Run the SeleniumTests.dll using dotnet test
dotnet test SeleniumTests.dll --logger:"console;verbosity=normal"


REM Generate Allure report with --clean and --single-file options
allure generate --clean --single-file

REM Wait for 3 seconds before opening the report
timeout /t 3 /nobreak >nul

REM  Open the generated Allure report (index.html) with Google Chrome
if exist allure-report\index.html (
    REM Modify the Chrome path to match where Chrome is installed on your machine
    "C:\Program Files\Google\Chrome\Application\chrome.exe" "D:\git\CloudbiteBackend-qa\SeleniumTests\bin\Debug\net8.0\allure-report\index.html"
) else (
    echo "Allure report not found. Unable to open index.html."
)

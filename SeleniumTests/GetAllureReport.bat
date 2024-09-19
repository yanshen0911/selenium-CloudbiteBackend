@echo off

REM Step 1: Change directory to D:\git\erpplus-qa\SeleniumTests\bin\Debug\net8.0
cd /d D:\git\erpplus-qa\SeleniumTests\bin\Debug\net8.0

REM Step 2: Delete the allure-results folder and its contents
if exist allure-results (
    rmdir /s /q allure-results
    echo "Deleted allure-results folder."
) else (
    echo "No allure-results folder found."
)

REM Step 3: Run the SeleniumTests.dll using dotnet test
dotnet test SeleniumTests.dll --logger:"console;verbosity=normal"


REM Step 4: Generate Allure report with --clean and --single-file options
allure generate --clean --single-file

REM Step 5: Wait for 3 seconds before opening the report
timeout /t 3 /nobreak >nul

REM Step 6: Open the generated Allure report (index.html) with Google Chrome
if exist allure-report\index.html (
    REM Modify the Chrome path to match where Chrome is installed on your machine
    "C:\Program Files\Google\Chrome\Application\chrome.exe" "D:\git\erpplus-qa\SeleniumTests\bin\Debug\net8.0\allure-report\index.html"
) else (
    echo "Allure report not found. Unable to open index.html."
)

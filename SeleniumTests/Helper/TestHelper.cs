using OpenQA.Selenium;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class TestHelper
{
    private IWebDriver driver;

    // Constructor to initialize the WebDriver in the helper class
    public TestHelper(IWebDriver driver)
    {
        this.driver = driver;
    }

    // Method to sanitize file names by replacing invalid characters
    private string SanitizeFileName(string fileName)
    {
        // Replace invalid characters with underscores
        return Regex.Replace(fileName, @"[^\w\-]", "_");
    }

    // Method to take a screenshot
    public void TakeScreenshot(string testName)
    {
        try
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            // Sanitize the test name to create a valid file name
            string sanitizedTestName = SanitizeFileName(testName);

            // Define where to save the screenshot
            string screenshotDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }

            // Define the file name and path
            string screenshotFilePath = Path.Combine(screenshotDirectory, $"{sanitizedTestName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            // Save the screenshot
            screenshot.SaveAsFile(screenshotFilePath);
            Console.WriteLine($"Screenshot saved: {screenshotFilePath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to take screenshot: {e.ToString()}");
        }
    }

    // Method to log test results to a file
    public void LogTestResult(string message)
    {
        try
        {
            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Sanitize the test name to create a valid file name
            string sanitizedLogFileName = SanitizeFileName($"TestLog_{DateTime.Now:yyyyMMdd}");

            string logFilePath = Path.Combine(logDirectory, $"{sanitizedLogFileName}.txt");

            // Debug log: Check where the log file is being saved
            Console.WriteLine($"Attempting to write log to: {logFilePath}");

            // Write the log to the file
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
                writer.Flush();  // Ensure data is written to the file immediately
            }

            // Confirm log entry was written
            Console.WriteLine($"Log entry written: {message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to write log: {e.ToString()}");
        }
    }
}

namespace SeleniumTests.Data
{
    public static class LanguageData
    {
        // Valid language data with expected breadcrumb text
        public static IEnumerable<object[]> ValidLanguageData()
        {
            yield return new object[] { "en", "Dashboard", true };    // English
            yield return new object[] { "zh", "仪表板", true };       // Chinese
            yield return new object[] { "ja", "ダッシュボード", true }; // Japanese
        }

        // Invalid language data with incorrect expected breadcrumb text
        public static IEnumerable<object[]> InvalidLanguageData()
        {
            yield return new object[] { "en", "仪表板", false };           // English should not display Chinese breadcrumb
            yield return new object[] { "zh", "Dashboard", false };         // Chinese should not display English breadcrumb
            yield return new object[] { "ja", "仪表板", false };       // Japanese should not display Chinese breadcrumb
            yield return new object[] { "invalid_code", "仪表板", false };       // invalid_code should not display Chinese breadcrumb
        }

        // Mixed language data (valid and invalid)
        public static IEnumerable<object[]> MixedLanguageData()
        {
            // Mixing valid and invalid data
            yield return new object[] { "en", "Dashboard", true };    // Valid case
            yield return new object[] { "zh", "仪表板", true };       // Valid case
            yield return new object[] { "ja", "ダッシュボード", true }; // Valid case
            yield return new object[] { "en", "仪表板", false };       // Invalid case
            yield return new object[] { "zh", "Dashboard", false };    // Invalid case
        }
    }
}

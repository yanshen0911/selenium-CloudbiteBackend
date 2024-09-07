using System.Collections.Generic;

namespace ERPPlus.IntegrationTests.Data.TestData
{
    public static class LoginData
    {
        // Valid login credentials
        public static IEnumerable<object[]> ValidLoginData()
        {
            yield return new object[] { "admin", "password" };
            yield return new object[] { "user1", "password1" };
        }

        // Invalid login credentials
        public static IEnumerable<object[]> InvalidLoginData()
        {
            yield return new object[] { "wrongUser", "password" };
            yield return new object[] { "admin", "wrongPassword" };
        }
    }
}

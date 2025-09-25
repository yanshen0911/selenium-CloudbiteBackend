using System.Collections.Generic;

namespace CloudbiteBackend.IntegrationTests.Data.TestData
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

        public static IEnumerable<object[]> LoginTestCases()
        {
            yield return new object[] { "admin", "password", 200 };  // True Positive: valid credentials
            yield return new object[] { "wrongUser", "wrongPassword", 401 };  // True Negative: invalid credentials
            yield return new object[] { "wrongUser", "wrongPassword", 200 };  // False Positive: invalid login returning success
            yield return new object[] { "admin", "password", 401 };  // False Negative: valid login failing

            yield return new object[] { "", "", 200 };  // NULL return success
            yield return new object[] { "admin", "", 200 };  // NULL return success
            yield return new object[] { "", "password", 200 };  // NULL return success
        }
    }
}

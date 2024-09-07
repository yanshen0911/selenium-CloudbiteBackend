using System.Collections.Generic;

namespace SeleniumTests.Data
{
    public static class LoginData
    {
        // Valid login credentials
        public static IEnumerable<object[]> ValidLoginData()
        {
            yield return new object[] { "admin", "password" };
            yield return new object[] { "user1", "password1" };
            yield return new object[] { "testUser", "testPassword" };
        }

        // Invalid login credentials (mixing invalid usernames and passwords)
        public static IEnumerable<object[]> InvalidLoginData()
        {
            yield return new object[] { "wrongUser", "password" };  // Invalid username, valid password
            yield return new object[] { "admin", "wrongPassword" }; // Valid username, invalid password
            yield return new object[] { "", "" };                   // Empty username and password
            yield return new object[] { "nonExistentUser", "wrongPassword" }; // Both username and password are invalid
        }

        // Mixed login credentials (both valid and invalid for data-driven testing)
        public static IEnumerable<object[]> MixedLoginData()
        {
            // Mixing valid and invalid data
            yield return new object[] { "admin", "password", true };        // Valid case
            yield return new object[] { "admin", "wrongPassword", false };  // Invalid password
            yield return new object[] { "wrongUser", "password", false };   // Invalid username
            yield return new object[] { "user1", "password1", true };       // Valid case
            yield return new object[] { "testUser", "wrongPassword", false }; // Invalid password for valid username
        }
    }
}

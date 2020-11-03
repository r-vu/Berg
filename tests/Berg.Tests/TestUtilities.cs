using System;

namespace Berg.Tests {
    public static class TestUtilities {

        public readonly static Random RNG = new Random(123456789);
        private readonly static string ITEM_NAME_CHARS =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\'\"\\,./-()&[]: ";

        // This is the default list for Identity
        // Any changes will be in Startup.cs and should be noted here if needed
        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-3.1
        private readonly static string USER_NAME_CHARS =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        public static string RandomItemName() {
            int length = RNG.Next(256);
            char[] output = new char[length];

            for (int idx = 0; idx < output.Length; idx++) {
                output[idx] = ITEM_NAME_CHARS[RNG.Next(ITEM_NAME_CHARS.Length)];
            }

            return new string(output);
        }

        public static string RandomUserName() {
            int length = RNG.Next(256);
            char[] output = new char[length];

            for (int idx = 0; idx < output.Length; idx++) {
                output[idx] = USER_NAME_CHARS[RNG.Next(USER_NAME_CHARS.Length)];
            }

            return new string(output);
        }

    }
}

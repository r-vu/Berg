using Berg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace Berg.Tests {
    public static class TestUtilities {

        public readonly static Random RNG = new Random(123456789);
        private readonly static string VALID_CHARACTERS =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\'\"\\,./-()&[]: ";

        public static string RandomString() {
            int length = RNG.Next(256);
            char[] output = new char[length];

            for (int idx = 0; idx < output.Length; idx++) {
                output[idx] = VALID_CHARACTERS[RNG.Next(VALID_CHARACTERS.Length)];
            }

            return new string(output);
        }

    }
}

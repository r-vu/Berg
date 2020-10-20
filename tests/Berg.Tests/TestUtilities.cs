using Berg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace Berg.Tests {
    public static class TestUtilities {

        public readonly static Random RNG = new Random();

        public static string RandomString() {
            int length = RNG.Next(3, 12);
            return Path.GetRandomFileName().Replace(".", "").Substring(0, length);
        }

    }
}

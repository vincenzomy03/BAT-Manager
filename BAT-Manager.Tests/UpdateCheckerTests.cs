using System;
using BatManager.Services;

namespace BatManager.Tests
{
    public static class UpdateCheckerTests
    {
        public static void RunAllTests()
        {
            TestNewerVersionDetection();
            TestSameOrOlderVersionDetection();
            TestInvalidOrMalformedTags();

            Console.WriteLine("  [PASS] All UpdateChecker tests passed!");
        }

        private static void TestNewerVersionDetection()
        {
            Assert(UpdateChecker.IsNewerVersion("v1.2.1", "1.2.0"), "v1.2.1 should be newer than 1.2.0");
            Assert(UpdateChecker.IsNewerVersion("v1.3.0", "1.2.0"), "v1.3.0 should be newer than 1.2.0");
            Assert(UpdateChecker.IsNewerVersion("v2.0.0", "1.2.0"), "v2.0.0 should be newer than 1.2.0");
            Assert(UpdateChecker.IsNewerVersion("1.2.5", "1.2.0"), "1.2.5 (without v) should be newer than 1.2.0");
        }

        private static void TestSameOrOlderVersionDetection()
        {
            Assert(!UpdateChecker.IsNewerVersion("v1.2.0", "1.2.0"), "Same version should not report as newer");
            Assert(!UpdateChecker.IsNewerVersion("v1.1.0", "1.2.0"), "Older version should not report as newer");
            Assert(!UpdateChecker.IsNewerVersion("v1.0.0", "1.2.0"), "v1.0.0 should not be newer than 1.2.0");
        }

        private static void TestInvalidOrMalformedTags()
        {
            Assert(!UpdateChecker.IsNewerVersion(null, "1.2.0"), "Null latest tag should return false");
            Assert(!UpdateChecker.IsNewerVersion("", "1.2.0"), "Empty latest tag should return false");
            Assert(!UpdateChecker.IsNewerVersion("not-a-version", "1.2.0"), "Invalid version string should return false");
            Assert(!UpdateChecker.IsNewerVersion("v1.2.0", null), "Null current version should return false");
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion Failed: " + message);
            }
        }
    }
}

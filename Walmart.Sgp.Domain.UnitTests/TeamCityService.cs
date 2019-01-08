using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.UnitTests
{
    public static class TeamCityService
    {
        private static readonly Regex s_sanitizeReportParameter = new Regex(@"[^A-Z]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static void ReportTestStarted(string testName)
        {
            Report("##teamcity[testStarted name='{0}' captureStandardOutput='true']", Sanitize(testName));
        }        

        public static void ReportTestFinished(string testName)
        {
            Report("##teamcity[testFinished name='{0}']", Sanitize(testName));
        }

        public static void ReportTestFailed(string testName, string failureMessage, string stacktrace)
        {
            Report("##teamcity[testFailed name='{0}' message='{1} - {2}']", Sanitize(testName), Sanitize(failureMessage), Sanitize(stacktrace));
        }

        public static void Log(string messageName, string value)
        {
            Report("##teamcity[{0} '{1}']", messageName, value);
        }

        private static void Report(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        private static string Sanitize(string testName)
        {
            var result = s_sanitizeReportParameter.Replace(testName, "_");

            return result;
        }
    }
}

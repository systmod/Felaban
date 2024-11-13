using Microsoft.Extensions.Configuration;

namespace Common.Reporting
{
    public static class ReportClientConfig
    {
        public static void Initialize(IConfiguration configuration)
        {
            DefaultFolder = configuration["Reporting:DefaultFolder"];
            ReportServerUrl = configuration["Reporting:ServerUrl"];
            ReportServerUser = configuration["Reporting:Username"];
            ReportServerPass = configuration["Reporting:Password"];
            ReportServerDomain = configuration["Reporting:Domain"];
        }

        internal static string ReportServerUrl { get; set; }
        internal static string ReportServiceUrl => $"{ReportServerUrl}/reportserver/ReportExecution2005.asmx?wsdl";
        internal static string DefaultFolder { get; set; }
        internal static string ReportServerUser { get; set; }
        internal static string ReportServerPass { get; set; }
        internal static string ReportServerDomain { get; set; }

    }
}

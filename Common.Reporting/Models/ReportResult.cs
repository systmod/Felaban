
using Common.Reporting.Services;

namespace Common.Reporting.Models
{
    public class ReportResult
    {
        public ReportResult(byte[] content)
        {
            Content = content;
        }

        public byte[] Content { get; private set; }
        public string MimeType { get; set; }
        public string Encoding { get; set; }
        public string Extension { get; set; }
        public string[] Streams { get; set; }
        public Warning[] Warnings { get; set; }

    }
}

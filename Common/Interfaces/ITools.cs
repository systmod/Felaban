using Common.Reporting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common
{
    public interface ITools
    {
        string ResourcesPath { get; }
        string ResourcesPathUri { get; }

        string GetHtmlTemplate<T>(string template, T model);
        string GetFilePath(params string[] paths);
        string GetFileText(string filename);

        FileContentResult DownloadFile(string path, string mimetype = default);
        FileContentResult TextToFileResult(string text, string mimetype = default);
        bool VerificaCedula(string validarCedula);
        bool VerificaIdentificacion(string identificacion, bool cedula = false);
        bool VerificaPersonaJuridica(string validarCedula);
        bool VerificaSectorPublico(string validarCedula);
        Task<byte[]> DigitalSignPDFAsync(string filename);

        Task<IOperationResult<byte[]>> GetPDFAsync(ReportBuilder report, string path = null);
        Task<IOperationResult<byte[]>> GeneratePDFAsync(ReportBuilder report, string path = null);
        Task<IOperationResult> SendMailAsync<T>(T model, string email, string subject, string template, params Attachment[] attachments);
        Task<IOperationResult> SendMailAsync(string recipients, string subject, string html, params Attachment[] attachments);
        Task<IOperationResult> SendMailAsyncFc<T>(T model, string email, string subject, string template, params Attachment[] attachments);
        Task<IOperationResult> SendMailAsyncFc(string recipients, string subject, string html, params Attachment[] attachments);
        Task<IOperationResult> SendMailActividades(string recipients, string subject, string html);
    }
}
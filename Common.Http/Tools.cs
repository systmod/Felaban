using Common.Reporting;
using DinkToPdf.Contracts;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//following bouncy castle libraries are to be referenced
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class Tools : ITools
    {
        public string ResourcesPathUri => _config["Resources"];
        public string ResourcesPath => ResourcesPathUri?.Replace("/", "\\");

        protected static IWebHostEnvironment HostingEnvironment { get; private set; }

        private readonly IConfiguration _config;
        private readonly IConverter _converter;
        private readonly ILogger<Tools> _logger;

        public Tools(IWebHostEnvironment hostingEnvironment, IConfiguration config, IConverter converter, ILogger<Tools> logger)
        {
            HostingEnvironment = hostingEnvironment;
            _converter = converter;
            _config = config;
            _logger = logger;
        }

        public async Task<IOperationResult<byte[]>> GeneratePDFAsync(ReportBuilder report, string path = null)
        {

            var data = await report.RenderAsync("PDF");

            if (data != null)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    File.WriteAllBytes(path, data.Content);

                    var result = await DigitalSignPDFAsync(path);

                    return result.ToResult();
                }

                return data.Content?.ToResult();
            }

            return new OperationResult<byte[]>(HttpStatusCode.InternalServerError, "Hubo un error al generar el documento electronico.");
        }


        public async Task<byte[]> DigitalSignPDFAsync(string filename)
        {
            string KEYSTORE = System.IO.Path.Combine(HostingEnvironment.ContentRootPath, _config["Certificate:KeyFile"]);
            char[] PASSWORD = _config["Certificate:Password"].ToCharArray();

            var pk12 = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);

            string alias = null;
            foreach (object a in pk12.Aliases)
            {
                alias = ((string)a);
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;

            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            string DEST = filename.Replace(".pdf", ""); ;
            string SRC = filename;

            using (var reader = new PdfReader(SRC))
            {
                using (var stream = new MemoryStream())
                {
                    var signer = new PdfSigner(reader, new FileStream(DEST, FileMode.Create), new StampingProperties());

                    var appearance = signer.GetSignatureAppearance();
                    appearance.SetReason("Firma de Seguridad")
                        .SetLocation("Guayaquil, Ecuador")
                        .SetContact("FCPC-CCV-CTE")
                        .SetLayer2Text("Emilio Legarda Izquierdo")
                        .SetSignatureCreator("Fondo Facil")
                        .SetLayer2FontColor(iText.Kernel.Colors.ColorConstants.BLUE)
                        .SetPageRect(new Rectangle(30, 100, 200, 100))
                        .SetPageNumber(4);

                    signer.SetFieldName("Firma");

                    IExternalSignature pks = new PrivateKeySignature(pk, DigestAlgorithms.SHA256);

                    signer.SignDetached(pks, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);


                    var result = System.IO.File.ReadAllBytes(DEST);

                    return await Task.FromResult(result);
                }
            }
        }

        public async Task<IOperationResult<byte[]>> GetPDFAsync(ReportBuilder report, string path = null)
        {
            try
            {
                if (report != null)
                {
                    return await GeneratePDFAsync(report, path);
                }

                return new OperationResult<byte[]>(HttpStatusCode.NotFound, "Hubo un error al generar el archivo PDF");
            }
            catch (Exception ex)
            {
                return new OperationResult<byte[]>(HttpStatusCode.NotFound, "Hubo un error al generar el archivo PDF", default, ex.ToString());

            }
        }

        public async Task<IOperationResult> SendMailAsync<T>(T model, string email, string subject, string template, params Attachment[] attachments)
        {
            var html = GetHtmlTemplate(template, model);

            if (!string.IsNullOrEmpty(html))
            {
                return await SendMailAsync($"{email}", subject, html, attachments);
            }

            return new OperationResult(HttpStatusCode.InternalServerError, "No se envió la notificación al usuario");
        }

        public async Task<IOperationResult> SendMailAsyncFc<T>(T model, string email, string subject, string template, params Attachment[] attachments)
        {
            var html = GetHtmlTemplate(template, model);

            if (!string.IsNullOrEmpty(html))
            {
                return await SendMailAsyncFc($"{email}", subject, html, attachments);
            }

            return new OperationResult(HttpStatusCode.InternalServerError, "No se envió la notificación al usuario");
        }

        public string GetFileText(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                return System.IO.File.ReadAllText(filename);
            }

            return default;
        }


        public string GetFilePath(params string[] paths)
        {
            string path = HostingEnvironment.ContentRootPath;

            foreach (var item in paths)
            {
                path = System.IO.Path.Combine(path, item);
            }

            return path;
        }


        public string GetHtmlTemplate<T>(string template, T model)
        {
            if (!template.Contains("."))
            {
                template += ".html";
            }

            string path = System.IO.Path.Combine(HostingEnvironment.ContentRootPath, "Templates", template);
            string html = default;
            if (!string.IsNullOrEmpty(path))
            {
                html = File.ReadAllText(path);

                var type = typeof(T);
                var props = type.GetProperties();

                foreach (var item in props)
                {
                    try
                    {
                        var name = item.Name.ToUpper();
                        var original = item.GetValue(model);
                        if (original is decimal)
                        {
                            // Si es decimal lo redondeo a dos decimales
                            original = decimal.Round((decimal)(original), 2);
                        }
                        var value = HttpUtility.HtmlEncode($"{original}");

                        html = html.Replace($"{{{name}}}", $"{value}");
                    }
                    catch { }
                }
            }

            return html;
        }


        public async Task<IOperationResult> SendMailAsync(string recipients, string subject, string html, params Attachment[] attachments)
        {
            try
            {
                // Carga de Parametros:
                var username = _config["Emailing:Username"];
                var password = _config["Emailing:Password"];

                var replyTo = _config["Emailing:ReplyTo"];

                var port = Convert.ToInt32(_config["Emailing:Port"]);
                var host = _config["Emailing:Host"];
                var enableSsl = _config["Emailing:EnableSSL"];
                var sender = _config["Emailing:Sender"];

                // Armamos el mensaje base para el correo
                var mailMessage = new MailMessage
                {
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                    Sender = new MailAddress(sender, "Notificaciones Binasystem"),
                    From = new MailAddress(sender, "Notificaciones Binasystem")
                };

                // Se configura para que las respuestas se reciban a este correo
                if (!string.IsNullOrEmpty(replyTo))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(replyTo, "Servicio al Participe Binasystem"));
                }

                // Si tiene adjuntos los agrega a la lista de adjuntos
                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(item);
                    }
                }

                // Listamos a todos los destinatarios, siempre y cuando no esten vacios los campos
                foreach (var recipient in recipients.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    mailMessage.To.Add(new MailAddress(recipient));
                }

                // Configuramos el cliente de correo
                var client = new SmtpClient(host)
                {
                    EnableSsl = true,
                    Host = host,
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(username, password)
                };

                client.Send(mailMessage);

                _logger.LogInformation("Se envio un correo electrónico a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(HttpStatusCode.OK, "El correo electrónico ha sido enviado exitosamente!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al enviar el correo a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(ex);
            }
        }

        public async Task<IOperationResult> SendMailAsyncFc(string recipients, string subject, string html, params Attachment[] attachments)
        {
            try
            {
                // Carga de Parametros:
                var username = _config["EmailingFc:Username"];
                var password = _config["EmailingFc:Password"];

                var replyTo = _config["EmailingFc:ReplyTo"];

                var port = Convert.ToInt32(_config["EmailingFc:Port"]);
                var host = _config["EmailingFc:Host"];
                var enableSsl = _config["EmailingFc:EnableSSL"];
                var sender = _config["EmailingFc:Sender"];

                // Armamos el mensaje base para el correo
                var mailMessage = new MailMessage
                {
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                    Sender = new MailAddress(sender, "Notificaciones Binasystem"),
                    From = new MailAddress(sender, "Notificaciones Binasystem")
                };

                // Se configura para que las respuestas se reciban a este correo
                if (!string.IsNullOrEmpty(replyTo))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(replyTo, "Servicio al Participe Binasystem"));
                }

                // Si tiene adjuntos los agrega a la lista de adjuntos
                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(item);
                    }
                }

                // Listamos a todos los destinatarios, siempre y cuando no esten vacios los campos
                foreach (var recipient in recipients.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    mailMessage.To.Add(new MailAddress(recipient));
                }

                // Configuramos el cliente de correo
                var client = new SmtpClient(host)
                {
                    EnableSsl = true,
                    Host = host,
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(username, password)
                };

                client.Send(mailMessage);

                _logger.LogInformation("Se envio un correo electrónico a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(HttpStatusCode.OK, "El correo electrónico ha sido enviado exitosamente!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al enviar el correo a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(ex);
            }
        }

        public async Task<IOperationResult> SendMailActividades(string recipients, string subject, string html)
        {
            /*Console.WriteLine(recipients, subject, html, attachments);
            return new OperationResult(HttpStatusCode.OK);*/
            try
            {
                // Carga de Parametros:
                var username = _config["Emailing:Username"];
                var password = _config["Emailing:Password"];

                var replyTo = _config["Emailing:ReplyTo"];

                var port = Convert.ToInt32(_config["Emailing:Port"]);
                var host = _config["Emailing:Host"];
                var enableSsl = _config["Emailing:EnableSSL"];
                var sender = _config["Emailing:Sender"];

                // Armamos el mensaje base para el correo
                var mailMessage = new MailMessage
                {
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                    Sender = new MailAddress(sender, "Notificaciones FCPC-CCV-CTE"),
                    From = new MailAddress(sender, "Notificaciones FCPC-CCV-CTE")
                };

                // Se configura para que las respuestas se reciban a este correo
                if (!string.IsNullOrEmpty(replyTo))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(replyTo, "Servicio al Participe FCPC-CCV-CTE"));
                }

                //// Si tiene adjuntos los agrega a la lista de adjuntos
                //if (attachments != null)
                //{
                //    foreach (var item in attachments)
                //    {
                //        mailMessage.Attachments.Add(item);
                //    }
                //}

                // Listamos a todos los destinatarios, siempre y cuando no esten vacios los campos
                foreach (var recipient in recipients.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    mailMessage.To.Add(new MailAddress(recipient));
                }

                // Configuramos el cliente de correo
                var client = new SmtpClient(host)
                {
                    EnableSsl = true,
                    Host = host,
                    Port = port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(username, password)
                };

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation("Se envio un correo electrónico a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(HttpStatusCode.OK, "El correo electrónico ha sido enviado exitosamente!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al enviar el correo a {0} con el titulo: {1}", recipients, subject);

                return new OperationResult(ex);
            }
        }

        public FileContentResult DownloadFile(string path, string mimetype = default)
        {
            if (File.Exists(path))
            {
                byte[] fileContents = File.ReadAllBytes(path);

                if (Encoding.UTF8.GetString(fileContents, 0, 10).StartsWith("data:"))
                {
                    string text = File.ReadAllText(path);

                    return this.TextToFileResult(text, mimetype);
                }

                return new FileContentResult(fileContents, mimetype ?? "text/plain");
            }

            string[] paths = new string[] { "Templates", "void.png" };
            path = this.GetFilePath(paths);
            return new FileContentResult(File.ReadAllBytes(path), "image/png");
        }



        public string DownloadContrato(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var file = System.IO.File.ReadAllBytes(path);

                var base64 = Convert.ToBase64String(file);

                return base64;
            }
            else
            {
                path = GetFilePath("Templates", "void.png");
                var data = System.IO.File.ReadAllBytes(path);
                return "";
            }
        }

        public FileContentResult TextToFileResult(string text, string mimetype = default)
        {

            byte[] fileContents = Array.Empty<byte>();

            if (text.StartsWith("data:"))
            {
                var index = text.IndexOf(";");
                mimetype = text[5..index];
                var startIndex = text.IndexOf(",") + 1;
                var content = text[startIndex..];

                try
                {
                    fileContents = Convert.FromBase64String(content);
                }
                catch
                {
                    fileContents = Encoding.UTF8.GetBytes(content);
                }
            }

            if (((fileContents == null) || (fileContents.Length == 0)) && !string.IsNullOrEmpty(text))
            {
                fileContents = Encoding.UTF8.GetBytes(text);
            }

            if (mimetype == null)
            {
                mimetype = "text/plain";
            }

            return new FileContentResult(fileContents, mimetype);

        }

        public bool VerificaIdentificacion(string identificacion, bool cedula = false)
        {
            bool estado = false;

            if ((cedula && identificacion.Length == 10) || (identificacion.Length == 10 && identificacion.Length == 13))
            {
                var valced = identificacion.Trim().ToCharArray();
                var provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));

                if (provincia > 0 && provincia < 25)
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                    {
                        estado = VerificaCedula(new string(valced));
                    }
                    else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        estado = VerificaSectorPublico(new string(valced));
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {
                        estado = VerificaPersonaJuridica(new string(valced));
                    }
                }
            }

            return estado;
        }

        public bool VerificaCedula(string validarCedula)
        {
            if (validarCedula.Length != 13 && validarCedula.Length != 10)
                return false;

            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        public bool VerificaPersonaJuridica(string validarCedula)
        {
            if (validarCedula.Length != 13)
                return false;

            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 9; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }
                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[9].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool VerificaSectorPublico(string validarCedula)
        {
            if (validarCedula.Length != 13)
                return false;

            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


    }
}

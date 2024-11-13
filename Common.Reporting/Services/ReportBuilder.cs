using Common.Reporting.Models;
using Common.Reporting.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Reporting
{
    public class ReportBuilder
    {
        public ReportBuilder(string reportPath)
        {
            ReportPath = reportPath;
        }

        protected string ReportPath { get; private set; }

        const string HistoryId = null;


        private readonly List<ParameterValue> parameters = new List<ParameterValue>();

        public ParameterValue AddParameter(string key, object value)
        {
            var parameter = new ParameterValue { Name = key, Value = Convert.ToString(value) };
            parameters.Add(parameter);
            return parameter;
        }

        public async Task<ReportResult> RenderAsync(string reportType)
        {
            var rsClient = await CreateClientAsync();
            var trustedHeader = new TrustedUserHeader();
            var loadReponse = await LoadReportAsync(rsClient, trustedHeader);

            await AddParametersToTheReportAsync(rsClient, loadReponse.ExecutionHeader, trustedHeader);

            var response = await RenderReportByteArrayAsync(loadReponse.ExecutionHeader, trustedHeader, rsClient, reportType, ReportWidth, ReportHeight);

            return await Task.FromResult(new ReportResult(response.Result)
            {
                Extension = response.Extension,
                Encoding = response.Encoding,
                MimeType = response.MimeType,
                Warnings = response.Warnings,
                Streams = response.StreamIds
            });
        }


        private async Task<RenderResponse> RenderReportByteArrayAsync(ExecutionHeader execHeader, TrustedUserHeader trustedHeader,
           ReportExecutionServiceSoapClient rs, string format, string width, string height)
        {
            string deviceInfo = $"<DeviceInfo><PageHeight>{height}</PageHeight><PageWidth>{width}</PageWidth><PrintDpiX>300</PrintDpiX><PrintDpiY>300</PrintDpiY></DeviceInfo>";

            var renderRequest = new RenderRequest(execHeader, trustedHeader, format, deviceInfo);

            //get report bytes
            RenderResponse response = await rs.RenderAsync(renderRequest);
            return response;
        }

        public string ReportWidth = "8.27in";
        public string ReportHeight = "11.69in";

        private async Task<ReportExecutionServiceSoapClient> CreateClientAsync()
        {
            // Para que funcione el servicio, debe Habilitarse Anonymous impersonando al servicio remoto.
            // En este caso usamos la cuenta de Servicio del AppPoolIdentity que es la misma del otro server.
            var rsBinding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                Security = { Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.Ntlm } }
            };
            // So we can download reports bigger than 64 KBytes
            // See https://stackoverflow.com/questions/884235/wcf-how-to-increase-message-size-quota
            rsBinding.MaxBufferPoolSize = 20000000;
            rsBinding.MaxBufferSize = 20000000;
            rsBinding.MaxReceivedMessageSize = 20000000;

            var rsEndpointAddress = new EndpointAddress(ReportClientConfig.ReportServiceUrl);
            var rsClient = new ReportExecutionServiceSoapClient(rsBinding, rsEndpointAddress);

            //await rsClient.LogonUserAsync(ReportClient.ReportServerUser, ReportClient.ReportServerPass, null);

            // Set user name and password
            rsClient.ClientCredentials.Windows.AllowedImpersonationLevel =
                System.Security.Principal.TokenImpersonationLevel.Impersonation;

            rsClient.ClientCredentials.Windows.ClientCredential =
                new System.Net.NetworkCredential(ReportClientConfig.ReportServerUser, ReportClientConfig.ReportServerPass, "");

            return await Task.FromResult(rsClient);
        }

        private async Task<SetExecutionParametersResponse> AddParametersToTheReportAsync(ReportExecutionServiceSoapClient rs, ExecutionHeader executionHeader, TrustedUserHeader trustedHeader)
        {
            SetExecutionParametersResponse setParamsResponse = await rs.SetExecutionParametersAsync(executionHeader, trustedHeader, parameters.ToArray(), "en-US");

            return setParamsResponse;
        }

        private async Task<LoadReportResponse> LoadReportAsync(ReportExecutionServiceSoapClient rs, TrustedUserHeader trustedHeader)
        {
            // Get the report and set the execution header.
            // Failure to set the execution header will result in this error: "The session identifier is missing. A session identifier is required for this operation."
            // See https://social.msdn.microsoft.com/Forums/sqlserver/en-US/17199edb-5c63-4815-8f86-917f09809504/executionheadervalue-missing-from-reportexecutionservicesoapclient
            LoadReportResponse loadReponse = await rs.LoadReportAsync(trustedHeader, $"/{ReportClientConfig.DefaultFolder}/{ReportPath}", HistoryId);

            return loadReponse;
        }


    }

}

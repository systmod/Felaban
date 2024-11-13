using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EasyFCPC 
{
	public class GraphHelper
	{
		private static GraphServiceClient graphClient;
		private static IConfiguration config;

		private static string TenantId => config?["GraphClient:TenantID"];
		private static string ClientId => config?["GraphClient:ClientID"];
		private static string ClientSecret => config?["GraphClient:ClientSecret"];
		private static string UserId => config?["GraphClient:UserID"];

		public static void Configure(IConfiguration configuration)
		{
			config = configuration;

		}

		public void OnPostAdd(CalcModel calcModel)
		{
			try
			{
				svcCalculator_PortClient client = new svcCalculator_PortClient();
				client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(Globals.WSUsername, Globals.WSPassword, Globals.WSDomain);
				//>>I need to be able to do this     
				client.Endpoint.Binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Ntlm;
				client.Endpoint.Binding.Security.Transport.proxycredentialtype = System.ServiceModel.HttpProxyCredentialType.Ntlm;
				//<<I need to be able to do this     
				FncAdd fncAdd = new FncAdd(calcModel.ValueA, calcModel.ValueB);
				FncAdd_Result fncAddResult = client.FncAdd(fncAdd);
				calcModel.Result = fncAddResult.return_value; ViewData["OutputMessage"] = calcModel.Result;
			}
			catch (Exception ex)
			{
				ViewData["OutputMessage"] = ex.Message;
			}
		}


		
		// <CreateEventSnippet>
				public static async Task SendMailAsync(
			string recipients,
			string subject,
			string body = null)
		{
			graphClient = await GetClientAsync();

			var message = new Message
			{
				Subject = subject,
				Body = new ItemBody
				{
					ContentType = BodyType.Text,
					Content = body
				},
				ToRecipients = new List<Recipient>()
				{
					new Recipient
					{
						EmailAddress = new EmailAddress
						{
							Address = recipients
						}
					}
				}
			};

			var saveToSentItems = true;

			await graphClient.Users[UserId]
				.SendMail(message, saveToSentItems)
				.Request()
				.PostAsync();

		}
		// </CreateEventSnippet>


		private static async Task<GraphServiceClient> GetClientAsync()
		{
            //The following scope is required to acquire the token
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                .Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token"))
                .Build();

            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            var authResult = await confidentialClient
                    .AcquireTokenForClient(scopes)
                    .ExecuteAsync().ConfigureAwait(false);

            var token = authResult.AccessToken;

            // Build the Microsoft Graph client. As the authentication provider, set an async lambda
            // which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
            // and inserts this access token in the Authorization header of each API request. 
            return new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    // Add the access token in the Authorization header of the API request.
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await Task.CompletedTask;
                })
            );
        }
	}
}
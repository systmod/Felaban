using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Common.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Storage.Handlers
{
    /// <summary>   
    /// FileHandler for Blob Service
    /// Documentation References:  
    /// - What is a Storage Account - http://azure.microsoft.com/en-us/documentation/articles/storage-whatis-account/ 
    /// - Getting Started with Blobs - http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/ 
    /// - Blob Service Concepts - http://msdn.microsoft.com/en-us/library/dd179376.aspx  
    /// - Blob Service REST API - http://msdn.microsoft.com/en-us/library/dd135733.aspx 
    /// - Blob Service C# API - http://go.microsoft.com/fwlink/?LinkID=398944 
    /// - Delegating Access with Shared Access Signatures - http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-shared-access-signature-part-1/ 
    /// </summary> 
    public class FileHandler
    {
        private IConfiguration _config;
        private ILogger<FileHandler> _logger;

        public FileHandler(IConfiguration config, ILogger<FileHandler> logger)
        {
            _config = config;
            _logger = logger;

        }

        public async Task<List<Uri>> GetFiles()
        {
            var container = await GetContainerAsync();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 

            // Gets all Block Blobs in the blobContainerName and passes them to the view
            List<Uri> allBlobs = new List<Uri>();
            foreach (BlobItem blob in container.GetBlobs())
            {
                if (blob.Properties.BlobType == BlobType.Block)
                    allBlobs.Add(container.GetBlobClient(blob.Name).Uri);
            }

            return allBlobs;
        }

        public async Task<IOperationResult<string>> GetFile(string filename)
        {
            try
            {
                var container = await GetContainerAsync();

                var blob = container.GetBlobClient(filename);

                return await $"{blob.Uri}".ToResultAsync(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"", ex.Message);

                return ex.ToResult<string>();
            }
        }

        public async Task<IOperationResult> UploadAsync(string filename, Stream file)
        {
            try
            {
                var container = await GetContainerAsync();
                file.Position = 0;
                BlobClient blob = container.GetBlobClient(filename);
                var result = await blob.UploadAsync(file, true);

                if (!string.IsNullOrEmpty(result?.Value?.VersionId))
                {
                    return await result.Value.ToResultAsync(System.Net.HttpStatusCode.Created, "El archivo se ha guardado con exito!");
                }

                return new OperationResult(System.Net.HttpStatusCode.BadRequest, "Ocurrió un error al guardar el archivo especificado!");
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }

        public async Task<IOperationResult> DeleteBlobAsync(string filename)
        {
            try
            {
                var container = await GetContainerAsync();
                BlobClient blob = container.GetBlobClient(filename);
                var result = await blob.DeleteAsync();

                return await result.ToResultAsync(System.Net.HttpStatusCode.OK, "El archivo se ha eliminado con exito!");
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }

        private async Task<BlobContainerClient> GetContainerAsync()
        {
            // Retrieve storage account information from connection string
            // How to create a storage connection string - http://msdn.microsoft.com/en-us/library/azure/ee758697.aspx
            var client = new BlobServiceClient(_config["Storage:ConnectionString"]);
            var containerName = _config["Storage:ContainerName"];

            var container = client.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            return container;
        }
    }

}

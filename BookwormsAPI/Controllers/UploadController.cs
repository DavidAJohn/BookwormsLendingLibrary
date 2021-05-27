using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BookwormsAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BookwormsAPI.Controllers
{
    public class UploadController : BaseApiController
    {
        private readonly string azureConnectionString;
        private readonly string azureContainersAllowed;
        private readonly string permittedFileExtensions;
        private readonly long fileSizeLimit;
        public UploadController(IConfiguration configuration)
        {
            azureConnectionString = configuration["AzureStorageConnectionString"];
            azureContainersAllowed = configuration.GetValue<string>("AzureContainersAllowed");
            permittedFileExtensions = configuration.GetValue<string>("FileUploadTypesAllowed");
            fileSizeLimit = configuration.GetValue<long>("MaxFileUploadSize");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();

            var requestedContainer = formCollection["container"].ToString();
            var fileExtension = Path.GetExtension(file.FileName.ToLowerInvariant());

            // check the file type is allowed
            if (!permittedFileExtensions.Contains(fileExtension) && fileExtension != "")
            {
                Console.WriteLine("The file type '" + fileExtension + "' is not allowed");
                return BadRequest(new ApiResponse(400));
            }

            // check the file size (in bytes)
            if (file.Length > fileSizeLimit)
            {
                Console.WriteLine("The file size was too large");
                return BadRequest(new ApiResponse(400));
            }

            // check the file name length isn't excessive
            if (file.FileName.Length > 75)
            {
                Console.WriteLine("The file name is too long: " + file.FileName.Length + " characters");
                return BadRequest(new ApiResponse(400));
            }

            // check container name isn't empty
            if (requestedContainer.Length == 0)
            {
                Console.WriteLine("Azure container name is empty");
                return BadRequest(new ApiResponse(400));
            }

            // check requested container name is in an allowed set of names
            if (!azureContainersAllowed.Contains(requestedContainer.ToLowerInvariant()))
            {
                Console.WriteLine("Invalid azure container name supplied: " + requestedContainer);
                return BadRequest(new ApiResponse(400));
            }

            if (file.Length > 0)
            {
                var azureContainer = new BlobContainerClient(azureConnectionString, requestedContainer);
                var createResponse = await azureContainer.CreateIfNotExistsAsync();

                // in case the container doesn't exist
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                {
                    await azureContainer.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                }

                // generate a unique upload file name
                // [original_filename_without_extension]_[8_random_chars].[original_filename_extension]
                // eg. filename_xgh38tye.jpg
                var fileName = HttpUtility.HtmlEncode(Path.GetFileNameWithoutExtension(file.FileName)) + 
                    "_" + Path.GetRandomFileName().Substring(0,8) + Path.GetExtension(file.FileName);

                var blob = azureContainer.GetBlobClient(fileName);
                //await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                // set the content type (which may or may not have been provided by the client)
                var blobHttpHeader = new BlobHttpHeaders();

                if (file.ContentType != null)
                {
                    blobHttpHeader.ContentType = file.ContentType;
                }
                else 
                {
                    blobHttpHeader.ContentType = fileExtension switch
                    {
                        ".jpg"  => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png"  => "image/png",
                        _ => null
                    };
                }
                
                using (var fileStream = file.OpenReadStream())
                {
                    await blob.UploadAsync(fileStream, blobHttpHeader);
                }

                return Ok(new {
                    filename = blob.Name,
                    container = blob.BlobContainerName,
                    uri = blob.Uri
                });
            }

            Console.WriteLine("The file could not be uploaded");
            return BadRequest(new ApiResponse(400));
        }
    }
}
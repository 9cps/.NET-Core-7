using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Services
{
    public static class S3FileExtensionAWS
    {
        // AWS S3 configuration parameters
        private const string bucketName = "YOUR_BUCKET_NAME";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2; // Replace with your desired region
        private static IAmazonS3 s3Client;

        static S3FileExtensionAWS()
        {
            // Initialize the S3 client
            s3Client = new AmazonS3Client(bucketRegion);
        }

        public static async Task UploadBase64FileToS3(string base64File, string fileName)
        {
            try
            {
                byte[] fileData = Convert.FromBase64String(base64File);
                var fileTransferUtility = new TransferUtility(s3Client);

                // Upload the file data
                await fileTransferUtility.UploadAsync(new MemoryStream(fileData), bucketName, fileName);
            }
            catch (AmazonS3Exception ex)
            {
                // Handle the Amazon S3 specific exception
                Console.WriteLine($"Error uploading file to Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine($"Error uploading file: {ex.Message}");
            }
        }

        public static async Task DownloadFileFromS3(string fileName, string destinationFilePath)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                using (var response = await s3Client.GetObjectAsync(request))
                using (var fileStream = File.Create(destinationFilePath))
                {
                    await response.ResponseStream.CopyToAsync(fileStream);
                }
            }
            catch (AmazonS3Exception ex)
            {
                // Handle the Amazon S3 specific exception
                Console.WriteLine($"Error downloading file from Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }

        public static async Task<string> PublicUrlFileFromS3(string fileName)
        {
            string result = string.Empty;
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    Expires = DateTime.Now.AddMinutes(30) // Set the expiration time for the URL
                };

                // Generate a public URL for the file
                string url = s3Client.GetPreSignedURL(request);
                result = url;
            }
            catch (AmazonS3Exception ex)
            {
                // Handle the Amazon S3 specific exception
                Console.WriteLine($"Error generating public URL from Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine($"Error generating public URL: {ex.Message}");
            }

            return result;
        }

    }
}

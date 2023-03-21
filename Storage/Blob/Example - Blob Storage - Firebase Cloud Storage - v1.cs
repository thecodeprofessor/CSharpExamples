using Azure.Storage.Blobs;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliedActivity3.Services
{
    class BlobStorageFirebaseCloudStorage : IBlobStorage
    {
        //NuGet Packages: FirebaseStorage.net by Step Up Labs, Inc.

        //https://firebase.google.com/products/storage
        //Docs: https://firebase.google.com/docs/storage
        //Console: https://console.firebase.google.com/u/0/project/_/storage/files
        //API Keys: https://firebase.google.com/docs/projects/api-keys
        //Credentials: https://console.cloud.google.com/apis/credentials?pli=1

        FirebaseStorage service = new FirebaseStorage(BucketName, new FirebaseStorageOptions { AuthTokenAsyncFactory = () => Task.FromResult(APIKey) });

        private static string BucketName => "bucketnamehere";
        private static string APIKey => "apikeyhere";
        private static string Container => "containernamehere";

        public async Task<MemoryStream> DownloadStreamAsync(string name)
        {
            try
            {
                FirebaseStorageReference blob = service.Child(Container).Child(name);
                Debug.
                if (blob != null)
                {
                    var stream = new MemoryStream();
                    var url = await blob.GetDownloadUrlAsync();

                    if (!string.IsNullOrEmpty(url))
                        stream = await DownloadFile(url);

                    stream.Position = 0;

                   
                    return stream;
                }
            }
            catch (Exception e)
            {
                //Catch errors here.
            }

            return null;
        }


        public async Task<MemoryStream> DownloadFile(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        public async Task<List<string>> ListBlobsAsync(string prefix, int? size = null)
        {
            throw new NotImplementedException();
        }

        public async Task UploadStreamAsync(string name, MemoryStream stream)
        {
            try
            {
                stream.Position = 0;
                FirebaseStorageReference blob = service.Child(Container).Child(name);
                await blob.PutAsync(stream);
            }
            catch (Exception e)
            {
                //Catch errors here.
            }
        }
    }
}

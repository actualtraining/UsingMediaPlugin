
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Media;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Plugin.Media.Abstractions;
using System.Diagnostics;

namespace UsingMediaPlugin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //private RestClient _client;

        /*public static void upload(MediaFile mediaFile)
        {
            
        }*/

        //private MediaFile MyMediaFile;

        private async void btnChoose_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            
            

            /*var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");*/

            /*byte[] myData;
            using (MemoryStream ms = new MemoryStream())
            {
                file.GetStream().CopyTo(ms);
                myData = ms.ToArray();
            }


            _client = new RestClient("http://192.168.8.101/SampleBackendAPI/");
            var request = new RestRequest("api/Upload/PostUserImage", Method.POST);
            //request.AddFile("sample1.jpg", myData, "sample1.jpg");
            request.AddBody(new { myFile = myData });
            request.AddHeader("Content-Type", "multipart/form-data");*/


            //var response = await _client.Execute(request);
            //await DisplayAlert("Keterangan", "Berhasil Upload Data !", "OK");
            //MyMediaFile = file;

            try
            {
                var media = CrossMedia.Current;
                var file = await media.PickPhotoAsync();

                
                while (file.GetStream().Length==0)
                {
                    System.Threading.Tasks.Task.Delay(2).Wait();
                }

                
                byte[] myData;
                using (var memoryStream = new MemoryStream())
                {
                    file.GetStream().CopyTo(memoryStream);
                    file.Dispose();
                    myData = memoryStream.ToArray();
                }

                var randName = Guid.NewGuid().ToString().Substring(0, 8) + ".jpg";
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(myData);
                content.Add(baContent, "File", randName);
                var response = await client.PostAsync("http://192.168.8.100/SampleBackendAPI/api/Upload/PostUserImage",content);
                var responsestr = response.Content.ReadAsStringAsync().Result;

                if(response.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    myImage.Source = string.Format("http://192.168.8.100/SampleBackendAPI/Images/{0}",randName);
                    await DisplayAlert("Hasil : ", responsestr, "OK");
                }
               
                //StreamContent scontent = new StreamContent(file.GetStream());
                /*scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "newimage",
                    Name = "image"
                };*/
                //scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                /*var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                multi.Add(scontent);
                client.BaseAddress = new Uri("http://192.168.8.101/SampleBackendAPI/");
                var result = client.PostAsync("api/Upload/PostUserImage", multi).Result;
                await DisplayAlert("Hasil : ", result.ReasonPhrase, "OK");*/
            }
            catch (Exception ex)
            {
                await DisplayAlert("Kesalahan : ", ex.Message, "OK");
            }
           

            /*myImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });*/



        }

        private void btnUpload_Clicked(object sender, EventArgs e)
        {

        }
    }
}

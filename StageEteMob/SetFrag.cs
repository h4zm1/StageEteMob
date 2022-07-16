using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StageEteMob
{
    public class SetFrag : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_main, container, false);

            MaterialButton btn = fragmentView.FindViewById<MaterialButton>(Resource.Id.button1);
            btn.Click += initEvent;

            return fragmentView;
        }
        private void initEvent(object sender, EventArgs eventArgs)
        {
            AppCompatEditText nomET = View.FindViewById<AppCompatEditText>(Resource.Id.nomTF);
            AppCompatEditText telET = View.FindViewById<AppCompatEditText>(Resource.Id.telTF);
            AppCompatEditText paysET = View.FindViewById<AppCompatEditText>(Resource.Id.paysTF);

            Console.WriteLine("*******************   " + nomET.Text);
            var data = new
            {
                Tel = telET.Text,
                Nom = nomET.Text,
                Pays = paysET.Text
            };
            var json = JsonConvert.SerializeObject(data);
            selector(json);
        }

        private async void selector(string val)
        {
            //await CallAPI();

            await SetAPI(val);
        }

        private async Task CallAPI()
        {
            try
            {
                string uri = "";

                //only for testing with current emulator
                if (Build.Hardware.Contains("ranchu"))
                {
                    uri = "https://10.0.2.2:44317/api/Values";
                    Console.WriteLine("********* ranchu *******");
                }
                else
                    uri = "https://192.168.1.2:45456/api/Values";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient httpClient = new HttpClient(clientHandler);

                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);
                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);

                    Console.WriteLine("************** Result: " + prettyJson);
                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
            }
        }

        private async Task SetAPI(string valueToSend)
        {
            string uri = "";
            //only for testing with current emulator
            if (Build.Hardware.Contains("ranchu"))
                uri = "https://10.0.2.2:44317/api/Values";
            else
                uri = "https://192.168.1.2:45456/api/Values";

            //bypassing SSLHandshakeException
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var client = new HttpClient(clientHandler);
            var httpContent = new StringContent("");
            var result = await client.PostAsync(uri + "?fromMob=" + valueToSend, httpContent);

            if (result.IsSuccessStatusCode)
            {
                var tokenJson = await result.Content.ReadAsStringAsync();
            }
            else
                Console.WriteLine("************** FAILED: " + result.ReasonPhrase);
        }
    }
}
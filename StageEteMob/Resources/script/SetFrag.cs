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
using Google.Android.Material.Tabs;

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

            var fragmentView = inflater.Inflate(Resource.Layout.content_set, container, false);

            MaterialButton btn = fragmentView.FindViewById<MaterialButton>(Resource.Id.button1);
            btn.Click += initEvent;

            return fragmentView;
        }
        private void initEvent(object sender, EventArgs eventArgs)
        {
            AppCompatEditText nomET = View.FindViewById<AppCompatEditText>(Resource.Id.nomTF);
            AppCompatEditText telET = View.FindViewById<AppCompatEditText>(Resource.Id.telTF);
            AppCompatEditText paysET = View.FindViewById<AppCompatEditText>(Resource.Id.paysTF);

            var data = new
            {
                Tel = telET.Text,
                Nom = nomET.Text,
                Pays = paysET.Text
            };
            var json = JsonConvert.SerializeObject(data);
            Console.WriteLine("### serialized object output:: " + json);

            midSync(json);
        }

        private async void midSync(string val)
        {
            await SetAPI(val);
        }



        private async Task SetAPI(string valueToSend)
        {
            string uri = "";
            //only for testing with current emulator
            if (Build.Hardware.Contains("ranchu"))
                uri = "https://10.0.2.2:44317/api/Client";
            else
                uri = "https://192.168.1.2:45456/api/Client";

            //bypassing SSLHandshakeException
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var client = new HttpClient(clientHandler);
            var httpContent = new StringContent("");

            var result = await client.PostAsync(uri + "?fromMob=" + valueToSend, httpContent);
            //this how to retrieve the returned string from the POST call
            var fromPost = await result.Content.ReadAsStringAsync();
            Console.WriteLine(fromPost);
            if (result.IsSuccessStatusCode)
            {
                var tokenJson = await result.Content.ReadAsStringAsync();
            }
            else
                Console.WriteLine("************** FAILED: " + result.ReasonPhrase);
        }
    }
}
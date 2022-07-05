using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Button;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace StageEteMob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);



            MaterialButton btn = FindViewById<MaterialButton>(Resource.Id.button1);
            btn.Click += testClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        void testClick(object sender, EventArgs eventArgs)
        {
            AppCompatEditText nomET = FindViewById<AppCompatEditText>(Resource.Id.nomTF);
            AppCompatEditText telET = FindViewById<AppCompatEditText>(Resource.Id.telTF);
            AppCompatEditText paysET = FindViewById<AppCompatEditText>(Resource.Id.paysTF);


            Console.WriteLine("*******************   " + nomET.Text);
            var data =new
            {
                Tel = telET.Text,
                Nom = nomET.Text,
                Pays = paysET.Text
            };
            var json = JsonConvert.SerializeObject(data);
            meth(json);
        }
        async void meth(string val)
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
            //    //only for testing with current emulator 
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
            var result = await client.PostAsync(uri + "?fromMob="+valueToSend, httpContent);


            if (result.IsSuccessStatusCode)
            {
                var tokenJson = await result.Content.ReadAsStringAsync();
            }
            else
                Console.WriteLine("************** FAILED: " + result.ReasonPhrase);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

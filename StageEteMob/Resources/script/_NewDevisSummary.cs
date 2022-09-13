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
using StageEteMob.Resources.script;
using Google.Android.Material.Tabs;
using AndroidX.RecyclerView.Widget;
using Android.Graphics;

namespace StageEteMob
{
    /// <summary>
    /// TODO ADD LOADING INDICATOR
    /// </summary>
    public class _NewDevisSummary : AndroidX.Fragment.App.Fragment
    {
        TextView clientNameTV;
        TextView devisNameTV;
        public TextView articleCountTV, totalCostTV;
        ImageButton gobackIB;
        public Button confirmBtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._newDevis_summar, container, false);

            gobackIB = fragmentView.FindViewById<ImageButton>(Resource.Id.goback);
            clientNameTV = fragmentView.FindViewById<TextView>(Resource.Id.clientNameTV);
            articleCountTV = fragmentView.FindViewById<TextView>(Resource.Id.articleCountTV);
            devisNameTV = fragmentView.FindViewById<TextView>(Resource.Id.devisNameTV);
            confirmBtn = fragmentView.FindViewById<Button>(Resource.Id.confirmBtn);
            totalCostTV = fragmentView.FindViewById<TextView>(Resource.Id.totalCostTV);


            gobackIB.Click += GobackIB_Click;
            confirmBtn.Click += ConfirmBtn_Click;
            sumSetup(fragmentView);
            return fragmentView;
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {

            Devis devis = new Devis();
            devis.nom = GlobVars.devisName;
            devis.clientCode = GlobVars.client.Code;
            devis.IdUtilisateur = GlobVars.user.IdUtilisateur;
            foreach (Article article in GlobVars.selectListArticle)
            {
                Console.WriteLine("##### " + article.Name);
                devis.listArticle.Add(article);
            }

            var json = JsonConvert.SerializeObject(devis);
            Console.WriteLine("**************************************");
            Console.WriteLine(json);

            Console.WriteLine("**************************************");
            midSync(json);
        }

        void sumSetup(View frag)
        {
            devisNameTV.Text = GlobVars.devisName;
            var amount = GlobVars.selectListArticle.Count;
            if (amount > 1)
                articleCountTV.Text = amount.ToString() + "  Articles";
            else
                articleCountTV.Text = amount.ToString() + "  Article";

            clientNameTV.Text = "Client: " + GlobVars.client.Nom;

            recyclerViewSetup(frag, GlobVars.selectListArticle);

            //midSync(fragmentView);

        }
        private void GobackIB_Click(object sender, EventArgs e)
        {
            Console.WriteLine("************* " + GlobVars.selectListArticle.Count);
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisArticle()).Commit();
        }

        public void recyclerViewSetup(View fragmentView, List<Article> listOfArticles)
        {
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.finalArticleRV);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listOfArticles, this);
            rv.SetAdapter(adapter);
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
                uri = "https://10.0.2.2:44317/api/Devis/Put";
            else
                uri = "https://192.168.1.2:45456/api/Devis/Put";

            //bypassing SSLHandshakeException
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var client = new HttpClient(clientHandler);
            var httpContent = new StringContent("");

            var result = await client.PutAsync(uri + "?value=" + valueToSend, httpContent);
            //this how to retrieve the returned string from the POST call
            var fromPut = await result.Content.ReadAsStringAsync();
            Console.WriteLine(fromPut);
            if (result.IsSuccessStatusCode)
            {
                var tokenJson = await result.Content.ReadAsStringAsync();
            }
            else
                Console.WriteLine("************** FAILED: " + result.ReasonPhrase);
        }
    }
}
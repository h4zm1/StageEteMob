using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Icu.Text.Transliterator;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace StageEteMob.Resources.script
{
    /// <summary>
    /// setup caching
    /// </summary>
    public class RVAdapter : RecyclerView.Adapter
    {
        /// <summary>listarticle is used when there's articles, 3 times: in search article, in article selection and summary</summary>
        List<Article> listArticle;
        List<Devis> listDevis;
        List<Client> listClient;
        public bool isArticle = false;
        public bool isDevis = false;
        public bool isClient = false;
        public bool isSum = false;
        public bool isNormalArticle = false;
        public bool isNormalClient = false;
        public bool isNormalDevis = false;
        /// <summary>
        /// if there's at least 1 item selected, used for triggering the "Next" button
        /// </summary>
        bool selected = false;
        int qte = 0;
        _NewDevisClient ndc;
        _NewDevisArticle nda;
        _NewDevisSummary nds;
        _ClientFrag cf;
        _DevisFrag df;
        ///<summary> where the current selected item(s) are held
        ///Ex: if select 5th item in list, we save the number 5 to positionList
        ///And now we can easily retrieve the object with IndexOf (but somehow IndexOf don't want to work)
        /// </summary>
        List<int> positionList = new List<int>();
        struct OutDelVal
        {
            public int utilisateurId;
            public int code;// this will hold the id when working with devis
        }
        public override int ItemCount
        {
            get
            {
                if (isDevis)
                    return listDevis.Count;
                if (isArticle)
                    return listArticle.Count;
                if (isClient)
                    return listClient.Count;
                if (isSum)
                    return listArticle.Count;
                return 0;
            }
        }
        ///<summary> client constuctor</summary>
        public RVAdapter(List<Client> listClient, _NewDevisClient ndc)
        {
            this.isClient = true;
            this.listClient = listClient;
            this.ndc = ndc;
            GlobVars.listClient = listClient;
            //if "next" in client selection screen got clicked
            //this happens when we click the back arrow in article selection secreen
            if (GlobVars.devisClientDone && GlobVars.client != null)
            {
                //go through the list of clients we got from server
                for (int i = 0; i < listClient.Count; i++)
                {
                    //comparing the saved client's code with ones in listClient (cause INDEXOF didnt work somehow)
                    if (listClient[i].Code.Equals(GlobVars.client.Code))
                    {
                        positionList.Add(i);
                        selected = true;
                        if (isClient)
                            ndc.toggleNext();
                    }
                }
            }
        }

        ///<summary>article constuctor</summary>
        public RVAdapter(List<Article> listArticle, _NewDevisArticle nda)
        {
            this.isArticle = true;
            this.listArticle = listArticle;
            this.nda = nda;
            GlobVars.listArticle = listArticle;
            //maintain selected articles when going back from Summary
            if (GlobVars.devisArticleDone)
            {
                for (int i = 0; i < GlobVars.selectListArticle.Count; i++)
                {
                    for (int j = 0; j < listArticle.Count; j++)
                    {
                        ///TODO change .name to something unique like code or wtv
                        if (GlobVars.selectListArticle[i].Name.Equals(listArticle[j].Name))
                        {
                            listArticle[j].Quantity = GlobVars.selectListArticle[i].Quantity;
                            positionList.Add(j);
                            selected = true;
                        }
                    }
                }
                //this to make sure that toggle will only trigger once if there's more than one selection 
                if (selected && isArticle)
                    nda.toggleNext();
            }

        }
        ///<summary>sum constuctor</summary>
        public RVAdapter(List<Article> listArticle, _NewDevisSummary sum)
        {
            isSum = true;
            this.listArticle = listArticle;
            nds = sum;
            //setting up the total amount
            updateTotalAmount();
        }
        //devis search constructor
        public RVAdapter(List<Devis> listDevis, _DevisFrag df)
        {
            isDevis = true;
            isNormalDevis = true;
            this.listDevis = listDevis;
            this.df = df;
        }
        //client search constructor
        public RVAdapter(List<Client> listClient, _ClientFrag cf)
        {
            isNormalClient = true;
            isClient = true;
            this.listClient = listClient;
            this.cf = cf;
        }
        //article search constructor
        public RVAdapter(List<Article> listArticle)
        {
            isNormalArticle = true;
            isArticle = true;
            this.listArticle = listArticle;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RVHolder rvHolder = holder as RVHolder;

            if (isDevis)
            {
                var devis = listDevis[position];
                rvHolder.topTV.Text = "#" + devis.id;
                rvHolder.bottomTV.Text = "Created in: " + devis.Date;
                rvHolder.QteTV.Text = "Total: " + devis.Total.ToString();
            }
            if (isArticle)
            {
                var article = listArticle[position];
                rvHolder.topTV.Text = article.Name;
                rvHolder.bottomTV.Text = "Category: " + article.Categorie;
                rvHolder.QteTV.Text = "Price: " + article.Vente;
            }
            if (isClient)
            {
                var client = listClient[position];
                rvHolder.topTV.Text = client.Nom;
            }
            if (isSum)
            {
                var sum = listArticle[position];
                rvHolder.topTV.Text = sum.Name;
                rvHolder.bottomTV.Text = "Price: " + sum.Achat;
                rvHolder.np.Value = sum.Quantity;
            }
            rvHolder.position = position;
            //int position2 = rvHolder.LayoutPosition;
            //change the bg of the selected items
            if (positionList.Contains(position))
            {
                rvHolder.card.SetBackgroundResource(Resource.Drawable.roundEdgeSelect);
                rvHolder.topTV.SetTextColor(Color.Rgb(255, 255, 255));
                rvHolder.bottomTV.SetTextColor(Color.Rgb(255, 255, 255));

                if (isArticle)
                {
                    rvHolder.QteTV.SetTextColor(Android.Graphics.Color.Rgb(245, 245, 245));
                    Console.WriteLine(positionList.Count + " POSLIST ");

                    rvHolder.np.Enabled = true;
                }
            }
            //the the non selected items  
            else
            {
                rvHolder.card.SetBackgroundResource(Resource.Drawable.roundEdge);
                rvHolder.topTV.SetTextColor(Color.Rgb(34, 34, 34));
                rvHolder.bottomTV.SetTextColor(Color.Rgb(34, 34, 34));
                rvHolder.QteTV.SetTextColor(Color.Rgb(34, 34, 34));
                if (isArticle)
                {
                    rvHolder.np.Enabled = false;
                }
            }

        }

        void updateTotalAmount()
        {
            decimal tot = 0;
            foreach (Article article in GlobVars.selectListArticle)
            {
                for (int i = 0; i < article.Quantity; i++)
                {

                    tot = tot + article.Vente;
                }
            }
            nds.totalCostTV.Text = "Total: " + tot.ToString();
        }

        ///OnCreateViewHolder
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItem, parent, false);
            RVHolder viewHolder = new RVHolder(itemView, this);

            if (isSum)
            {
                viewHolder.bin.Click += (sender, e) =>
                {
                    //reseting the quantity of the deleted article to default (1)
                    listArticle[viewHolder.position].Quantity = 1;
                    //deleting the article, this only effect theh summary screen, listarticle will get refillled when we go back to selection
                    listArticle.RemoveAt(viewHolder.position);
                    NotifyDataSetChanged();

                    //updating the total cost
                    updateTotalAmount();

                    if (listArticle.Count > 0)
                        nds.confirmBtn.Enabled = true;
                    else
                        nds.confirmBtn.Enabled = false;

                    ///QOE
                    if (GlobVars.selectListArticle.Count > 1)
                        nds.articleCountTV.Text = GlobVars.selectListArticle.Count.ToString() + "  Articles";
                    else
                        nds.articleCountTV.Text = GlobVars.selectListArticle.Count.ToString() + "  Article";
                };

            }

            // deleting clients
            if (isClient)
            {
                viewHolder.bin.Click += (sender, e) =>
                {
                    OutDelVal outDelVal = new OutDelVal();
                    outDelVal.code = listClient[viewHolder.LayoutPosition].Code;
                    outDelVal.utilisateurId = GlobVars.user.IdUtilisateur;

                    var json = JsonConvert.SerializeObject(outDelVal);
                    midSync(json);
                    listClient.RemoveAt(viewHolder.position);
                    NotifyDataSetChanged();
                    cf.clientCountTV.Text = "Number of Clients: " + listClient.Count.ToString();

                };
            }
            // deleting devis
            if (isDevis)
            {
                viewHolder.bin.Click += (sender, e) =>
                {
                    OutDelVal outDelVal = new OutDelVal();
                    outDelVal.code = listDevis[viewHolder.LayoutPosition].id;
                    outDelVal.utilisateurId = GlobVars.user.IdUtilisateur;

                    var json = JsonConvert.SerializeObject(outDelVal);
                    midSync(json);
                    listDevis.RemoveAt(viewHolder.position);
                    NotifyDataSetChanged();

                };
            }
            async void midSync(string delJson)
            {
                if (isNormalClient)
                    await cf.DeletePI(delJson);
                else
                    await df.DeletePI(delJson);

            }
            //skip the click event on these lists
            if (isNormalArticle || isNormalClient || isNormalDevis)
                return viewHolder;
            viewHolder.ItemView.Click += (sender, e) =>
            {
                if (isSum)
                    return;
                //clicking same item => deselect it 
                if (positionList.Contains(viewHolder.position))
                {

                    positionList.Remove(viewHolder.position);
                    NotifyDataSetChanged();

                    if (selected)
                    {
                        if (isClient)
                        {
                            ndc.toggleNext();
                            selected = false;
                        }
                        if (isArticle)
                        {
                            updateGlobals();
                            if (positionList.Count == 0)
                            {
                                nda.toggleNext();
                                selected = false;
                            }
                        }
                    }
                }
                //clicking new item
                else
                {
                    //maintain one selection while working with client
                    if (isClient)
                        positionList.Clear();
                    //adding this item position to a list
                    positionList.Add(viewHolder.position);
                    NotifyDataSetChanged();


                    updateGlobals();
                    //toggle "next" when selection >0 for both lists
                    if (!selected)
                    {
                        if (isClient)
                            ndc.toggleNext();
                        if (isArticle)
                            nda.toggleNext();
                        selected = true;
                    }
                }

            };

            ///Handling quantity selection for articles
            if (isSum)
            {
                viewHolder.np.ValueChanged += (s, e) =>
            {
                NumberPicker np = s as NumberPicker;
                for (int i = 0; i < GlobVars.selectListArticle.Count; i++)
                {
                    if (i == viewHolder.LayoutPosition)
                        GlobVars.selectListArticle[i].Quantity = np.Value;
                }
                updateTotalAmount();
            };
            }
            return viewHolder;

        }
        void updateGlobals()
        {
            if (isClient)
            {
                GlobVars.client = GlobVars.listClient[positionList[0]];
            }
            if (isArticle)
            {
                //just clear it and refill 
                GlobVars.selectListArticle.Clear();
                foreach (int i in positionList)
                {
                    GlobVars.selectListArticle.Add(listArticle[i]);
                }
            }
        }
    }

    public class RVHolder : RecyclerView.ViewHolder
    {
        public TextView bottomTV;
        public TextView topTV;
        public TextView QteTV;
        public LinearLayout card;
        public LinearLayout rightLL;

        public LinearLayout leftLL;
        public NumberPicker np;
        public ImageView bin;
        public int position = -2;
        RVAdapter rVAdapter;
        public RVHolder(View itemview, RVAdapter rVAdapter) : base(itemview)
        {
            bottomTV = itemview.FindViewById<TextView>(Resource.Id.bottomTV);
            topTV = itemview.FindViewById<TextView>(Resource.Id.topTV);
            QteTV = itemview.FindViewById<TextView>(Resource.Id.QteTV);
            card = itemview.FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            bin = itemview.FindViewById<ImageView>(Resource.Id.bin);
            leftLL = itemview.FindViewById<LinearLayout>(Resource.Id.leftLL);
            rightLL = itemview.FindViewById<LinearLayout>(Resource.Id.rightLL);
            card.Elevation = 0f;
            this.rVAdapter = rVAdapter;
            np = itemview.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            np.MinValue = 1;
            np.MaxValue = 100;
            np.WrapSelectorWheel = true;
            np.Value = 0;
            np.SelectionDividerHeight = 0;


            ///Display conditions

            //isNormalClient for the clients in the search tab
            //isClient for the clients in the devis creation tab


            if (rVAdapter.isClient && !rVAdapter.isNormalClient)
            {
                rightLL.Visibility = ViewStates.Gone;
                topTV.TextSize = 15f;
                bottomTV.Visibility = ViewStates.Gone;
                leftLL.LayoutParameters.Height = 90;
            }
            if (rVAdapter.isNormalClient)
            {
                topTV.TextSize = 15f;
                np.Visibility = ViewStates.Gone;
                QteTV.Visibility = ViewStates.Gone;
                bottomTV.Visibility = ViewStates.Gone;
                //bin.LayoutParameters.Width = 40;
                //bin.LayoutParameters.Height = 70;
                //Console.WriteLine("HEIGHT " + leftLL.LayoutParameters.Height);
                leftLL.LayoutParameters.Height = 90;
                bin.LayoutParameters.Width = 60;
                bin.LayoutParameters.Height = 70;
            }
            if (rVAdapter.isArticle)
            {
                bin.Visibility = ViewStates.Gone;
                //bottomTV.Visibility = ViewStates.Gone;
                np.Visibility = ViewStates.Gone;
                leftLL.LayoutParameters.Height = 150;
                rightLL.LayoutParameters.Height = 150;

                topTV.TextSize = 13f;
                //bottomTV.TextSize = 10f;

            }
            if (rVAdapter.isNormalArticle)
            {
                rightLL.Visibility = ViewStates.Gone;
            }
            if (rVAdapter.isNormalDevis)
            {
                //rightLL.Visibility = ViewStates.Gone;

                np.Visibility = ViewStates.Gone;
                bin.LayoutParameters.Width = 60;
                bin.LayoutParameters.Height = 70;
                leftLL.LayoutParameters.Height = 90;
                topTV.TextSize = 15f;
                topTV.SetTypeface(topTV.Typeface, TypefaceStyle.Bold);
                bottomTV.TextSize = 13f;

                leftLL.LayoutParameters.Height = 130;

            }
            if (rVAdapter.isSum)
            {
                leftLL.LayoutParameters.Height = 210;
                rightLL.LayoutParameters.Height = 210;
                bin.LayoutParameters.Width = 60;
                bin.LayoutParameters.Height = 70;
                topTV.TextSize = 14f;
                //bottomTV.TextSize = 14f;
            }
        }

    }
}

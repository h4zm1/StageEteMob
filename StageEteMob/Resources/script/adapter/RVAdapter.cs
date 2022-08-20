using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
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
        List<Article> listArticle;
        List<Devis> listDevis;
        List<Client> listClient;
        public bool isArticle = false;
        public bool isDevis = false;
        public bool isClient = false;
        bool selected = false;
        _NewDevisClient ndc;
        _NewDevisArticle nda;
        List<int> positionList = new List<int>();
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
                return 0;
            }
        }
        public RVAdapter(List<Client> listClient, _NewDevisClient ndc)
        {
            this.isClient = true;
            this.listClient = listClient;
            this.ndc = ndc;
            GlobVars.listClient = listClient;
            //if "next" in client selection got clicked
            if (GlobVars.devisClientDone)
            {
                for (int i = 0; i < listClient.Count; i++)
                {
                    //comparing objects AND INDEXOF didnt work
                    if (listClient[i].Nom.Equals(GlobVars.client.Nom))
                    {
                        positionList.Add(i);
                        selected = true;
                        if (isClient)
                            ndc.toggleNext();
                    }
                }
            }
        }
        public RVAdapter(List<Article> listArticle)
        {
            this.isArticle = true;
            this.listArticle = listArticle;
        }
        public RVAdapter(List<Article> listArticle, _NewDevisArticle nda)
        {
            this.isArticle = true;
            this.listArticle = listArticle;
            this.nda = nda;
            GlobVars.listArticle = listArticle;
            if (GlobVars.devisArticleDone)
            {
                for (int i = 0; i < GlobVars.selectListArticle.Count; i++)
                {
                    for (int j = 0; j < listArticle.Count; j++)
                    {
                        ///TODO change .name to something unique like code or wtv
                        if (GlobVars.selectListArticle[i].Name.Equals(listArticle[j].Name))
                        {
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
        public RVAdapter(List<Devis> listDevis)
        {
            this.isDevis = true;
            this.listDevis = listDevis;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RVHolder rvHolder = holder as RVHolder;
            if (isDevis)
            {
                var devis = listDevis[position];
                rvHolder.itemTV.Text = devis.code;
            }
            if (isArticle)
            {
                var article = listArticle[position];
                rvHolder.itemTV.Text = article.Name;
            }
            if (isClient)
            {
                var client = listClient[position];
                rvHolder.itemTV.Text = client.Nom;
            }

            rvHolder.position = position;


            //change the card bg of all item in the list
            if (positionList.Contains(position))
            {
                rvHolder.card.SetBackgroundColor(Android.Graphics.Color.Rgb(230, 230, 230));
            }
            else
            {
                rvHolder.card.SetBackgroundColor(Color.White);
            }

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItem, parent, false);
            RVHolder viewHolder = new RVHolder(itemView, this);
            viewHolder.ItemView.Click += (sender, e) =>
            {
                //clicking same item => deselect it 
                if (positionList.Contains(viewHolder.position))
                {
                    positionList.Remove(viewHolder.position);
                    NotifyDataSetChanged();
                    if (selected)
                    {
                        if (isClient)
                            ndc.toggleNext();
                        if (isArticle)
                            nda.toggleNext();
                        selected = false;
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
                //no need to update globallist in delete, just clear it
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
        public TextView itemTV
        {
            get;
            set;
        }
        public CardView card
        {
            get;
            set;
        }
        public int position = -2;
        RVAdapter rVAdapter;
        public RVHolder(View itemview, RVAdapter rVAdapter) : base(itemview)
        {

            itemTV = itemview.FindViewById<TextView>(Resource.Id.itemTextView);
            card = itemview.FindViewById<CardView>(Resource.Id.cardView1);
            this.rVAdapter = rVAdapter;
            NumberPicker np = itemview.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            np.MinValue = 00;
            np.MaxValue = 100;
            np.WrapSelectorWheel = true;
            np.Value = 5;
            np.SelectionDividerHeight = 0;
            if (rVAdapter.isClient)
                np.Visibility = ViewStates.Gone;
        }

    }
}

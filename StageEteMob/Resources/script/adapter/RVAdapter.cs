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
    public class RVAdapter : RecyclerView.Adapter
    {
        List<Article> listArticle;
        List<Devis> listDevis;
        List<Client> listClient;
        Boolean assignToArticle = false;
        Boolean assignToDevis = false;
        Boolean assignToClient = false;
        public int selectedPosition = -1;
        Boolean selected = false;
        int position = -2;
        _NewDevisClient ndc;
        public override int ItemCount
        {
            get
            {
                if (assignToDevis)
                    return listDevis.Count;
                if (assignToArticle)
                    return listArticle.Count;
                if (assignToClient)
                    return listClient.Count;
                return 0;
            }
        }
        public RVAdapter(List<Client> listClient, _NewDevisClient ndc)
        {
            this.assignToClient = true;
            this.listClient = listClient;
            this.ndc = ndc;
        }
        public RVAdapter(List<Article> listArticle)
        {
            this.assignToArticle = true;
            this.listArticle = listArticle;
        }
        public RVAdapter(List<Devis> listDevis)
        {
            this.assignToDevis = true;
            this.listDevis = listDevis;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RVHolder rvHolder = holder as RVHolder;
            if (assignToDevis)
            {
                var devis = listDevis[position];
                rvHolder.itemTV.Text = devis.code;
            }
            if (assignToArticle)
            {
                var article = listArticle[position];
                rvHolder.itemTV.Text = article.Name;
            }
            if (assignToClient)
            {
                var client = listClient[position];
                rvHolder.itemTV.Text = client.Nom;
            }

            rvHolder.position = position;

            if (selectedPosition == position)
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
                if (selectedPosition == viewHolder.position)
                {
                    selectedPosition = -1;
                    NotifyDataSetChanged();
                    if (selected)
                    {
                        if (assignToClient)
                            ndc.toggleNext();
                        selected = false;
                    }
                }
                //clicking new item
                else
                {
                    selectedPosition = viewHolder.position;
                    NotifyDataSetChanged();
                    if (!selected)
                    {
                        if (assignToClient)
                            ndc.toggleNext();
                        selected = true;
                    }
                }

            };

            return viewHolder;

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
            //np.Visibility = ViewStates.Gone;
        }

    }
}

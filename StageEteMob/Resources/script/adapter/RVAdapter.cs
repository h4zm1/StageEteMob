using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageEteMob.Resources.script
{
    public class RVAdapter : RecyclerView.Adapter
    {
        List<Article> listArticle;
        List<Devis> listDevis;

        public override int ItemCount
        {
            get
            {
                if (listArticle == null)
                    return listDevis.Count;
                else
                    return listArticle.Count;
            }
        }
        public RVAdapter(List<Article> listArticle)
        {
            this.listArticle = listArticle;
        }
        public RVAdapter(List<Devis> listDevis)
        {
            this.listDevis = listDevis;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RVHolder rvHolder = holder as RVHolder;
            if (listArticle == null)
            {
                var devis = listDevis[position];
                rvHolder.itemTV.Text = devis.code;
            }
            else
            {
                var article = listArticle[position];
                rvHolder.itemTV.Text = article.Name;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItem, parent, false);
            RVHolder viewHolder = new RVHolder(itemView);
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
        public RVHolder(View itemview) : base(itemview)
        {
            itemTV = itemview.FindViewById<TextView>(Resource.Id.itemTextView);
            NumberPicker np = itemview.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            np.MinValue = 00;
            np.MaxValue = 100;
            np.WrapSelectorWheel = true;
            np.Value = 5;
            np.SelectionDividerHeight = 0;
        }
    }
}

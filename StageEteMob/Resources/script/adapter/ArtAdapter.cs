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
    public  class ArtAdapter : RecyclerView.Adapter
    {
        List<Article> listArticle;
        public override int ItemCount => listArticle.Count;
        public ArtAdapter (List<Article> listArticle)
        {
            this.listArticle = listArticle;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ArtViewHolder artVH = holder as ArtViewHolder;
            var article = listArticle[position];
            artVH.itemTV.Text = article.Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItem, parent, false);
            ArtViewHolder viewHolder = new ArtViewHolder(itemView);
            return viewHolder;

        }
    }
    public class ArtViewHolder : RecyclerView.ViewHolder
    {
        public TextView itemTV
        {
            get;
            set;
        }
        public ArtViewHolder(View itemview) : base(itemview)
        {
            itemTV = itemview.FindViewById<TextView>(Resource.Id.itemTextView);
        }
    }
}

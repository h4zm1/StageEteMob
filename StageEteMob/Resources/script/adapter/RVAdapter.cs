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
        ///<summary> where the current selected item(s) are held
        ///Ex: if select 5th item in list, we save the number 5 to positionList
        ///And now we can easily retrieve the object with IndexOf (but somehow IndexOf don't want to work)
        /// </summary>
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
            //if "next" in client selection got clicked
            if (GlobVars.devisClientDone && GlobVars.client != null)
            {
                //go through the list of client we got from server
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

        ///<summary>article constuctor</summary>
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
        ///<summary>sum constuctor</summary>
        public RVAdapter(List<Article> listArticle, _NewDevisSummary sum)
        {
            isSum = true;
            this.listArticle = listArticle;
            nds = sum;
        }
        public RVAdapter(List<Devis> listDevis)
        {
            isDevis = true;
            isNormalDevis = true;
            this.listDevis = listDevis;
        }
        public RVAdapter(List<Client> listClient)
        {
            isNormalClient = true;
            isClient = true;
            this.listClient = listClient;
        }
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
                rvHolder.topTV.Text = devis.code;
            }
            if (isArticle)
            {
                var article = listArticle[position];
                rvHolder.topTV.Text = article.Name;
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
                rvHolder.np.Value = sum.Quantity;
            }
            rvHolder.position = position;
            //int position2 = rvHolder.LayoutPosition;
            //change the bg of the selected items
            if (positionList.Contains(position))
            {
                rvHolder.card.SetBackgroundResource(Resource.Drawable.roundEdgeSelect);
                if (isArticle)
                {
                    rvHolder.QteTV.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
                    Console.WriteLine(positionList.Count + " POSLIST ");


                    //if (rvHolder.np.Enabled)
                    //{

                    rvHolder.np.Enabled = true;

                    rvHolder.np.Value = GlobVars.selectListArticle[positionList.IndexOf(position)].Quantity;
                    //Console.WriteLine(position + " 11111  " + position);
                    //}
                }
            }
            //the the non selected items
            else
            {
                rvHolder.card.SetBackgroundResource(Resource.Drawable.roundEdge);
                if (isArticle)
                {
                    //if (!modifiedQteList.Contains(position))
                    //{

                    //    rvHolder.np.Enabled = false;
                    //    Console.WriteLine(position + " 22222  " + position);
                    //}
                    rvHolder.np.Enabled = false;
                    rvHolder.QteTV.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                    //Console.WriteLine(position + " 22222  " + position);
                }
            }


        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listItem, parent, false);
            RVHolder viewHolder = new RVHolder(itemView, this);

            if (isSum)
            {
                viewHolder.bin.Click += (sender, e) =>
                {
                    listArticle.RemoveAt(viewHolder.position);
                    NotifyDataSetChanged();
                };
            }
            //skip the click event on these lists
            if (isNormalArticle || isNormalClient || isNormalDevis || isSum)
                return viewHolder;
            viewHolder.ItemView.Click += (sender, e) =>
            {
                //clicking same item => deselect it 
                if (positionList.Contains(viewHolder.position))
                {

                    ///need to rest the article quantity to 0 when deselected before it's position get removed from positionList
                    if (isArticle)
                    {
                        //NumberPicker np = s as NumberPicker;
                        for (int i = 0; i < GlobVars.selectListArticle.Count; i++)
                        {
                            //positionList contains an index of item location (int)
                            //viewHolder.layoutPosition is the position on sight (int)
                            if (positionList[i] == viewHolder.LayoutPosition)
                            {
                                GlobVars.selectListArticle[i].Quantity = 0;
                            }
                        }
                    }

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
            if (isArticle)
                viewHolder.np.ValueChanged += (s, e) =>
            {
                NumberPicker np = s as NumberPicker;
                for (int i = 0; i < GlobVars.selectListArticle.Count; i++)
                {
                    //positionList contains an index of item location (int)
                    //viewHolder.layoutPosition is the position on sight (int)
                    if (positionList[i] == viewHolder.LayoutPosition)
                    {
                        GlobVars.selectListArticle[i].Quantity = np.Value;
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
            np.MinValue = 00;
            np.MaxValue = 100;
            np.WrapSelectorWheel = true;
            np.Value = 0;
            np.SelectionDividerHeight = 0;


            //Display conditions
            if (rVAdapter.isClient && !rVAdapter.isNormalClient)
            {
                rightLL.Visibility = ViewStates.Gone;
                bottomTV.Visibility = ViewStates.Gone;
            }
            if (rVAdapter.isClient && rVAdapter.isNormalClient)
            {
                np.Visibility = ViewStates.Gone;
                QteTV.Visibility = ViewStates.Gone;
                bottomTV.Visibility = ViewStates.Gone;
                bin.LayoutParameters.Width = 40;
                bin.LayoutParameters.Height = 70;
            }
            if (rVAdapter.isArticle)
            {
                bin.Visibility = ViewStates.Gone;
                bottomTV.Visibility = ViewStates.Gone;

            }
            if (rVAdapter.isNormalArticle)
            {
                rightLL.Visibility = ViewStates.Gone;
            }
            if (rVAdapter.isNormalDevis)
            {
                rightLL.Visibility = ViewStates.Gone;
                bottomTV.Visibility = ViewStates.Gone;
            }
            if (rVAdapter.isSum)
            {
                bin.LayoutParameters.Width = 60;
                bin.LayoutParameters.Height = 70;
            }
        }

    }
}

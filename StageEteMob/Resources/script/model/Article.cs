using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageEteMob.Resources.script
{
    public class Article
    {
        public string Name { get; set; }
        public decimal Achat { get; set; }
        public string Categorie { get; set; }
        public decimal Vente { get; set; }
        public string Code { get; set; }
        public string Tva { get; set; }
        public string Designation { get; set; }
        public int Quantity { get; set; }
    }
}
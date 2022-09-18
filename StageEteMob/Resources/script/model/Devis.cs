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
    public class Devis
    {
        public int id { get; set; }
        public int clientCode { get; set; }
        public string nom { get; set; }
        public string userCode { get; set; }
        public string Date { get; set; }
        public string code { get; set; }
        public List<Article> listArticle = new List<Article>();
        public int IdUtilisateur { get; set; }
        public decimal Total { get; set; }
    }
}
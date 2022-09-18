using System.Collections.Generic;

namespace StageEteMob.Resources.script
{
    internal static class GlobVars
    {
        public static bool devisNameDone = false;
        public static bool devisClientDone = false;
        public static bool devisArticleDone = false;
        public static string devisName { get; set; }
        public static Client client { get; set; }
        public static User user { get; set; }

        public static List<Client> listClient = new List<Client>();
        public static List<Article> listArticle = new List<Article>();
        public static List<Article> selectListArticle = new List<Article>();

    }
}
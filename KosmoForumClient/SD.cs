using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient
{
    public class SD
    {
        public static string ApiBaseUrl = "https://localhost:44374/";
        public static string Categories = ApiBaseUrl + "api/v1/categories/";
        public static string ForumPosts = ApiBaseUrl + "api/v1/forumposts/";
        public static string Opinions = ApiBaseUrl + "api/v1/opinions/";
    }
}

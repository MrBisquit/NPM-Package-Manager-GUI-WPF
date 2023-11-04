using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Types
{
    public class SearchResult
    {
        [JsonProperty("objects")]
        public SearchResultObject[] Objects;

        [JsonProperty("total")]
        public int Total;

        [JsonProperty("time")]
        public string Time;
    }

    public class SearchResultObject
    {
        [JsonProperty("package")]
        public SearchResultObjectPackage Package;

        [JsonProperty("flags")]
        public SearchResultObjectFlags Flags;

        [JsonProperty("score")]
        public SearchResultObjectScore Score;

        [JsonProperty("searchScore")]
        public double SearchScore;
    }

    public class SearchResultObjectPackage
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("scope")]
        public string Scope;

        [JsonProperty("version")]
        public string Version;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("keywords")]
        public string[] Keywords;

        [JsonProperty("date")]
        public string Date;

        [JsonProperty("links")]
        public SearchResultObjectPackageLinks Links;

        [JsonProperty("author")]
        public Author Author;

        [JsonProperty("publisher")]
        public Publisher Publisher;

        [JsonProperty("maintainers")]
        public Maintainer[] Maintainers;
    }

    public class SearchResultObjectPackageLinks
    {
        [JsonProperty("npm")]
        public string NPM;

        [JsonProperty("homepage")]
        public string Homepage;

        [JsonProperty("repository")]
        public string Repository;

        [JsonProperty("bugs")]
        public string Bugs;
    }

    public class Publisher
    {
        [JsonProperty("username")]
        public string Username;

        [JsonProperty("email")]
        public string Email;
    }

    public class SearchResultObjectFlags
    {
        [JsonProperty("insecure")]
        public int Insecure;
    }

    public class SearchResultObjectScore
    {
        [JsonProperty("final")]
        public double Final;

        [JsonProperty("detail")]
        public SearchResultObjectScoreDetail Detail;
    }

    public class SearchResultObjectScoreDetail
    {
        [JsonProperty("quality")]
        public double Quality;

        [JsonProperty("popularity")]
        public double Popularity;

        [JsonProperty("maintenance")]
        public double Maintenance;
    }
}

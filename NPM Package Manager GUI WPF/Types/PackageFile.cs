using Newtonsoft.Json;

namespace NPM_Package_Manager_GUI_WPF.Types
{
    // ChatGPT
    public class PackageJson
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("scripts")]
        public Dictionary<string, string> Scripts { get; set; }

        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies { get; set; }

        [JsonProperty("devDependencies")]
        public Dictionary<string, string> DevDependencies { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }
    }

    public class Repository
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

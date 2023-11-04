using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Types
{
    public class RegistryVersions
    {
        [JsonProperty("_id")]
        public string Id;

        [JsonProperty("_rev")]
        public string Rev;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("dist-tags")]
        public Dictionary<string, string> DistTags;

        [JsonProperty("versions")]
        public Dictionary<string, RegistryVersion> Versions;
    }

    // This was ChatGPT since it's only a structure
    public class RegistryVersion
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("contributors")]
        public List<Contributor> Contributors { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("directories")]
        public Directories Directories { get; set; }

        [JsonProperty("scripts")]
        public Dictionary<string, string> Scripts { get; set; }

        [JsonProperty("engines")]
        public Engines Engines { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_nodeSupported")]
        public bool NodeSupported { get; set; }

        [JsonProperty("_npmVersion")]
        public string NpmVersion { get; set; }

        [JsonProperty("_nodeVersion")]
        public string NodeVersion { get; set; }

        [JsonProperty("dist")]
        public Dist Dist { get; set; }

        [JsonProperty("deprecated")]
        public string Deprecated { get; set; }
    }

    public class Directories
    {
        [JsonProperty("lib")]
        public string Lib { get; set; }
    }

    public class Engines
    {
        [JsonProperty("node")]
        public string Node { get; set; }
    }
}

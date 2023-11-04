using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Types
{
    public class RegistryPackageVersion
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("version")]
        public string Version;

        [JsonProperty("author")]
        public Author Author;

        [JsonProperty("contributors")]
        public Contributor[] Contributors;

        [JsonProperty("license")]
        public string license;

        [JsonProperty("repository")]
        public Repository Repository;

        [JsonProperty("homepage")]
        public string HomePage;

        [JsonProperty("keywords")]
        public string[] Keywords;

        [JsonProperty("dependencies")]
        public Dictionary<string, string> Dependencies;

        [JsonProperty("devDependencies")]
        public Dictionary<string, string> DevDependencies;

        [JsonProperty("engines")]
        public Dictionary<string, string> Engines;

        [JsonProperty("scripts")]
        public Dictionary<string, string> Scripts;

        [JsonProperty("gitHead")]
        public string GitHead;

        [JsonProperty("_id")]
        public string Id;

        [JsonProperty("_nodeVersion")]
        public string NodeVersion;

        [JsonProperty("_npmVersion")]
        public string NPMVersion;

        [JsonProperty("dist")]
        public Dist Dist;

        [JsonProperty("_npmUser")]
        public NPMUser NPMUser;

        // "directories" is here but the object on https://registry.npmjs.org/express/latest (The package I'm using as a template) was empty.

        [JsonProperty("maintainers")]
        public Maintainer[] Maintainers;

        // There's also "_npmOperationalInternal" and "_hasShrinkwrap" here but I don't think they're needed.
    }

    public class Author 
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("email")]
        public string Email;
    }

    public class Contributor
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("email")]
        public string Email;
    }

    public class Bugs
    {
        [JsonProperty("url")]
        public string Url;
    }

    public class Dist
    {
        [JsonProperty("integrity")]
        public string Integrity;

        [JsonProperty("shasum")]
        public string shasum;

        [JsonProperty("tarball")]
        public string Tarball;

        [JsonProperty("fileCount")]
        public int FileCount;

        [JsonProperty("unpackedSize")]
        public int UnpackedSize;

        [JsonProperty("signatures")]
        public Signiture[] Signitures;

        [JsonProperty("npm-signiture")]
        public string NPMSignuture;
    }

    public class Signiture
    {
        [JsonProperty("keyid")]
        public string KeyId;

        [JsonProperty("sig")]
        public string Sig;
    }

    public class NPMUser
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("email")]
        public string Email;
    }

    public class Maintainer
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("email")]
        public string Email;
    }
}

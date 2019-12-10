 using Newtonsoft.Json;
 using System.Collections.Generic;

[System.Serializable]
public class CustomAssetBundleManifest
{
    [JsonProperty("AssetBundles")]
    public List<string> AssetBundles = new List<string>();
    //string = assetBundleName, List<string> = assetBundleDependencies
    [JsonProperty("Dependencies")]
    public Dictionary<string, List<string>> Dependencies = new Dictionary<string, List<string>>();
    ///string = assetBundleName
    [JsonProperty("AssetBundleHashs")]
    public Dictionary<string, string> AssetBundleHashs = new Dictionary<string, string>(); 
} 
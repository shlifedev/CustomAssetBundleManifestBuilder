using Newtonsoft.Json;
using System.Collections.Generic;
public static class CustomAssetBundleManifestReader
{
    public static void Read(string content, out CustomAssetBundleManifest oput)
    {
        CustomAssetBundleManifest cabm = null;
        cabm = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomAssetBundleManifest>(content);
        oput = cabm;
    }
}

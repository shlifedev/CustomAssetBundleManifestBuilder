using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;
public static class CustomAssetBundleManifestBuilder
{ 
    public static void Generate(string[] manifestBundles, ref CustomAssetBundleManifest CManifest, string writePath = null)
    {
        foreach (var manifestPath in manifestBundles)
        {
            var m_bundle = AssetBundle.LoadFromFile(manifestPath);
            var loadedManifest = m_bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (loadedManifest != null)
            {
                foreach (var data in loadedManifest.GetAllAssetBundles())
                {
                    //에셋번들&해쉬
                    CManifest.AssetBundles.Add(data);
                    CManifest.AssetBundleHashs.Add(data, loadedManifest.GetAssetBundleHash(data).ToString());

                    //종속성 추가 
                    var dependencies = new List<string>();
                    var loadedDependencies = loadedManifest.GetAllDependencies(data);
                    if (loadedDependencies.Length != 0)
                    {
                        dependencies.AddRange(loadedDependencies);
                        CManifest.Dependencies.Add(data, dependencies);
                    }
                }
            }
            else
            {
                Debug.LogError("Load Failed AssetBundle");
            }
         
            m_bundle.Unload(true);
        }

        if (writePath != null)
        {
            Write(CManifest, writePath);
        }
    }

    public static void Write(CustomAssetBundleManifest data, string path)
    {
        var serialize = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        System.IO.File.WriteAllText(path, serialize);
        AssetDatabase.Refresh();
        System.IO.FileInfo fi = new System.IO.FileInfo(path);

        string fileName = fi.Name;
//        if (System.IO.File.Exists(path))
//            AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant(fileName.Split('.')[0], "");
    }
}

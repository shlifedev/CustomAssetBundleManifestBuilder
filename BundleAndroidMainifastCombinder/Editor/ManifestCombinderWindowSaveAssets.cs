using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ManifestCombinderWindowSaveAssets : ScriptableObject
{ 
    public List<string> targetBundlePath = new List<string>();
    public string savePath;
}
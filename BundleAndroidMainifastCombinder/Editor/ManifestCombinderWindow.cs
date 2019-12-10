using UnityEngine;
using UnityEditor;

public class ManifestCombinderWindow : EditorWindow
{

    public ManifestCombinderWindowSaveAssets loadedAsset;
    [MenuItem("HamsterLib/MenifestCombinder/Open")]
    private static void ShowWindow()
    {
        var window = GetWindow<ManifestCombinderWindow>();
        window.titleContent = new GUIContent("ManifestCombinderWindow");
        window.Show();
    }
    public void OnEnable()
    {
        var existAsset = System.IO.File.Exists("Assets/BundleAndroidMainifastCombinder/Editor/ManifestCombinderWindowSaveAssets.asset");
        if (existAsset)
        {
            loadedAsset = AssetDatabase.LoadAssetAtPath("Assets/BundleAndroidMainifastCombinder/Editor/ManifestCombinderWindowSaveAssets.asset", typeof(ManifestCombinderWindowSaveAssets)) as ManifestCombinderWindowSaveAssets;
        }
        else
        {
            loadedAsset = new ManifestCombinderWindowSaveAssets();
            loadedAsset.targetBundlePath = new System.Collections.Generic.List<string>();
            Save();
        }
    }

    public void Save()
    {
        AssetDatabase.CreateAsset(loadedAsset, "Assets/BundleAndroidMainifastCombinder/Editor/ManifestCombinderWindowSaveAssets.asset");
    }

    public void Verify()
    {
        loadedAsset.targetBundlePath.ForEach(x =>
        {
            var exist = System.IO.File.Exists(x);

            if (!exist)
            {
                EditorUtility.DisplayDialog("Verify Faield!!", "Verify Faield!!", "ok");
                Debug.LogError("file is not exist =>" + x);
                return;
            }

        }); 
        EditorUtility.DisplayDialog("Verify Sucesfully!!", "Verify Sucesfully!!", "ok");
    }
    private void OnGUI()
    { 
        GUILayout.Label("빌드할 에셋번들 메니페스트 경로(s)"); 
        for (int i = 0; i < loadedAsset.targetBundlePath.Count; i++)
        {
            GUILayout.BeginHorizontal();
            loadedAsset.targetBundlePath[i] = GUILayout.TextArea(loadedAsset.targetBundlePath[i]);
            int current = i;
            if (GUILayout.Button("Remove"))
            {
                loadedAsset.targetBundlePath.RemoveAt(current);
            }
            GUILayout.EndHorizontal();
        } 
        GUILayout.Label("경로 및 저장 파일 이름");
        loadedAsset.savePath = GUILayout.TextArea(loadedAsset.savePath); 
        if (GUILayout.Button("Add Path"))
        {
            loadedAsset.targetBundlePath.Add("");
        }
        if (GUILayout.Button("Verify Manifest All Exist"))
        {
            Verify();
        }
        if (GUILayout.Button("Generate Custom Manifest"))
        {
            CustomAssetBundleManifest menifest = new CustomAssetBundleManifest();
            CustomAssetBundleManifestBuilder.Generate(loadedAsset.targetBundlePath.ToArray(), ref menifest, loadedAsset.savePath);
            AssetDatabase.Refresh();
        } 
    }
}
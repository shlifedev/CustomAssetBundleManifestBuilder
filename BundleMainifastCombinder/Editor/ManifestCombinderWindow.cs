using UnityEngine;
using UnityEditor;

public class ManifestCombinderWindow : EditorWindow
{

    public static string assetPath = "Assets/ManifestCombinder/Editor/ManifestCombinderWindowSaveAssets.asset";
    public ManifestCombinderWindowSaveAssets loadedAsset;
    [MenuItem("HamsterLibs/MenifestCombinder/Open")]
    private static void ShowWindow()
    {
        var window = GetWindow<ManifestCombinderWindow>();
        window.titleContent = new GUIContent("ManifestCombinderWindow");
        window.Show(); 
    }

    public void OnEnable()
    {
        var existAsset = System.IO.File.Exists(assetPath);
        if (existAsset)
        {
            loadedAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ManifestCombinderWindowSaveAssets)) as ManifestCombinderWindowSaveAssets;
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
        AssetDatabase.CreateAsset(loadedAsset, assetPath);
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

    void GenerateCustomManifest()
    {
        CustomAssetBundleManifest menifest = new CustomAssetBundleManifest();
        CustomAssetBundleManifestBuilder.Generate(loadedAsset.targetBundlePath.ToArray(), ref menifest, loadedAsset.savePath);
        UnityEditor.EditorUtility.SetDirty(loadedAsset);
        AssetDatabase.Refresh();
    }

    void OpenSavePath()
    {       var path = loadedAsset.savePath;
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo(fi.Directory.FullName);
            proc.Start();

    }
    public bool ftpFoldout = false;
    private void DrawFTPLogic()
    {
        // GUILayout.Space(25);
        // GUILayout.
        // ftpFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(ftpFoldout, "");
        // GUILayout.Button("");
        // EditorGUILayout.EndFoldoutHeaderGroup();
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
                UnityEditor.EditorUtility.SetDirty(loadedAsset);
            }
            if (GUILayout.Button("OpenFolder "))
            {
                var path = loadedAsset.targetBundlePath[current];
                System.IO.FileInfo fi = new System.IO.FileInfo(loadedAsset.targetBundlePath[current]);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo(fi.Directory.FullName);
                proc.Start(); 
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("경로 및 저장 파일 이름");
        loadedAsset.savePath = GUILayout.TextArea(loadedAsset.savePath);
        if (GUILayout.Button("Open Folder"))
        {
           OpenSavePath();
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Path"))
        {
            loadedAsset.targetBundlePath.Add("");
            UnityEditor.EditorUtility.SetDirty(loadedAsset);
        }

        if (GUILayout.Button("Verify Manifest All Exist"))
        {
            UnityEditor.EditorUtility.SetDirty(loadedAsset);
            Verify();
        }

        if (GUILayout.Button("Generate Custom Manifest"))
        {
            GenerateCustomManifest();
            OpenSavePath(); 
        } 
DrawFTPLogic();
    }
}
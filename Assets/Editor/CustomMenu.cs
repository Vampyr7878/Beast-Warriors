using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomMenu : MonoBehaviour
{
    [MenuItem("Beast Warriors/Cleanup SavePrefabs")]
    public static  void CleanupSavePrefabs()
    {
        DirectoryInfo dir = new DirectoryInfo(@"Assets\Resources\Parts\Prefabs");
        foreach (FileInfo file in dir.GetFiles("*.prefab"))
        {
            string path = file.FullName.Replace("\\", "/");
            path = "Assets" +  path.Replace(Application.dataPath, "");
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            SavePrefab save = prefab.GetComponent<SavePrefab>();
            DestroyImmediate(save, true);
        }
    }
}

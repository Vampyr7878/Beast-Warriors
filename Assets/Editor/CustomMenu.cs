using System;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CustomMenu : MonoBehaviour
{
    [MenuItem("Beast Warriors/Cleanup SavePrefabs")]
    public static  void CleanupSavePrefabs()
    {
        DirectoryInfo dir = new(@"Assets\Resources\Parts");
        foreach (FileInfo file in dir.GetFiles("*.prefab"))
        {
            string path = file.FullName.Replace("\\", "/");
            path = $"Assets{path.Replace(Application.dataPath, "")}";
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            SavePrefab save = prefab.GetComponent<SavePrefab>();
            DestroyImmediate(save, true);
        }
    }

    [MenuItem("Beast Warriors/Update Animators")]
    public static void UpdateAnimators()
    {
        DirectoryInfo dir = new(@"Assets\Animations");
        AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(@"Assets\Animations\Animator.controller", typeof(AnimatorController));
        foreach (FileInfo file in dir.GetFiles("*.controller", SearchOption.AllDirectories))
        {
            if (!file.Name.Contains("Animator"))
            {
                string path = file.FullName.Replace("\\", "/");
                path = "Assets" + path.Replace(Application.dataPath, "");
                AnimatorController animator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                animator.RemoveLayer(0);
                //animator.AddLayer(controller.layers[0]);
                //AnimatorControllerLayer[] layers = animator.layers;
                //Array.Reverse(layers);
                //animator.layers = layers;
            }
        }
    }
}

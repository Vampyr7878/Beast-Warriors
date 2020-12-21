using UnityEditor;
using UnityEngine;

public class SavePrefab : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        gameObject.AddComponent<BodyPart>();
        PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/Resources/Parts/" + name + ".prefab");
#endif
    }
}

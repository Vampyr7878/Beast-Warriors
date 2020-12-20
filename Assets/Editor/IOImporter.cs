using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[UnityEditor.AssetImporters.ScriptedImporter(1, "io")]
public class IOImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        List<Part> parts = new List<Part>();
        string name = "default";
        using (FileStream file = new FileStream(ctx.assetPath, FileMode.Open))
        {
            ZipFile zip = new ZipFile(file);
            zip.Password = "soho0909";
            foreach (ZipEntry entry in zip)
            {
                if (entry.Name == "model2.ldr")
                {
                    using (StreamReader reader = new StreamReader(zip.GetInputStream(entry)))
                    {
                        string line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        name = line.Substring(9);
                        string[] words;
                        do
                        {
                            line = reader.ReadLine();
                            words = line.Split(' ');
                            if (words[0] == "1")
                            {
                                parts.Add(new Part(words));
                            }
                        } while (line != "0 NOFILE") ;
                    }
                    break;
                }
            }
        }
        GameObject prefab;
        GameObject main = new GameObject(name);
        GameObject[] meshes = new GameObject[parts.Count];
        List<Material> materials = new List<Material>();
        for (int i = 0; i < parts.Count; i++)
        {
            if (!materials.Exists(m => m.name == parts[i].Color.ToString()))
            {
                materials.Add(new Material(Resources.Load<Material>("Materials/" + parts[i].Color.ToString())));
            }
        }
        for (int i = 0; i < parts.Count; i++)
        {
            try
            {
                prefab = Resources.Load<GameObject>("Parts/" + parts[i].Name);
                meshes[i] = Instantiate(prefab, main.transform);
                meshes[i].transform.position = new Vector3(-parts[i].Matrix.m03, parts[i].Matrix.m13, -parts[i].Matrix.m23);
                meshes[i].transform.Rotate(-parts[i].Matrix.rotation.eulerAngles.x, parts[i].Matrix.rotation.eulerAngles.y, -parts[i].Matrix.rotation.eulerAngles.z);
                meshes[i].GetComponent<MeshRenderer>().material = materials.Find(m => m.name == parts[i].Color.ToString());
            }
            catch
            {
                Debug.LogError(parts[i].Name);
            }
        }
        main.transform.Rotate(0f, 90f, 0f);
        main.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
        ctx.AddObjectToAsset(name, main);
        ctx.SetMainObject(main);
        foreach (GameObject mesh in meshes)
        {
            ctx.AddObjectToAsset(mesh.name, mesh);
        }
        foreach (Material material in materials)
        {
            ctx.AddObjectToAsset(material.name, material);
        }
    }
}

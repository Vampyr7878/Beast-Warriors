using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[UnityEditor.AssetImporters.ScriptedImporter(1, "io")]
public class IOImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        List<Part> parts = new();
        string name = "default";
        using (FileStream file = new(ctx.assetPath, FileMode.Open))
        {
            ZipFile zip = new(file)
            {
                Password = "soho0909"
            };
            foreach (ZipEntry entry in zip)
            {
                if (entry.Name == "model2.ldr")
                {
                    using StreamReader reader = new(zip.GetInputStream(entry));
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
                    } while (line != "0 NOFILE");
                    break;
                }
            }
        }
        GameObject prefab;
        GameObject main = new(name);
        GameObject[] meshes = new GameObject[parts.Count];
        List<Material> materials = new();
        for (int i = 0; i < parts.Count; i++)
        {
            try
            {
                if (!materials.Exists(m => m.name == parts[i].Color.ToString()))
                {
                    materials.Add(new Material(Resources.Load<Material>($"Materials/{parts[i].Color}")));
                }
            }
            catch
            {
                Debug.LogError($"{name}-Color:{parts[i].Color}");
            }
        }
        for (int i = 0; i < parts.Count; i++)
        {
            try
            {
                prefab = Resources.Load<GameObject>($"Parts/{parts[i].Name.Replace("rename_", "")}");
                meshes[i] = Instantiate(prefab, main.transform);
                meshes[i].name += $"-{i}";
                meshes[i].transform.localPosition = new Vector3(-parts[i].Matrix.m03, -parts[i].Matrix.m13, -parts[i].Matrix.m23);
                meshes[i].transform.localRotation = parts[i].Matrix.rotation;
                SpecialCases(parts[i], meshes[i]);
                meshes[i].GetComponent<MeshRenderer>().material = materials.Find(m => m.name == parts[i].Color.ToString());
            }
            catch
            {
                Debug.LogError($"{name}-Part:{parts[i].Name}");
            }
        }
        main.transform.Rotate(0f, 90f, 0f);
        main.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
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

    private void SpecialCases(Part part, GameObject mesh)
    {
        if (part.Name == "93571")
        {
            mesh.tag = "Body";
            mesh.transform.Translate(0f, 0f, 19.66f, Space.Self);
        }
        else if (part.Name == "90626")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "64262")
        {
            mesh.tag = "Head";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "93277")
        {
            mesh.tag = "Head";
        }
        else if (part.Name == "92208")
        {
            mesh.tag = "Head";
        }
        else if (part.Name == "92210")
        {
            mesh.tag = "Head";
        }
        else if (part.Name == "92211")
        {
            mesh.tag = "Head";
        }
        else if (part.Name == "92219")
        {
            mesh.tag = "Head";
        }
        else if (part.Name == "90616")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "90608")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "93575")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "90615")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else if (part.Name == "90607")
        {
            mesh.tag = "Body";
            mesh.GetComponent<BodyPart>().isBodyPart = true;
        }
        else
        {
            mesh.tag = "Body";
        }
    }
}

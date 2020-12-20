using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject[] choices;

    GameObject[] instances;

    SqliteConnection connection;
    
    string dbPath;

    List<string> characterNames;

    List<string> characterPaths;

    int page;

    void Awake()
    {
        page = 0;
        dbPath = "URI=file:" + Application.streamingAssetsPath + "/database.sqlite";
        connection = new SqliteConnection(dbPath);
        characterNames = new List<string>();
        characterPaths = new List<string>();
        connection.Open();
        using (SqliteCommand command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Characters;";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                characterNames.Add(reader.GetString(1));
                characterPaths.Add(reader.GetString(2));
            }
        }
        connection.Close();
        instances = new GameObject[choices.Length];
    }

    void Start()
    {
        LoadCharacters();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit))
            {
                PlayerPrefs.SetString("Character", hit.collider.GetComponent<Choice>().character);
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    void LoadCharacters()
    {
        foreach (GameObject instance in instances)
        {
            if (instance != null)
            {
                DestroyImmediate(instance);
            }
        }
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].GetComponentInChildren<TextMeshPro>().text = characterNames[i + page * choices.Length];
            choices[i].GetComponent<Choice>().character = characterPaths[i + page * choices.Length];
            GameObject prefab = Resources.Load<GameObject>("Beast Warriors/" + characterPaths[i + page * choices.Length]);
            instances[i] = Instantiate(prefab, choices[i].transform);
            instances[i].GetComponent<BeastWarrior>().enabled = false;
        }
    }

    public void PreviousButton()
    {
        int value = page == 0 ? page = 0 : page - 1;
        if (value != page)
        {
            page = value;
            LoadCharacters();
        }
    }

    public void NextButton()
    {
        int value = page == (characterNames.Count / 3) - 1 ? page : page + 1;
        if (value != page)
        {
            page = value;
            LoadCharacters();
        }
    }
}

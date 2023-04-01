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

    List<bool> characterEnabled;

    List<int> activeCharacters;

    int page;

    int pages;

    void Awake()
    {
        page = 0;
        dbPath = "URI=file:" + Application.streamingAssetsPath + "/database.sqlite";
        connection = new SqliteConnection(dbPath);
        characterNames = new List<string>();
        characterPaths = new List<string>();
        characterEnabled = new List<bool>();
        activeCharacters = new List<int>();
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
                characterEnabled.Add(reader.GetBoolean(3));
            }
        }
        connection.Close();
        instances = new GameObject[choices.Length];
        pages = Mathf.CeilToInt(characterEnabled.FindAll(b => b).Count / (float)choices.Length);
        for (int i = 0; i < characterEnabled.Count; i++)
        {
            if (characterEnabled[i])
            {
                activeCharacters.Add(i);
            }
        }
        string character = PlayerPrefs.GetString("Character");
        if (!string.IsNullOrEmpty(character))
        {
            int index = activeCharacters.FindIndex(c => characterPaths[c] == character);
            if (index >= 0)
            {
                page = index / choices.Length;
            }
        }
    }

    void Start()
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
#endif
        LoadCharacters();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
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
            if (i + page * choices.Length >= activeCharacters.Count)
            {
                choices[i].SetActive(false);
            }
            else
            {
                choices[i].SetActive(true);
                choices[i].GetComponentInChildren<TextMeshPro>().text = characterNames[activeCharacters[i + page * choices.Length]];
                choices[i].GetComponent<Choice>().character = characterPaths[activeCharacters[i + page * choices.Length]];
                GameObject prefab = Resources.Load<GameObject>("Beast Warriors/" + characterPaths[activeCharacters[i + page * choices.Length]]);
                instances[i] = Instantiate(prefab, choices[i].transform);
                instances[i].GetComponent<BeastWarrior>().enabled = false;
            }
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
        int value = page == pages - 1 ? page : page + 1;
        if (value != page)
        {
            page = value;
            LoadCharacters();
        }
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

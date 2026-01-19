using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class CharacterSelect : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject[] choices;

    private GameObject[] instances;

    private SqliteConnection connection;

    private string dbPath;

    private List<string> characterNames;

    private List<string> characterPaths;

    private List<bool> characterEnabled;

    private List<int> activeCharacters;

    private int page;

    private int pages;

    private int selected;

    void Awake()
    {
        page = 0;
        selected = 0;
        dbPath = $"URI=file:{Application.streamingAssetsPath}/database.sqlite";
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
                selected = index % 3;
                UpdateSelection();
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
                try
                {
                    choices[i].SetActive(true);
                    choices[i].GetComponentInChildren<TextMeshPro>().text = characterNames[activeCharacters[i + page * choices.Length]];
                    choices[i].GetComponent<Choice>().character = characterPaths[activeCharacters[i + page * choices.Length]];
                    GameObject prefab = Resources.Load<GameObject>($"Beast Warriors/{characterPaths[activeCharacters[i + page * choices.Length]]}");
                    instances[i] = Instantiate(prefab, choices[i].transform);
                    instances[i].GetComponent<BeastWarrior>().enabled = false;
                }
                catch
                {
                    choices[i].SetActive(false);
                }
            }
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            if (i == selected)
            {
                choices[i].GetComponent<Choice>().EnableOutline(true);
            }
            else
            {
                choices[i].GetComponent<Choice>().EnableOutline(false);
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
            selected = 2;
            UpdateSelection();
        }
    }

    public void NextButton()
    {
        int value = page == pages - 1 ? page : page + 1;
        if (value != page)
        {
            page = value;
            LoadCharacters();
            selected = 0;
            UpdateSelection();
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

    public void OnExit(CallbackContext context)
    {
        if (context.performed)
        {
            ExitButton();
        }
    }

    public void OnPrevious(CallbackContext context)
    {
        if (context.performed)
        {
            PreviousButton();
        }
    }

    public void OnNext(CallbackContext context)
    {
        if (context.performed)
        {
            NextButton();
        }
    }

    public void OnMove(CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();
            if (value < 0f)
            {
                selected = selected == 0 ? 0 : selected - 1;
            }
            else if (value > 0f)
            {
                if (choices[2].activeSelf)
                {
                    selected = selected == 2 ? 2 : selected + 1;
                }
                else if (choices[1].activeSelf)
                {
                    selected = selected == 1 ? 1 : selected + 1;
                }
            }
            UpdateSelection();
        }
    }

    public void OnSelect(CallbackContext context)
    {
        PlayerPrefs.SetString("Character", choices[selected].GetComponent<Choice>().character);
        SceneManager.LoadScene("SampleScene");
    }
}

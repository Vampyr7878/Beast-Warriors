using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class Menu : MonoBehaviour
{
    public Button[] buttons;

    private int selected;

    void Start()
    {
        selected = 0;
        buttons[selected].Select();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
                selected = selected == buttons.Length - 1 ? buttons.Length - 1 : selected + 1;
            }
            buttons[selected].Select();
        }
    }

    public void OnSelect(CallbackContext context)
    {
        switch (selected)
        {
            case 0:
                StartButton();
                break;
            case 1:
                ExitButton();
                break;
        }
    }
}

using UnityEngine;

public class Choice : MonoBehaviour
{
    public string character;

    public GameObject Outline;

    void FixedUpdate()
    {

        GetComponentInChildren<Animator>().transform.Rotate(0f, -1f, 0f);
    }

    public void EnableOutline(bool enabled)
    {
        Outline.SetActive(enabled);
    }
}

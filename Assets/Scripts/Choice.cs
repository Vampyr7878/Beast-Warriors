using UnityEngine;

public class Choice : MonoBehaviour
{
    public string character;

    void FixedUpdate()
    {

        GetComponentInChildren<Animator>().transform.Rotate(0f, -1f, 0f);
    }
}

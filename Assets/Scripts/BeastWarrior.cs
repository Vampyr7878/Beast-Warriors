using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public abstract class BeastWarrior : MonoBehaviour
{
    protected Camera characterCamera;

    protected Animator animator;

    protected int weapon;

    protected float[] cameraPosition;

    protected void Awake()
    {
        cameraPosition = new float[4];
        cameraPosition[0] = 0f;
        cameraPosition[1] = 0f;
        cameraPosition[2] = -1.5f;
        cameraPosition[3] = -1.5f;
        weapon = 1;
    }

    protected void Start()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        characterCamera = transform.parent.GetComponent<Character>().characterCamera;
    }

    protected void FixedUpdate()
    {
        float x = characterCamera.transform.localPosition.x;
        if (characterCamera.transform.localPosition.x > cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x - 0.1f, cameraPosition[weapon - 1], x);
        }
        else if (characterCamera.transform.localPosition.x < cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x + 0.1f, x, cameraPosition[weapon - 1]);
        }
        characterCamera.transform.localPosition = new Vector3(x, characterCamera.transform.localPosition.y, characterCamera.transform.localPosition.z);
    }

    public abstract void OnMeleeWeak(CallbackContext context);

    public abstract void OnMeleeStrong(CallbackContext context);

    public abstract void OnRangedWeak(CallbackContext context);

    public abstract void OnRangedStrong(CallbackContext context);
}

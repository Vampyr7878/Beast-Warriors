using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour
{
    public Camera characterCamera;

    public List<Transform> skeleton;

    public LayerMask mask;

    public float speed;

    public float jump;

    public float cameraRange;

    BeastWarrior warrior;

    Transform[] bodyParts;

    List<Collision> terrainCollsion;

    float cameraAngle;

    Rigidbody body;

    Animator animator;

    Vector3 movement;

    Vector2 move;

    Vector2 look;

    bool isMoving;

    bool isJumping;

    void Awake()
    {
        cameraAngle = characterCamera.transform.rotation.eulerAngles.x;
        isMoving = false;
        isJumping = false;
        bodyParts = new Transform[skeleton.Count];
        terrainCollsion = new List<Collision>();
        string character = PlayerPrefs.GetString("Character");
        GameObject prefab = Resources.Load<GameObject>("Beast Warriors/" + character);
        GameObject instance = Instantiate(prefab, transform);
        RuntimeAnimatorController controller = instance.GetComponent<Animator>().runtimeAnimatorController;
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = controller;
        warrior = instance.GetComponent<BeastWarrior>();
        PlayerPrefs.SetString("Character", character);
    }

    void Start()
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
#endif
        body = GetComponent<Rigidbody>();
        int i = 0;
        foreach (BodyPart child in GetComponentsInChildren<BodyPart>())
        {
            if (child.isBodyPart)
            {
                i = skeleton.FindIndex(b => b.name == child.tag);
                if (i >= 0)
                {
                    if (child.tag.Contains("Foot"))
                    {
                        child.isBodyPart = false;
                        GameObject foot = new(child.tag.Replace(" ", ""));
                        foot.transform.parent = child.transform.parent;
                        foot.transform.position = child.transform.position;
                        foot.transform.localEulerAngles = new Vector3(0f, -180f, -90f);
                        foot.tag = child.tag;
                        bodyParts[i] = foot.transform;
                    }
                    else
                    {
                        bodyParts[i] = child.transform;
                    }
                }
            }
        }
        foreach (BodyPart child in GetComponentsInChildren<BodyPart>(true))
        {
            if (!child.isBodyPart)

            {
                i = skeleton.FindIndex(b => b.name == child.tag);
                if (i >= 0)
                {
                    child.transform.parent = bodyParts[i];
                }
            }
        }
        PlayerInput input = GetComponent<PlayerInput>();
        input.actionEvents[4].AddListener(warrior.OnMeleeWeak);
        input.actionEvents[5].AddListener(warrior.OnMeleeStrong);
        input.actionEvents[6].AddListener(warrior.OnRangedWeak);
        input.actionEvents[7].AddListener(warrior.OnRangedStrong);
        input.actionEvents[8].AddListener(warrior.OnAttack);
    }

    void Update()
    {
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Jumping", isJumping);
        animator.SetBool("Forward", move.y >= 0f);
        animator.SetFloat("Blend", move.x);
    }

    void FixedUpdate()
    {
        if (terrainCollsion.Count > 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
            {
                isJumping = false;
            }
            float y = !isMoving && !isJumping ? 0f : body.velocity.y;
            movement = transform.right * -move.x * speed + transform.up * y + transform.forward * -move.y * speed;
            body.velocity = movement;
        }
        if (body.velocity.y < 0)
        {   
            body.AddForce(Physics.gravity * 4, ForceMode.Acceleration);
        }
        else
        {
            body.AddForce(Physics.gravity, ForceMode.Acceleration);
        }    
        transform.Rotate(0f, look.x, 0f);
        Vector3 rotation = characterCamera.transform.rotation.eulerAngles;
        float angle = Mathf.Clamp(rotation.x - look.y, cameraAngle - cameraRange, cameraAngle + cameraRange);
        characterCamera.transform.eulerAngles = new Vector3(angle, rotation.y, rotation.z);
        skeleton[2].eulerAngles = new Vector3(angle - cameraAngle, skeleton[2].eulerAngles.y, skeleton[2].eulerAngles.z);
        MoveParts();
    }

    void MoveParts()
    {
        for (int i = 0; i < skeleton.Count; i++)
        {
            if (bodyParts[i] != null)
            {
                bodyParts[i].transform.position = skeleton[i].position;
                bodyParts[i].transform.rotation = skeleton[i].rotation;
            }
        }
    }

    public void OnMove(CallbackContext context)
    {
        isMoving = !context.canceled;
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext context)
    {
        if (terrainCollsion.Count > 0)
        {
            body.AddForce(Vector3.up * jump, ForceMode.Impulse);
            isJumping = true;
            animator.SetTrigger("Jump");
        }
    }

    public void OnExit(CallbackContext context)
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & mask) != 0)
        {
            if (!terrainCollsion.Contains(collision))
            {
                terrainCollsion.Add(collision);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (terrainCollsion.Contains(collision))
        {
            terrainCollsion.Remove(collision);
        }
    }
}

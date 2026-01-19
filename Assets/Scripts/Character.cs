using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Character : MonoBehaviour
{
    public Camera characterCamera;

    public Animator mainAnimator;

    public List<Transform> skeleton;

    public List<Transform> robot;

    public LayerMask mask;

    public float speed;

    public float jump;

    public float cameraRange;

    private BeastWarrior warrior;

    private Transform[] bodyParts;

    private List<Collision> terrainCollsion;

    private float cameraAngle;

    private Rigidbody body;

    private Vector3 movement;

    private Vector2 move;

    private Vector2 look;

    private bool isMoving;

    private bool isJumping;

    private bool turningRight;

    private bool turningLeft;

    private bool overrideRightArm;

    private bool overrideLeftArm;

    void Awake()
    {
        foreach(var bone in skeleton)
        {
            bone.GetComponent<MeshRenderer>().enabled = false;
        }
        cameraAngle = characterCamera.transform.rotation.eulerAngles.x;
        isMoving = false;
        isJumping = false;
        turningRight = false;
        turningLeft = false;
        bodyParts = new Transform[skeleton.Count];
        terrainCollsion = new List<Collision>();
        string character = PlayerPrefs.GetString("Character");
        GameObject prefab = Resources.Load<GameObject>($"Beast Warriors/{character}");
        GameObject instance = Instantiate(prefab, transform);
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
        mainAnimator.SetBool("Moving", isMoving);
        mainAnimator.SetBool("Jumping", isJumping);
        mainAnimator.SetBool("Right", turningRight);
        mainAnimator.SetBool("Left", turningLeft);
        mainAnimator.SetBool("Forward", move.y >= 0f);
        mainAnimator.SetFloat("Blend", move.x);
    }

    void FixedUpdate()
    {
        if (terrainCollsion.Count > 0)
        {
            if (mainAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
            {
                isJumping = false;
            }
            float y = !isMoving && !isJumping ? 0f : body.linearVelocity.y;
            movement = -move.x * speed * transform.right + transform.up * y + -move.y * speed * transform.forward;
            body.linearVelocity = movement;
        }
        if (body.linearVelocity.y < 0)
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
        skeleton[(int)BodyPart.Part.Body].localPosition = new Vector3(-robot[(int)BodyPart.Part.Body].localPosition.x * 350,
            robot[(int)BodyPart.Part.Body].localPosition.y * 400, robot[(int)BodyPart.Part.Body].localPosition.z * 300);
        skeleton[(int)BodyPart.Part.Head].eulerAngles = new Vector3(angle - cameraAngle, skeleton[2].eulerAngles.y, skeleton[2].eulerAngles.z);
        skeleton[(int)BodyPart.Part.RightHip].localRotation = robot[(int)BodyPart.Part.RightHip].localRotation;
        skeleton[(int)BodyPart.Part.RightKnee].localRotation = robot[(int)BodyPart.Part.RightKnee].localRotation;
        skeleton[(int)BodyPart.Part.RightFoot].localRotation = robot[(int)BodyPart.Part.RightFoot].localRotation;
        skeleton[(int)BodyPart.Part.LeftHip].localRotation = robot[(int)BodyPart.Part.LeftHip].localRotation;
        skeleton[(int)BodyPart.Part.LeftKnee].localRotation = robot[(int)BodyPart.Part.LeftKnee].localRotation;
        skeleton[(int)BodyPart.Part.LeftFoot].localRotation = robot[(int)BodyPart.Part.LeftFoot].localRotation;
        if (!overrideRightArm)
        {
            skeleton[(int)BodyPart.Part.RightShoulder].localRotation = robot[(int)BodyPart.Part.RightShoulder].localRotation;
            skeleton[(int)BodyPart.Part.RightElbow].localRotation = robot[(int)BodyPart.Part.RightElbow].localRotation;
            skeleton[(int)BodyPart.Part.RightHand].localRotation = robot[(int)BodyPart.Part.RightHand].localRotation;
            skeleton[(int)BodyPart.Part.RightClaw].localRotation = robot[(int)BodyPart.Part.RightElbow].localRotation;
        }
        if (!overrideLeftArm)
        {
            skeleton[(int)BodyPart.Part.LeftShoulder].localRotation = robot[(int)BodyPart.Part.LeftShoulder].localRotation;
            skeleton[(int)BodyPart.Part.LeftElbow].localRotation = robot[(int)BodyPart.Part.LeftElbow].localRotation;
            skeleton[(int)BodyPart.Part.LeftHand].localRotation = robot[(int)BodyPart.Part.LeftHand].localRotation;
            skeleton[(int)BodyPart.Part.LeftClaw].localRotation = robot[(int)BodyPart.Part.LeftElbow].localRotation;
        }
        MoveParts();
    }

    void MoveParts()
    {
        for (int i = 0; i < skeleton.Count; i++)
        {
            if (bodyParts[i] != null)
            {
                bodyParts[i].transform.SetPositionAndRotation(skeleton[i].position, skeleton[i].rotation);
                CompensateLimb(i, "Right Arm", 90f, 0f, 0f);
                CompensateLimb(i, "Right Elbow", 0f, -90f, 90f);
                CompensateLimb(i, "Right Forearm", 90f, 0f, 0f);
                CompensateLimb(i, "Right Hand", 0f, 90f, 90f);
                CompensateLimb(i, "Left Arm", 90f, 0f, 0f);
                CompensateLimb(i, "Left Elbow", 0f, -90f, 90f);
                CompensateLimb(i, "Left Forearm", 90f, 0f, 0f);
                CompensateLimb(i, "Left Hand", 0f, 90f, 90f);
                CompensateLimb(i, "Right Leg", 90f, 0f, -90f);
                CompensateLimb(i, "Right Lower", 90f, 0f, 90f);
                CompensateLimb(i, "Right Foot", 0f, -90f, 30f);
                CompensateLimb(i, "Left Leg", 90f, 0f, 90f);
                CompensateLimb(i, "Left Lower", 90f, 0f, -90f);
                CompensateLimb(i, "Left Foot", 0f, -90f, 30f);
                CompensateLimb(i, "Right Long", 90f, 0f, 180f);
                CompensateLimb(i, "Right Claw", 0f, -90f, 90f);
                CompensateLimb(i, "Right Hook", 90f, 0f, 0f);
                CompensateLimb(i, "Left Long", 90f, 0f, 180f);
                CompensateLimb(i, "Left Claw", 0f, -90f, 90f);
                CompensateLimb(i, "Left Hook", 90f, 0f, 0f);
            }
        }
    }

    void CompensateLimb(int index, string tag, float x, float y, float z)
    {
        if (bodyParts[index].CompareTag(tag))
        {
            bodyParts[index].transform.localRotation *= Quaternion.Euler(x, y, z);
        }
    }

    public void OverrideArm(BeastWarrior.WeaponArm arm)
    {
        switch (arm)
        {
            case BeastWarrior.WeaponArm.Right:
                overrideRightArm = true;
                overrideLeftArm = false;
                break;
            case BeastWarrior.WeaponArm.Left:
                overrideRightArm = false;
                overrideLeftArm = true;
                break;
            case BeastWarrior.WeaponArm.Both:
                overrideRightArm = true;
                overrideLeftArm = true;
                break;
            case BeastWarrior.WeaponArm.None:
            default:
                overrideRightArm = false;
                overrideLeftArm = false;
                break;
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
        if (look.x > 0)
        {
            turningRight = true;
            turningLeft = false;
        }
        else if (look.x < 0)
        {
            turningRight = false;
            turningLeft = true;
        }
        else
        {
            turningRight = false;
            turningLeft = false;
        }
    }

    public void OnJump(CallbackContext context)
    {
        if (terrainCollsion.Count > 0)
        {
            mainAnimator.SetTrigger("Jump");
            body.AddForce(Vector3.up * jump, ForceMode.Impulse);
            isJumping = true;
        }
    }

    public void OnExit(CallbackContext context)
    {
        if (context.canceled)
        {
            SceneManager.LoadScene("CharacterSelect");
        }
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

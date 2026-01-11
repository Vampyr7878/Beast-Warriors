using UnityEngine;

public class Thrown : MonoBehaviour
{
    public GameObject flash;

    public float flashMultiply;

    public bool spin;

    public Vector3 forward;

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (spin)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 30);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Character" && !other.isTrigger)
        {
            body.linearVelocity = Vector3.zero;
            spin = false;
            GameObject f = Instantiate(flash);
            f.transform.position = transform.position - GetComponent<BoxCollider>().center + forward * flashMultiply;
            f.transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            Destroy(gameObject);
        }
    }
}

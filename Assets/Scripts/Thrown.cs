using UnityEngine;

public class Thrown : MonoBehaviour
{
    public float speed;

    public GameObject flash;

    public Vector3 forward;

    public bool spin;
    
    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.linearVelocity = forward * speed;
    }

    private void FixedUpdate()
    {
        if (spin)
        {
            transform.Rotate(0f, 0f, 30f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Character" && body != null)
        {
            body.linearVelocity = Vector3.zero;
            spin = false;
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point + contact.normal * 0.1f;
            Instantiate(flash, position, Quaternion.Euler(-transform.eulerAngles.x, transform.eulerAngles.y, 0f));
            Destroy(gameObject);
        }
    }
}

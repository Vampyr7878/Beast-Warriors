using UnityEngine;

public class Thrown : MonoBehaviour
{
    public GameObject flash;

    public float flashMultiply;

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Character" && !other.isTrigger)
        {
            body.velocity = Vector3.zero;
            GameObject f = Instantiate(flash);
            f.transform.position = transform.position + transform.forward * flashMultiply;
            f.transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            Destroy(gameObject);
        }
    }
}

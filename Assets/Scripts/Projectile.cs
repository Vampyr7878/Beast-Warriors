using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    public float speed;

    public GameObject flash;

    private Rigidbody body;

    private new Light light;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.linearVelocity = -transform.forward * speed;
        light = GetComponent<Light>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Character" && body != null)
        {
            body.linearVelocity = Vector3.zero;
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GameObject f = Instantiate(flash, position, Quaternion.Euler(-transform.eulerAngles.x, transform.eulerAngles.y, 0f));
            if (light != null)
            {
                f.GetComponent<Light>().color = light.color;
                MainModule m = f.GetComponent<ParticleSystem>().main;
                m.startColor = new MinMaxGradient(light.color);
            }
            Destroy(gameObject);
        }
    }
}

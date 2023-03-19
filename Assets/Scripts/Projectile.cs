using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    public float speed;

    public GameObject flash;

    public float flashMultiply;

    private Rigidbody body;

    private new Light light;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = -transform.forward * speed;
        light = GetComponent<Light>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Character" && !other.isTrigger)
        {
            body.velocity = Vector3.zero;
            GameObject f = Instantiate(flash);
            f.transform.position = transform.position + transform.forward * flashMultiply;
            f.transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            f.GetComponent<Light>().color = light.color;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(light.color);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    public float speed;

    public GameObject energyFlash;

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
        if (other.gameObject.name != "Character")
        {
            GameObject ef = Instantiate(energyFlash);
            ef.transform.position = transform.position;
            ef.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            ef.GetComponent<Light>().color = light.color;
            MainModule m = ef.GetComponent<ParticleSystem>().main;
            Color color = light.color;
            color.a /= 8;
            m.startColor = new MinMaxGradient(color);
            Destroy(gameObject);
        }
    }
}

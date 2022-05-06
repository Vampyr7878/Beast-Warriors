using UnityEngine;

public class RayProjectile : MonoBehaviour
{
    public float lifespan;

    private float time;

    void Start()
    {
        time = 0;
    }

    void FixedUpdate()
    {
        if (time >= lifespan)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }
}

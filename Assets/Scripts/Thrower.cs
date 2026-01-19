using UnityEngine;

public class Thrower : MonoBehaviour {

    public Light start;

    public Light end;

    public float lightMin;

    public float lightMax;

    public float lightRate;

    private float time;

    public void Awake()
    {
        time = 0f;
    }

    public void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= lightRate)
        {
            start.intensity = Random.Range(lightMin, lightMax);
            end.intensity = Random.Range(lightMin, lightMax);
            time = 0f;
        }
    }
}

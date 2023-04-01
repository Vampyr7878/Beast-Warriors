using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public abstract class BeastWarrior : MonoBehaviour
{
    protected Camera characterCamera;

    protected Transform cameraAimHelper;

    protected Animator animator;

    protected int weapon;

    protected float[] cameraPosition;

    protected bool lightShoot;

    protected bool heavyShoot;

    protected void Awake()
    {
        cameraPosition = new float[4];
        cameraPosition[0] = 0f;
        cameraPosition[1] = 0f;
        cameraPosition[2] = -1.5f;
        cameraPosition[3] = -1.5f;
        weapon = 1;
        lightShoot = false;
        heavyShoot = false;
    }

    protected void Start()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        characterCamera = transform.parent.GetComponent<Character>().characterCamera;
        cameraAimHelper = characterCamera.GetComponentsInChildren<Transform>()[1];
    }

    protected void FixedUpdate()
    {
        float x = characterCamera.transform.localPosition.x;
        if (characterCamera.transform.localPosition.x > cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x - 0.1f, cameraPosition[weapon - 1], x);
        }
        else if (characterCamera.transform.localPosition.x < cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x + 0.1f, x, cameraPosition[weapon - 1]);
        }
        characterCamera.transform.localPosition = new Vector3(x, characterCamera.transform.localPosition.y, characterCamera.transform.localPosition.z);
    }

    protected void RaycastBullet(GameObject bullet, Vector3 direction, LayerMask layerMask, GameObject barrel, bool audio = true)
    {
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject b = Instantiate(bullet);
            b.transform.position = barrel.transform.position;
            b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            b.GetComponent<AudioSource>().enabled = audio;
            GameObject h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(barrel.transform.position, hit.point, Color.blue, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.cyan, 3600);
        }
    }

    protected void RaycastLaser(LineRenderer laser, Vector3 direction, LayerMask layerMask, GameObject barrel, Color color)
    {
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            LineRenderer l = Instantiate(laser);
            l.transform.position = barrel.transform.position;
            l.SetPosition(0, Vector3.zero);
            l.SetPosition(1, hit.point - barrel.transform.position);
            l.startColor = color;
            l.endColor = color;
            l.material.SetColor("_Color", color);
            GameObject f = l.transform.GetChild(0).gameObject;
            f.GetComponent<Light>().color = color;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(color);
            GameObject h = l.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            h.GetComponent<Light>().color = color;
            m = h.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(color);
            Debug.DrawLine(barrel.transform.position, hit.point, Color.red, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.magenta, 3600);
        }
    }

    protected void ProjectileParticle(GameObject flash, GameObject projectile, Vector3 flashDirection, Vector3 projectileDirection, GameObject barrel, Color flashColor, Color projectileColor, Gradient gradient)
    {
        GameObject f = Instantiate(flash);
        f.transform.position = barrel.transform.position;
        f.transform.eulerAngles = flashDirection;
        f.GetComponent<Light>().color = flashColor;
        MainModule m = f.GetComponent<ParticleSystem>().main;
        m.startColor = new MinMaxGradient(flashColor);
        GameObject p = Instantiate(projectile);
        p.transform.position = barrel.transform.position;
        p.transform.eulerAngles = projectileDirection;
        p.GetComponent<Light>().color = projectileColor;
        ColorOverLifetimeModule c = p.GetComponent<ParticleSystem>().colorOverLifetime;
        c.color = new MinMaxGradient(gradient);
    }

    protected void ProjectileBolt(GameObject flash, LineRenderer projectile, Vector3 direction, GameObject barrel, Color color)
    {
        GameObject f = Instantiate(flash);
        f.transform.position = barrel.transform.position;
        f.transform.eulerAngles = direction;
        f.GetComponent<Light>().color = color;
        MainModule m = f.GetComponent<ParticleSystem>().main;
        m.startColor = new MinMaxGradient(color);
        LineRenderer b = Instantiate(projectile);
        b.transform.position = barrel.transform.position;
        b.transform.eulerAngles = direction;
        b.SetPosition(0, Vector3.zero);
        b.startColor = color;
        b.endColor = color;
        b.material.SetColor("_Color", color);
        b.GetComponent<Light>().color = color;
    }

    protected void MeshProjectile(GameObject flash, GameObject projectile, Vector3 direction, GameObject barrel, Material material)
    {
        GameObject e = Instantiate(flash);
        e.transform.position = barrel.transform.position;
        e.transform.eulerAngles = direction;
        GameObject m = Instantiate(projectile);
        m.transform.position = barrel.transform.position;
        m.transform.eulerAngles = direction;
        m.GetComponentInChildren<MeshRenderer>().material = material;
    }

    protected void ThrownProjectile(GameObject thrown, GameObject projectile, Vector3 direction, Vector3 aim, GameObject hold, int force)
    {
        GameObject t = Instantiate(thrown);
        Instantiate(projectile, t.transform);
        t.transform.position = hold.transform.position;
        t.transform.eulerAngles = direction;
        Rigidbody b = t.GetComponent<Rigidbody>();
        b.AddForce(aim * force, ForceMode.Impulse);
    }

    public abstract void OnMeleeWeak(CallbackContext context);

    public abstract void OnMeleeStrong(CallbackContext context);

    public abstract void OnRangedWeak(CallbackContext context);

    public abstract void OnRangedStrong(CallbackContext context);

    public abstract void OnAttack(CallbackContext context);
}

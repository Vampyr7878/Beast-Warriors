using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public class Iguanus : BeastWarrior
{
    public GameObject gun;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject energyFlash;

    public GameObject energyBall;

    public float fireRate;

    private float time;

    private int barrel;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun();
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            ShootEnergyBall();
        }
    }

    void EquipGun(GameObject attachment)
    {
        gun.transform.parent = attachment.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject b = Instantiate(bullet);
            b.transform.position = lightBarrels[barrel].transform.position;
            b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            GameObject h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(lightBarrels[barrel].transform.position, hit.point, Color.blue, 3600);
            b = Instantiate(bullet);
            b.transform.position = lightBarrels[barrel + 2].transform.position;
            b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(lightBarrels[barrel + 2].transform.position, hit.point, Color.blue, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(Vector3.forward) * hit.distance, Color.cyan, 3600);
        }
        barrel = barrel == (lightBarrels.Length - 3) ? 0 : barrel + 1;
    }

    void ShootEnergyBall()
    {
        GameObject ef = Instantiate(energyFlash);
        ef.transform.position = heavyBarrel.transform.position;
        ef.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        ef.GetComponent<Light>().color = Color.magenta;
        MainModule m = ef.GetComponent<ParticleSystem>().main;
        Color color = Color.magenta;
        color.a /= 8;
        m.startColor = new MinMaxGradient(color);
        GameObject eb = Instantiate(energyBall);
        eb.transform.position = heavyBarrel.transform.position;
        eb.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        eb.GetComponent<Light>().color = Color.green;
        ColorOverLifetimeModule c = eb.GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient g = new Gradient();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = Color.magenta;
        colors[0].time = 0f;
        colors[1].color = Color.green;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        c.color = new MinMaxGradient(g);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(hold);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                barrel = 0;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

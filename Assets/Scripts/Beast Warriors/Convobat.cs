using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public class Convobat : BeastWarrior
{
    public GameObject rightScimitar;

    public GameObject leftScimitar;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject ripple;

    public GameObject sonic;

    public Color laserColor;

    public Color rippleColor;

    public float laserInaccuracy;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLaser();
        }
        if (heavyShoot)
        {
            ShootRipple();
        }
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new Vector3(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            LineRenderer l = Instantiate(laser);
            l.transform.position = lightBarrels[0].transform.position;
            l.SetPosition(0, Vector3.zero);
            l.SetPosition(1, hit.point - lightBarrels[0].transform.position);
            l.startColor = laserColor;
            l.endColor = laserColor;
            l.material.SetColor("_Color", laserColor);
            GameObject f = l.transform.GetChild(0).gameObject;
            f.GetComponent<Light>().color = laserColor;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(laserColor);
            GameObject h = l.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            h.GetComponent<Light>().color = laserColor;
            m = h.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(laserColor);
            l = Instantiate(laser);
            l.transform.position = lightBarrels[1].transform.position;
            l.SetPosition(0, Vector3.zero);
            l.SetPosition(1, hit.point - lightBarrels[1].transform.position);
            l.startColor = laserColor;
            l.endColor = laserColor;
            l.material.SetColor("_Color", laserColor);
            f = l.transform.GetChild(0).gameObject;
            f.GetComponent<Light>().color = laserColor;
            m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(laserColor);
            h = l.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            h.GetComponent<Light>().color = laserColor;
            m = h.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(laserColor);
            Debug.DrawLine(lightBarrels[0].transform.position, hit.point, Color.red, 3600);
            Debug.DrawLine(lightBarrels[1].transform.position, hit.point, Color.red, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.magenta, 3600);
        }
        lightShoot = false;
    }

    void ShootRipple()
    {
        GameObject s = Instantiate(sonic);
        s.transform.position = heavyBarrels[0].transform.position;
        s.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y + 180, 0f);
        s.GetComponent<Light>().color = rippleColor;
        MainModule m = s.GetComponent<ParticleSystem>().main;
        m.startColor = new MinMaxGradient(rippleColor);
        s = Instantiate(sonic);
        s.transform.position = heavyBarrels[1].transform.position;
        s.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y + 180, 0f);
        s.GetComponent<Light>().color = rippleColor;
        m = s.GetComponent<ParticleSystem>().main;
        m.startColor = new MinMaxGradient(rippleColor);
        GameObject r = Instantiate(ripple);
        r.transform.position = heavyBarrels[0].transform.position;
        r.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        r.GetComponent<Light>().color = rippleColor;
        ColorOverLifetimeModule c = r.GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient g = new Gradient();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = rippleColor;
        colors[0].time = 0f;
        colors[1].color = rippleColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[3];
        alphas[0].alpha = 0f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 0.5f;
        alphas[2].alpha = 0f;
        alphas[2].time = 1f;
        g.SetKeys(colors, alphas);
        c.color = new MinMaxGradient(g);
        r = Instantiate(ripple);
        r.transform.position = heavyBarrels[1].transform.position;
        r.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        r.GetComponent<Light>().color = rippleColor;
        c = r.GetComponent<ParticleSystem>().colorOverLifetime;
        c.color = new MinMaxGradient(g);
        heavyShoot = false;
    }

    void EquipRightScimitar(GameObject attachment)
    {
        rightScimitar.transform.parent = attachment.transform;
        rightScimitar.transform.localPosition = Vector3.zero;
        rightScimitar.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLeftScimitar(GameObject attachment)
    {
        leftScimitar.transform.parent = attachment.transform;
        leftScimitar.transform.localPosition = Vector3.zero;
        leftScimitar.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHold);
        EquipLeftScimitar(leftHold);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

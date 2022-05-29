using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public class Armordillo : BeastWarrior
{
    public GameObject mace;

    public GameObject gun;

    public GameObject maceHolster;

    public GameObject gunHolster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public LineRenderer laser;

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
            ShootLaser();
        }
    }

    void EquipMace(GameObject attachment)
    {
        mace.transform.parent = attachment.transform;
        mace.transform.localPosition = Vector3.zero;
        mace.transform.localEulerAngles = Vector3.zero;
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
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(Vector3.forward) * hit.distance, Color.cyan, 3600);
        }
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            LineRenderer l = Instantiate(laser);
            l.transform.position = heavyBarrel.transform.position;
            l.SetPosition(0, Vector3.zero);
            l.SetPosition(1, hit.point - heavyBarrel.transform.position);
            l.startColor = Color.red;
            l.endColor = Color.red;
            l.material.SetColor("_Color", Color.red);
            GameObject f = l.transform.GetChild(0).gameObject;
            f.GetComponent<Light>().color = Color.red;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(Color.red);
            GameObject h = l.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            h.GetComponent<Light>().color = Color.red;
            m = h.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(Color.red);
            Debug.DrawLine(heavyBarrel.transform.position, hit.point, Color.red, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(Vector3.forward) * hit.distance, Color.magenta, 3600);
        }
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipGun(gunHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(hold);
        EquipGun(gunHolster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipGun(gunHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipGun(hold);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch(weapon)
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

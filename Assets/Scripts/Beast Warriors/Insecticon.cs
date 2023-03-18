using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public class Insecticon : BeastWarrior
{
    public GameObject rightSickle;

    public GameObject leftSickle;

    public GameObject crossbow;

    public GameObject right;

    public GameObject left;

    public GameObject holster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject energyFlash;

    public LineRenderer bolt;

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

    void EquipRightSickle(GameObject attachment)
    {
        rightSickle.transform.parent = attachment.transform;
        rightSickle.transform.localPosition = Vector3.zero;
        rightSickle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLeftSickle(GameObject attachment)
    {
        leftSickle.transform.parent = attachment.transform;
        leftSickle.transform.localPosition = Vector3.zero;
        leftSickle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipCrossbow(GameObject attachment)
    {
        crossbow.transform.parent = attachment.transform;
        crossbow.transform.localPosition = Vector3.zero;
        crossbow.transform.localEulerAngles = Vector3.zero;
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
        Color color = Color.green;
        color.a /= 8;
        m.startColor = new MinMaxGradient(color);
        LineRenderer l = Instantiate(bolt);
        l.transform.position = heavyBarrel.transform.position;
        l.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        l.SetPosition(0, Vector3.zero);
        l.startColor = Color.green;
        l.endColor = Color.green;
        l.material.SetColor("_Color", Color.green);
        l.GetComponent<Light>().color = Color.green;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(rightHold);
        EquipLeftSickle(leftHold);
        EquipCrossbow(holster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(rightHold);
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

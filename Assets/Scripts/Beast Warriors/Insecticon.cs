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

    public GameObject flash;

    public LineRenderer bolt;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

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
            ShootBolt();
        }
    }

    void EquipRightSickle(GameObject attachment)
    {
        rightSickle.transform.parent = attachment.transform;
        rightSickle.transform.localPosition = Vector3.zero;
        rightSickle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLftSickle(GameObject attachment)
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
        Vector3 direction = new Vector3(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject b = Instantiate(bullet);
            b.transform.position = lightBarrels[barrel].transform.position;
            b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            GameObject h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(lightBarrels[barrel].transform.position, hit.point, Color.blue, 3600);
            b = Instantiate(bullet);
            b.transform.position = lightBarrels[barrel + 3].transform.position;
            b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(lightBarrels[barrel + 3].transform.position, hit.point, Color.blue, 3600);
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.cyan, 3600);
        }
        barrel = barrel == (lightBarrels.Length - 4) ? 0 : barrel + 1;
    }

    void ShootBolt()
    {
        GameObject f = Instantiate(flash);
        f.transform.position = heavyBarrel.transform.position;
        f.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        f.GetComponent<Light>().color = boltColor;
        MainModule m = f.GetComponent<ParticleSystem>().main;
        m.startColor = new MinMaxGradient(boltColor);
        LineRenderer b = Instantiate(bolt);
        b.transform.position = heavyBarrel.transform.position;
        b.transform.eulerAngles = new Vector3(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        b.SetPosition(0, Vector3.zero);
        b.startColor = boltColor;
        b.endColor = boltColor;
        b.material.SetColor("_Color", boltColor);
        b.GetComponent<Light>().color = boltColor;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(rightHold);
        EquipLftSickle(leftHold);
        EquipCrossbow(holster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLftSickle(left);
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

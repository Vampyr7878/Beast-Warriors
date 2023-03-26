using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Razorbeast : BeastWarrior
{
    public GameObject gun;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightGun;

    public GameObject leftGun;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject slug;

    public float fireRate;

    public float bulletInaccuracy;

    public int slugCount;

    private float foldAngle;

    private float deployAngle;

    private float time;

    private int barrel;

    new void Awake()
    {
        foldAngle = -40;
        deployAngle = -80;
        base.Awake();
    }

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
            ShootShotgun();
        }
    }

    void EquipGun(GameObject attachment)
    {
        gun.transform.parent = attachment.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;
        barrel = 0;
    }

    void DeployGuns(bool enable)
    {
        if (enable)
        {
            rightGun.transform.localEulerAngles = new Vector3(deployAngle, rightGun.transform.localEulerAngles.y, rightGun.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(deployAngle, leftGun.transform.localEulerAngles.y, leftGun.transform.localEulerAngles.z);
        }
        else
        {
            rightGun.transform.localEulerAngles = new Vector3(foldAngle, rightGun.transform.localEulerAngles.y, rightGun.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(foldAngle, leftGun.transform.localEulerAngles.y, leftGun.transform.localEulerAngles.z);
        }
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
            Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.cyan, 3600);
        }
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
    }

    void ShootShotgun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        GameObject s = Instantiate(slug);
        s.transform.position = heavyBarrels[barrel].transform.position;
        s.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        Vector3 direction;
        GameObject b;
        GameObject h;
        for (int i = 0; i < slugCount; i++)
        {
            direction = new Vector3(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
            if (Physics.Raycast(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                b = Instantiate(bullet);
                b.transform.position = heavyBarrels[barrel].transform.position;
                b.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                b.GetComponent<AudioSource>().enabled = false;
                h = b.transform.GetChild(1).gameObject;
                h.transform.position = hit.point;
                Debug.DrawLine(heavyBarrels[barrel].transform.position, hit.point, Color.blue, 3600);
                Debug.DrawRay(cameraAimHelper.position, cameraAimHelper.TransformDirection(direction) * hit.distance, Color.cyan, 3600);
            }
        }
        barrel = barrel == (heavyBarrels.Length - 1) ? 0 : barrel + 1;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(false);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(false);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(true);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(hold);
        DeployGuns(false);
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

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

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
    }

    void ShootShotgun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        GameObject s = Instantiate(slug);
        s.transform.position = heavyBarrels[barrel].transform.position;
        s.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        Vector3 direction;
        for (int i = 0; i < slugCount; i++)
        {
            direction = new Vector3(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
            RaycastBullet(bullet, direction, layerMask, heavyBarrels[barrel], false);
        }
        barrel = barrel == (heavyBarrels.Length - 1) ? 0 : barrel + 1;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        Deploy(rightGun, foldAngle, 0f, 0f);
        Deploy(leftGun, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        Deploy(rightGun, foldAngle, 0f, 0f);
        Deploy(leftGun, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        Deploy(rightGun, deployAngle, 0f, 0f);
        Deploy(leftGun, deployAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, hold);
        Deploy(rightGun, foldAngle, 0f, 0f);
        Deploy(leftGun, foldAngle, 0f, 0f);
        barrel = 0;
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

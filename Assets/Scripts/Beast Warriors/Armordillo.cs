using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Armordillo : BeastWarrior
{
    public GameObject mace;

    public GameObject rifle;

    public GameObject maceHolster;

    public GameObject gunHolster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public LineRenderer laser;

    public Color laserColor;

    public float fireRate;

    public float bulletInaccuracy;

    public float laserInaccuracy;

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

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, heavyBarrel, laserColor);
        heavyShoot = false;
    }

    void EquipMace(GameObject attachment)
    {
        mace.transform.parent = attachment.transform;
        mace.transform.localPosition = Vector3.zero;
        mace.transform.localEulerAngles = Vector3.zero;
    }

    void EquipRifle(GameObject attachment)
    {
        rifle.transform.parent = attachment.transform;
        rifle.transform.localPosition = Vector3.zero;
        rifle.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipRifle(gunHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(hold);
        EquipRifle(gunHolster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipRifle(gunHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipRifle(hold);
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

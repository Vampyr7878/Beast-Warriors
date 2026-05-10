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

    public int bulletCost;

    public float slugCooldown;

    public int slugCost;

    public int slugCount;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    private float time;

    new void Awake()
    {
        foldVectors = new Vector3[]
        {
            new(0f, 180f, 0f),
            new(0f, 180f, 0f)
        };
        deployVectors = new Vector3[]
        {
            new(-45f, 180f, 0f),
            new(-45f, 180f, 0f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot && character.energy >= bulletCost)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.None, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootShotgun(WeaponArm.Right, bullet, slug, heavyBarrels, bulletInaccuracy, slugCooldown, slugCost, slugCount);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightGun, deployVectors[0]);
        Deploy(leftGun, deployVectors[1]);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, hold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
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
                if (canShoot && character.energy >= slugCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

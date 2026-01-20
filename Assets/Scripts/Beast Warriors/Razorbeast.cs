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
                ShootMachineGun(WeaponArm.None, bullet, lightBarrels, bulletInaccuracy);
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            heavyShoot = ShootShotgun(WeaponArm.Right, bullet, slug, heavyBarrels, bulletInaccuracy, slugCount);
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
        Deploy(rightGun, foldAngle, 0f, 0f);
        Deploy(leftGun, foldAngle, 0f, 0f);
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
        Deploy(rightGun, foldAngle, 0f, 0f);
        Deploy(leftGun, foldAngle, 0f, 0f);
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
        Deploy(rightGun, deployAngle, 0f, 0f);
        Deploy(leftGun, deployAngle, 0f, 0f);
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

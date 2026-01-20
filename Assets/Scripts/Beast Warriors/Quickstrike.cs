using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Quickstrike : BeastWarrior
{
    public GameObject cobraHead;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public LineRenderer laser;

    public Color laserColor;

    public float fireRate;

    public float bulletInaccuracy;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    new void Awake()
    {
        foldAngle = 90;
        deployAngle = 225;
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
            heavyShoot = ShootLaser(WeaponArm.Right, laser, heavyBarrel, laserColor, laserInaccuracy);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(cobraHead, 0f, foldAngle, 90f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(cobraHead, 0f, foldAngle, 90f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(cobraHead, 0f, foldAngle, 90f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Diagonal);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
        Deploy(cobraHead, 0f, deployAngle, 90f);
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

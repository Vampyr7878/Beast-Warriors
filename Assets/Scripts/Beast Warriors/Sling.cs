using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Sling : BeastWarrior
{
    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public LineRenderer laser;

    public Color laserColor;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    private float time;

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
            heavyShoot = ShootLaser(WeaponArm.Right, laser, heavyBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
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
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
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
                if (canShoot && character.energy >= laserCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

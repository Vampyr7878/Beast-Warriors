using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Prowl : BeastWarrior
{
    public GameObject tail;
    
    public GameObject fork;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject bullet;

    public Color laserColor;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.None, laser, lightBarrels, laserColor, laserInaccuracy, laserCooldown, laserCost, 2);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootMachineGun(WeaponArm.Both, bullet, heavyBarrels, bulletInaccuracy, fireRate, bulletCost);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(false);
        fork.SetActive(true);
        Equip(fork, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedStrong(context);
        barrel = 0;
        right = true;
        left = false;
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= laserCost)
                {
                    lightShoot = context.performed;
                    canShoot = !lightShoot;
                }
                break;
            case 4:
                if (canShoot && character.energy >= bulletCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

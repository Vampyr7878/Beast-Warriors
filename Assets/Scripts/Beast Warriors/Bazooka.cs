using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Bazooka : BeastWarrior
{
    public GameObject axe;

    public GameObject holster;

    public GameObject axeHold;

    public GameObject gunHold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public Material boltMaterial;

    public Color boltColor;

    public float boltCooldown;

    public int boltCost;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Left, flash, bolt, lightBarrel, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootMachineGun(WeaponArm.Right, bullet, heavyBarrel, bulletInaccuracy, fireRate, bulletCost);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(axe, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(axe, axeHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(axe, holster);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(axe, gunHold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= boltCost)
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

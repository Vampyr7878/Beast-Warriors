using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Torca : BeastWarrior
{
    public GameObject gun;

    public GameObject holster;

    public GameObject hold;
    
    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Material boltMaterial;

    public Color boltColor;

    public Color laserColor;

    public float boltCooldown;

    public int boltCost;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Both, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
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
        Equip(gun, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
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
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(gun, holster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
        barrel = 0;
        right = true;
        left = false;
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
                if (canShoot && character.energy >= laserCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

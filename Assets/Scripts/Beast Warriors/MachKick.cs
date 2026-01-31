using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MachKick : BeastWarrior
{
    public GameObject axe;

    public GameObject holster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject bolt;

    public Color laserColor;

    public Material boltMaterial;

    public Color boltColor;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float boltCooldown;

    public int boltCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.Right, laser, lightBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, flash, bolt, heavyBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
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
        Equip(axe, hold);
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
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(axe, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        barrel = 0;
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
                if (canShoot && character.energy >= boltCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

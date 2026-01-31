using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Terragator : BeastWarrior
{
    public GameObject cannon;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public GameObject slug;

    public Material boltMaterial;

    public Color boltColor;

    public float boltCooldown;

    public int boltCost;

    public float bulletInaccuracy;

    public float slugCooldown;

    public int slugCost;

    public int slugCount;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Left, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
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
        Equip(cannon, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedWeak(context);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, hold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
        barrel = 0;
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
                if (canShoot && character.energy >= slugCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class RamHorn : BeastWarrior
{
    public GameObject fold;

    public GameObject claw;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

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
            lightShoot = ShootLaser(WeaponArm.None, laser, lightBarrels, laserColor, laserInaccuracy, laserCooldown, laserCost, 2);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Both, flash, bolt, heavyBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(false);
        claw.SetActive(true);
        Equip(claw, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
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
                if (canShoot && character.energy >= boltCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

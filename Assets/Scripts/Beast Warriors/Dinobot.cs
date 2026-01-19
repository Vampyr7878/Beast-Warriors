using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Dinobot : BeastWarrior
{
    public GameObject sword;

    public GameObject slash;

    public GameObject swordHolster;

    public GameObject slashHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Material boltMaterial;

    public Color boltColor;

    public Color laserColor;

    public float laserInaccuracy;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Left, flash, bolt, lightBarrel, boltMaterial, boltColor);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootLaser(WeaponArm.None, laser, heavyBarrels, laserColor, laserInaccuracy, 2);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(slash, slashHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, rightHold);
        Equip(slash, leftHold);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Straight);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, slashHolster);
        Equip(slash, leftHold);
        character.OverrideArm(WeaponArm.Left);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(slash, slashHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

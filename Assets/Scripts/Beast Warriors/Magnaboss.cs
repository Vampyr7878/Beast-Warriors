using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Magnaboss : BeastWarrior
{
    public GameObject sword;

    public GameObject holster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject blast;

    public GameObject missle;

    public Material boltMaterial;

    public Color boltColor;

    public Material missleMaterial;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrel, boltMaterial, boltColor);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrels, missleMaterial, Color.clear);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, hold);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.None);
        barrel = 0;
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

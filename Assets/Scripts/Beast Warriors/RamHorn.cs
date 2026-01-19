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

    public float laserInaccuracy;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.None, laser, lightBarrels, laserColor, laserInaccuracy, 2);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Both, flash, bolt, heavyBarrels, boltMaterial, boltColor);
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
        barrel = 0;
        right = true;
        left = false;
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

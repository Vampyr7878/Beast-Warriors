using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public class Inferno : BeastWarrior
{
    public GameObject flamethrower;

    public GameObject launcher;

    public GameObject flamethrowerHolster;

    public GameObject launcherHolster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public GameObject thrower;

    public Material boltMaterial;

    public Color boltColor;

    public Color[] flameColors;

    protected new void Awake()
    {
        base.Awake();
        InitThrower(thrower, flameColors);
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Right, flash, bolt, lightBarrel, boltMaterial, boltColor);
        }
        thrower.SetActive(heavyShoot);
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flamethrower, flamethrowerHolster);
        Equip(launcher, launcherHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        Equip(flamethrower, hold);
        Equip(launcher, launcherHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(flamethrower, flamethrowerHolster);
        Equip(launcher, hold);
        character.OverrideArm(WeaponArm.Right);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(flamethrower, hold);
        Equip(launcher, launcherHolster);
        character.OverrideArm(WeaponArm.Right);
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

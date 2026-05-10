using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Dirgegun : BeastWarrior
{
    public GameObject launcher;

    public GameObject holster;

    public GameObject hold;

    public GameObject backLauncher;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public GameObject blast;

    public GameObject missle;

    public Material boltMaterial;

    public Color boltColor;

    public Material missleMaterial;

    public float boltCooldown;

    public int boltCost;

    public float missleCooldown;

    public int missleCost;

    private Vector3 foldVector;

    private Vector3 deployVector;

    new void Awake()
    {
        foldVector = new(0f, 180f, 0f);
        deployVector = new(-235f, 180f, 0f);
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrel, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Right, blast, missle, heavyBarrel, missleMaterial, Color.clear, missleCost, missleCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(backLauncher, foldVector);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(backLauncher, foldVector);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(backLauncher, deployVector);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, hold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
        Deploy(backLauncher, foldVector);
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
                if (canShoot && character.energy >= missleCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

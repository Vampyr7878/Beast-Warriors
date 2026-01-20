using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Jetstorm : BeastWarrior
{
    public GameObject rightGun;

    public GameObject rightGunBarrel;

    public GameObject leftGun;

    public GameObject leftGunBarrel;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Material boltMaterial;

    public Color boltColor;

    public Color laserColor;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 180;
        deployAngle = 90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrels, boltMaterial, boltColor);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootLaser(WeaponArm.None, laser, heavyBarrel, laserColor, laserInaccuracy);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightGun, 0f, deployAngle, 0f);
        Deploy(rightGunBarrel, foldAngle, 0f, 0f);
        Deploy(leftGun, 0f, -deployAngle, 0f);
        Deploy(leftGunBarrel, -foldAngle, 0f, 0f);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
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

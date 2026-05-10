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

    public float boltCooldown;

    public int boltCost;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    new void Awake()
    {
        foldVectors = new Vector3[]
        {
            new(0f, 180f, 0f),
            new(0f, 0f, 0f),
            new(0f, -180f, 0f),
            new(0f, 0f, 0f)
        };
        deployVectors = new Vector3[]
        {
            new(0f, 90f, 0f),
            new(0f, 0f, -90f),
            new(0f, -90f, 0f),
            new(0f, 0f, 90f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootLaser(WeaponArm.None, laser, heavyBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
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
        Deploy(rightGun, foldVectors[0]);
        Deploy(rightGunBarrel, foldVectors[1]);
        Deploy(leftGun, foldVectors[2]);
        Deploy(leftGunBarrel, foldVectors[3]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(rightGunBarrel, foldVectors[1]);
        Deploy(leftGun, foldVectors[2]);
        Deploy(leftGunBarrel, foldVectors[3]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightGun, deployVectors[0]);
        Deploy(rightGunBarrel, deployVectors[1]);
        Deploy(leftGun, deployVectors[2]);
        Deploy(leftGunBarrel, deployVectors[3]);
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
        Deploy(rightGun, foldVectors[0]);
        Deploy(rightGunBarrel, foldVectors[1]);
        Deploy(leftGun, foldVectors[2]);
        Deploy(leftGunBarrel, foldVectors[3]);
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

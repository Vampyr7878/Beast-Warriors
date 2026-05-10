using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Longrack : BeastWarrior
{
    public GameObject knife;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightGun;

    public GameObject leftGun;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject blast;

    public GameObject missle;

    public Color laserColor;

    public Material missleMaterial;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float missleCooldown;

    public int missleCost;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    new void Awake()
    {
        foldVectors = new Vector3[]
        {
            new(0f, 180f, 0f),
            new(0f, 180f, 0f)
        };
        deployVectors = new Vector3[]
        {
            new(-90f, 180f, 0f),
            new(-90f, 180f, 0f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.Left, laser, lightBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrels, missleMaterial, Color.clear, missleCost, missleCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(knife, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedWeak(context);
        Deploy(rightGun, foldVectors[0]);
        Deploy(leftGun, foldVectors[1]);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(rightGun, deployVectors[0]);
        Deploy(leftGun, deployVectors[1]);
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
                if (canShoot && character.energy >= missleCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

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

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = 90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.Left, laser, lightBarrel, laserColor, laserInaccuracy);
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
        Equip(knife, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
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
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
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
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
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
        Deploy(rightGun, deployAngle, 180f, 0f);
        Deploy(leftGun, deployAngle, 180f, 0f);
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

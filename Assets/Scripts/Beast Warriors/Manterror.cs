using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Manterror : BeastWarrior
{
    public GameObject rightLauncher;

    public GameObject leftLauncher;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject disc;

    public Color laserColor;

    public Material discMaterial;

    public Color discColor;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 9;
        deployAngle = 170;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.None, laser, lightBarrels, laserColor, laserInaccuracy, 2);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Both, flash, disc, heavyBarrels, discMaterial, discColor, 90f);
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
        Deploy(rightLauncher, 0f, foldAngle, 0f);
        Deploy(leftLauncher, 0f, -foldAngle, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightLauncher, 0f, deployAngle, 0f);
        Deploy(leftLauncher, 0f, -deployAngle, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightLauncher, 0f, foldAngle, 0f);
        Deploy(leftLauncher, 0f, -foldAngle, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedStrong(context);
        Deploy(rightLauncher, 0f, deployAngle, 0f);
        Deploy(leftLauncher, 0f, -deployAngle, 0f);
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

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

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float discCooldown;

    public int discCost;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    new void Awake()
    {
        foldVectors = new Vector3[]
        {
            new(0f, 90f, -90f),
            new(0f, -90f, 90f)
        };
        deployVectors = new Vector3[]
        {
            new(0f, -100f, -90f),
            new(0f, 100f, 90f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.None, laser, lightBarrels, laserColor, laserInaccuracy, laserCooldown, laserCost, 2);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Both, flash, disc, heavyBarrels, discMaterial, discColor, discCost, discCooldown, 90f);
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
        Deploy(rightLauncher, foldVectors[0]);
        Deploy(leftLauncher, foldVectors[1]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightLauncher, deployVectors[0]);
        Deploy(leftLauncher, deployVectors[1]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightLauncher, foldVectors[0]);
        Deploy(leftLauncher, foldVectors[1]);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedStrong(context);
        Deploy(rightLauncher, deployVectors[0]);
        Deploy(leftLauncher, deployVectors[1]);
        barrel = 0;
        right = true;
        left = false;
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
                if (canShoot && character.energy >= discCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

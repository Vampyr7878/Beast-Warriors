using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PolarClaw : BeastWarrior
{
    public GameObject claw;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject flash;

    public GameObject ball;

    public Color flashColor;

    public Color ballColor;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    public float ballCooldown;

    public int ballCost;

    private float foldAngle;

    private float deployAngle;

    private float time;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = -130;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot && character.energy >= bulletCost)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Both, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.None, flash, ball, heavyBarrels, ballColor, ballColor, ballCooldown, ballCost);
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
        Deploy(claw, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(claw, deployAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
        Deploy(claw, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(claw, foldAngle, 0f, 0f);
        barrel = 0;
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                barrel = 0;
                right = true;
                left = false;
                break;
            case 4:
                if (canShoot && character.energy >= ballCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

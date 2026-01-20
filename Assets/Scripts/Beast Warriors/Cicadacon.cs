using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cicadacon : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject cannon;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject flash;

    public GameObject ball;

    public Color flashColor;

    public Color ballColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

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
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.None, bullet, lightBarrels, bulletInaccuracy, 2);
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.None, flash, ball, heavyBarrel, flashColor, ballColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Straight);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(cannon, deployAngle, 0f, 0f);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                barrel = 0;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class OptimusPrimal : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject flail;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject flailHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightCannon;

    public GameObject leftCannon;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject blast;

    public GameObject missle;

    public Material missleMaterial;

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
                ShootMachineGun(WeaponArm.Left, bullet, lightBarrels, bulletInaccuracy);
                time = 0;
            }
            time += Time.deltaTime;
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
        Equip(flail, rightHold);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.Left);
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightCannon, deployAngle, 0f, 0f);
        Deploy(leftCannon, deployAngle, 0f, 0f);
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
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

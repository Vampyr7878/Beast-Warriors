using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Silverbolt : BeastWarrior
{
    public GameObject rightBlade;

    public GameObject leftBlade;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightCannons;

    public GameObject leftCannons;

    public GameObject[] heavyBarrels;

    public GameObject thrown;

    public GameObject blast;

    public GameObject missle;

    public Material missleMaterial;

    public int angle;

    public int force;

    private GameObject[] holds;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        holds = new GameObject[2] { rightHold, leftHold };
        foldAngle = 180;
        deployAngle = 90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = Throw(WeaponArm.Both, thrown, rightBlade, holds, 180f, 90f, true);
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
        Equip(rightBlade, rightHolster);
        Equip(leftBlade, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHold);
        Equip(leftBlade, leftHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Throw);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHold);
        Equip(leftBlade, leftHold);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
        right = true;
        left = false;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHolster);
        Equip(leftBlade, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(rightCannons, 0f, deployAngle, 0f);
        Deploy(leftCannons, 0f, -deployAngle, 0f);
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

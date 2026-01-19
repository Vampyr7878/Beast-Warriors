using System.Threading;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Galvatron : BeastWarrior
{
    public GameObject rightAxe;

    public GameObject leftAxe;

    public GameObject claw;

    public GameObject rightBlaster;

    public GameObject leftBlaster;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject clawHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public GameObject ball;

    public Material boltMaterial;

    public Color boltColor;

    public Color ballColor;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 90;
        deployAngle = 270;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Both, flash, bolt, lightBarrels, boltMaterial, boltColor);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.Right, flash, ball, heavyBarrel, ballColor, ballColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHold);
        Equip(leftAxe, leftHold);
        Equip(claw, clawHolster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHolster);
        Equip(leftAxe, leftHolster);
        Equip(claw, rightHold);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHold);
        Equip(leftAxe, leftHold);
        Equip(claw, clawHolster);
        character.OverrideArm(WeaponArm.Both);
        Deploy(rightBlaster, 0f, deployAngle, 90f);
        Deploy(leftBlaster, 0f, -deployAngle, -90f);
        barrel = 0;
        right = true;
        left = false;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHolster);
        Equip(leftAxe, leftHolster);
        Equip(claw, rightHold);
        character.OverrideArm(WeaponArm.Right);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
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
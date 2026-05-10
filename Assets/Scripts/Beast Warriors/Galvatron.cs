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

    public float boltCooldown;

    public int boltCost;

    public float ballCooldown;

    public int ballCost;

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
            new(0f, 270f, -90f),
            new(0f, -270f, 90f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.Both, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.Right, flash, ball, heavyBarrel, ballColor, ballColor, ballCooldown, ballCost);
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
        base.OnMeleeWeak(context);
        Deploy(rightBlaster, foldVectors[0]);
        Deploy(leftBlaster, foldVectors[1]);
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
        base.OnMeleeStrong(context);
        Deploy(rightBlaster, foldVectors[0]);
        Deploy(leftBlaster, foldVectors[1]);
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
        base.OnRangedWeak(context);
        Deploy(rightBlaster, deployVectors[0]);
        Deploy(leftBlaster, deployVectors[1]);
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
        base.OnRangedStrong(context);
        Deploy(rightBlaster, foldVectors[0]);
        Deploy(leftBlaster, foldVectors[1]);
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
                if (canShoot && character.energy >= ballCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}
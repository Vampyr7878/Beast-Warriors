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

    public float throwCooldown;

    public int throwCost;

    public int angle;

    public int force;

    public float missleCooldown;

    public int missleCost;

    private GameObject[] holds;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    new void Awake()
    {
        holds = new GameObject[]
        { 
            rightHold,
            leftHold
        };
        foldVectors = new Vector3[]
        {
            new(0f, 180f, 0f),
            new(0f, 180f, 0f)
        };
        deployVectors = new Vector3[]
        {
            new(0f, 90f, 0f),
            new(0f, -90f, 0f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = Throw(WeaponArm.Both, thrown, rightBlade, holds, 180f, 90f, throwCooldown, throwCost, true);
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
        Equip(rightBlade, rightHolster);
        Equip(leftBlade, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightCannons, foldVectors[0]);
        Deploy(leftCannons, foldVectors[1]);
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
        Deploy(rightCannons, foldVectors[0]);
        Deploy(leftCannons, foldVectors[1]);
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
        Deploy(rightCannons, foldVectors[0]);
        Deploy(leftCannons, foldVectors[1]);
        right = true;
        left = false;
        barrel = 0;
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
        Deploy(rightCannons, deployVectors[0]);
        Deploy(leftCannons, deployVectors[1]);
        barrel = 0;
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= throwCost)
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

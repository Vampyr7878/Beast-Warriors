using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Ironhide : BeastWarrior
{
    public GameObject rightClub;

    public GameObject leftClub;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightBlade;

    public GameObject leftBlade;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public Material boltMaterial;

    public Color boltColor;

    public float boltCooldown;

    public int boltCost;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

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
            new(0f, -90f, -90f),
            new(0f, 90f, 90f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootMachineGun(WeaponArm.Both, bullet, heavyBarrels, bulletInaccuracy, fireRate, bulletCost);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightBlade, deployVectors[0]);
        Deploy(leftBlade, deployVectors[1]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHold);
        Equip(leftClub, leftHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightBlade, foldVectors[0]);
        Deploy(leftBlade, foldVectors[1]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(rightBlade, foldVectors[0]);
        Deploy(leftBlade, foldVectors[1]);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedStrong(context);
        Deploy(rightBlade, foldVectors[0]);
        Deploy(leftBlade, foldVectors[1]);
        barrel = 0;
        right = true;
        left = false;
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
                if (canShoot && character.energy >= bulletCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

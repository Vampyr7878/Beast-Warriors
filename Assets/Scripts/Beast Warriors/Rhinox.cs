using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Rhinox : BeastWarrior
{
    public GameObject sword;

    public GameObject gun;

    public GameObject rightBlaster;

    public GameObject leftBlaster;

    public GameObject swordHolster;

    public GameObject gunHolster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bolt;

    public Material boltMaterial;

    public Color boltColor;

    public GameObject bullet;

    public GameObject flash;

    public float boltCooldown;

    public int boltCost;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    private float time;

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
        if (lightShoot && character.energy >= bulletCost)
        {
            lightShoot = ShootBolt(WeaponArm.Both, flash, bolt, lightBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
        else if (heavyShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Right, bullet, heavyBarrels, bulletInaccuracy, 0f, bulletCost, 2);
                time = 0;
            }
            time += Time.deltaTime;
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(gun, gunHolster);
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
        Equip(sword, hold);
        Equip(gun, gunHolster);
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
        Equip(sword, swordHolster);
        Equip(gun, gunHolster);
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
        Equip(sword, swordHolster);
        Equip(gun, hold);
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
                heavyShoot = context.performed;
                time = fireRate;
                barrel = 0;
                break;
        }
    }
}

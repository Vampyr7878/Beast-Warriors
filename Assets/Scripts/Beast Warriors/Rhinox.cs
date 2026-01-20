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

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public Material boltMaterial;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    new void Awake()
    {
        foldAngle = 180;
        deployAngle = 0;
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
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Right, bullet, heavyBarrels, bulletInaccuracy);
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
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
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
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
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
        Deploy(rightBlaster, 0f, deployAngle, 0f);
        Deploy(leftBlaster, 0f, deployAngle, 0f);
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
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
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
                time = fireRate;
                barrel = 0;
                break;
        }
    }
}

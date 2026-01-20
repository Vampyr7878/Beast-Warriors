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

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    new void Awake()
    {
        foldAngle = 90;
        deployAngle = -90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, flash, bolt, lightBarrels, boltMaterial, boltColor);
        }
        if (heavyShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Both, bullet, heavyBarrels, bulletInaccuracy);
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
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightBlade, 0f, deployAngle, 80f);
        Deploy(leftBlade, 0f, -deployAngle, -80f);
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
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
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
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
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
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
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
                right = true;
                left = false;
                break;
        }
    }
}

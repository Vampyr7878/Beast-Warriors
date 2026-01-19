using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class SeaClamp : BeastWarrior
{
    public GameObject launcher;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightClaws;

    public GameObject leftClaws;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject flash;

    public GameObject bolt;

    public Material boltMaterial;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = 160;
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
            heavyShoot = ShootBolt(WeaponArm.Right, flash, bolt, heavyBarrels, boltMaterial, boltColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightClaws, 0f, -deployAngle, 80f);
        Deploy(leftClaws, 0f, deployAngle + 180, -80f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm(WeaponArm.None);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, hold);
        character.OverrideArm(WeaponArm.Right);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
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

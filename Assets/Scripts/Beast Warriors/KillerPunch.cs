using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class KillerPunch : BeastWarrior
{
    public GameObject sword;

    public GameObject holster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject flash;

    public GameObject bolt;

    public Material boltMaterial;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float time;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Left, bullet, lightBarrel, bulletInaccuracy);
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Right, flash, bolt, heavyBarrel, boltMaterial, boltColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, hold);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.Left);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm(WeaponArm.Right);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

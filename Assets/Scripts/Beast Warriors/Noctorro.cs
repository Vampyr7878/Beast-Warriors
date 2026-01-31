using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Noctorro : BeastWarrior
{
    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject flash;

    public GameObject bolt;

    public Material boltMaterial;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    public float boltCooldown;

    public int boltCost;

    private float time;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot && character.energy >= bulletCost)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Both, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost, 2);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, flash, bolt, heavyBarrels, boltMaterial, boltColor, boltCost, boltCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
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
                right = true;
                left = false;
                break;
            case 4:
                if (canShoot && character.energy >= boltCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

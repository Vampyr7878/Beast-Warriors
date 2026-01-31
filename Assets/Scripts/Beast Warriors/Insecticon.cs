using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Insecticon : BeastWarrior
{
    public GameObject rightSickle;

    public GameObject leftSickle;

    public GameObject crossbow;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject holster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

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
                ShootMachineGun(WeaponArm.None, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost, 2);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.Right, flash, bolt, heavyBarrel, boltMaterial, boltColor, boltCost, boltCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSickle, rightHolster);
        Equip(leftSickle, leftHolster);
        Equip(crossbow, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSickle, rightHold);
        Equip(leftSickle, leftHold);
        Equip(crossbow, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Straight);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSickle, rightHolster);
        Equip(leftSickle, leftHolster);
        Equip(crossbow, holster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSickle, rightHolster);
        Equip(leftSickle, leftHolster);
        Equip(crossbow, rightHold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
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
                if (canShoot && character.energy >= boltCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

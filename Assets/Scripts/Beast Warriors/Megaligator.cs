using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Megaligator : BeastWarrior
{
    public GameObject gun;

    public GameObject tail;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject flash;

    public GameObject ball;

    public Color flashColor;

    public Color ballColor;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    public float ballCooldown;

    public int ballCost;

    private float time;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot && character.energy >= bulletCost)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Both, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost);
                time = 0;
            }
            time += Time.deltaTime;
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
        tail.SetActive(false);
        gun.SetActive(true);
        Equip(gun, holster);
        Equip(tail, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        gun.SetActive(false);
        Equip(gun, holster);
        Equip(tail, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(false);
        gun.SetActive(true);
        Equip(gun, holster);
        Equip(tail, holster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(false);
        gun.SetActive(true);
        Equip(gun, hold);
        Equip(tail, holster);
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
                right = true;
                left = false;
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

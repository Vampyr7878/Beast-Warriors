using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Rattrap : BeastWarrior
{
    public GameObject rifleFront;

    public GameObject rifleBack;

    public GameObject bomb;

    public GameObject frontHolster;

    public GameObject backHolster;

    public GameObject bombHolster;

    public GameObject front;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject bullet;

    public GameObject thrown;

    public float fireRate;

    public float bulletInaccuracy;

    public int angle;

    public int force;

    private float time;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Right, bullet, lightBarrel, bulletInaccuracy);
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            heavyShoot = Throw(WeaponArm.Right, thrown, bomb, hold, 0f, -180f);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, backHolster);
        Equip(bomb, bombHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, hold);
        Equip(bomb, bombHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, front);
        Equip(rifleBack, hold);
        Equip(bomb, bombHolster);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Throw);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, backHolster);
        Equip(bomb, hold);
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
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

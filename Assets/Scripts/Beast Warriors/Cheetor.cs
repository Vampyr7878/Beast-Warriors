using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cheetor : BeastWarrior
{
    public GameObject rifle;

    public GameObject cannon;

    public GameObject rifleHolster;

    public GameObject cannonHolster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject ball;

    public Color laserColor;

    public Color flashColor;

    public Color ballColor;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float ballCooldown;

    public int ballCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.Right, laser, lightBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
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
        Equip(rifle, rifleHolster);
        Equip(cannon, cannonHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, rifleHolster);
        Equip(cannon, cannonHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, hold);
        Equip(cannon, cannonHolster);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, rifleHolster);
        Equip(cannon, hold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= laserCost)
                {
                    lightShoot = context.performed;
                    canShoot = !lightShoot;
                }
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

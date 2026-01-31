using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Buzzclaw : BeastWarrior
{
    public GameObject clawPack;

    public GameObject clawPackWings;

    public GameObject shield;

    public GameObject shieldHolster;

    public GameObject shieldHold;

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
            lightShoot = ShootLaser(WeaponArm.Left, laser, lightBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
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
        clawPack.SetActive(false);
        clawPackWings.SetActive(true);
        Equip(shield, shieldHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        clawPack.SetActive(true);
        clawPackWings.SetActive(false);
        Equip(shield, shieldHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        clawPack.SetActive(false);
        clawPackWings.SetActive(true);
        Equip(shield, shieldHold);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedWeak(context);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        clawPack.SetActive(true);
        clawPackWings.SetActive(false);
        Equip(shield, shieldHolster);
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

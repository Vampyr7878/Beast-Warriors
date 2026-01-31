using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Convobat : BeastWarrior
{
    public GameObject rightScimitar;

    public GameObject leftScimitar;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject sonic;

    public GameObject ripple;

    public Color laserColor;

    public Color rippleColor;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    public float rippleCooldown;

    public int rippleCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootLaser(WeaponArm.Both, laser, lightBarrels, laserColor, laserInaccuracy, laserCooldown, laserCost);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.None, sonic, ripple, heavyBarrels, rippleColor, rippleColor, rippleCooldown, rippleCost);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightScimitar, rightHolster);
        Equip(leftScimitar, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightScimitar, rightHold);
        Equip(leftScimitar, leftHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rightScimitar, rightHolster);
        Equip(leftScimitar, leftHolster);
        character.OverrideArm(WeaponArm.Both);
        base.OnRangedWeak(context);
        barrel = 0;
        right = true;
        left = false;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightScimitar, rightHolster);
        Equip(leftScimitar, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        barrel = 0;
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
                if (canShoot && character.energy >= rippleCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

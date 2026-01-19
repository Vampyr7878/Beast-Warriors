using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Wolfang : BeastWarrior
{
    public GameObject shield;

    public GameObject gun;

    public GameObject shieldHlster;

    public GameObject gunHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject blast;

    public GameObject missle;

    public GameObject flash;

    public GameObject ball;

    public Material missleMaterial;

    public Color ballColor;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, blast, missle, lightBarrel, missleMaterial, Color.clear);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.Right, flash, ball, heavyBarrel, ballColor, ballColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(shield, shieldHlster);
        Equip(gun, gunHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(shield, leftHold);
        Equip(gun, gunHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(shield, shieldHlster);
        Equip(gun, gunHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(shield, shieldHlster);
        Equip(gun, rightHold);
        character.OverrideArm(WeaponArm.Right);
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
                break;
        }
    }
}

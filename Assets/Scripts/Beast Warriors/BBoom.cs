using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BBoom : BeastWarrior
{
    public GameObject rifle;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

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
            lightShoot = ShootBolt(WeaponArm.None, blast, missle, lightBarrels, missleMaterial, Color.clear);
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
        Equip(rifle, holster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, holster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, holster);
        character.OverrideArm(WeaponArm.None);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(rifle, hold);
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

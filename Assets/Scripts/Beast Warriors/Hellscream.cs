using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Hellscream : BeastWarrior
{
    public GameObject head;

    public GameObject chestCannon;

    public GameObject holster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject blast;

    public GameObject missle;

    public GameObject flash;

    public GameObject ball;

    public Color ballColor;

    public Material missleMaterial;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = -90;
        deployAngle = 0;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, blast, missle, lightBarrel, missleMaterial, Color.clear);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.Left, flash, ball, heavyBarrel, ballColor, ballColor);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(head, holster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        Deploy(chestCannon, deployAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(head, holster);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedStrong(context);
        Deploy(chestCannon, foldAngle, 0f, 0f);
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

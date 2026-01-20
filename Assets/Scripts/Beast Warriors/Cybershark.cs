using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cybershark : BeastWarrior
{
    public GameObject head;

    public GameObject tail;

    public GameObject chestCannon;

    public GameObject headHolster;

    public GameObject tailHolster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject ball;

    public GameObject blast;

    public GameObject missle;

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
            lightShoot = ShootBall(WeaponArm.Right, flash, ball, lightBarrel, ballColor, ballColor);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrel, missleMaterial, Color.clear);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        Equip(tail, tailHolster);
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
        Equip(head, headHolster);
        Equip(tail, hold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(head, headHolster);
        Equip(tail, hold);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedWeak(context);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        Equip(tail, tailHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(chestCannon, deployAngle, 0f, 0f);
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

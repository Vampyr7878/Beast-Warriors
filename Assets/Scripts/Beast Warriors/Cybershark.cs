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

    public float ballCooldown;

    public int ballCost;

    public float missleCooldown;

    public int missleCost;

    private Vector3 foldVector;

    private Vector3 deployVector;

    new void Awake()
    {
        foldVector = new(0f, 180f, 0f);
        deployVector = new(90f, 180f, 0f);
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBall(WeaponArm.Right, flash, ball, lightBarrel, ballColor, ballColor, ballCooldown, ballCost);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrel, missleMaterial, Color.clear, missleCost, missleCooldown);
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
        Deploy(chestCannon, foldVector);
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
        Deploy(chestCannon, foldVector);
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
        Deploy(chestCannon, foldVector);
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
        Deploy(chestCannon, deployVector);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= ballCost)
                {
                    lightShoot = context.performed;
                    canShoot = !lightShoot;
                }
                break;
            case 4:
                if (canShoot && character.energy >= missleCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

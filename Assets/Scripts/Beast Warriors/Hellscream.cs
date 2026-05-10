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

    public float missleCooldown;

    public int missleCost;

    public float ballCooldown;

    public int ballCost;

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
            lightShoot = ShootBolt(WeaponArm.None, blast, missle, lightBarrel, missleMaterial, Color.clear, missleCost, missleCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBall(WeaponArm.Left, flash, ball, heavyBarrel, ballColor, ballColor, ballCooldown, ballCost);
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
        Deploy(chestCannon, foldVector);
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
        Deploy(chestCannon, foldVector);
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
        Deploy(chestCannon, deployVector);
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
        Deploy(chestCannon, foldVector);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                if (canShoot && character.energy >= missleCost)
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

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class OptimusPrimal : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject flail;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject flailHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightCannon;

    public GameObject leftCannon;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject blast;

    public GameObject missle;

    public Material missleMaterial;

    public float fireRate;

    public float bulletInaccuracy;

    public int bulletCost;

    public float missleCooldown;

    public int missleCost;

    private Vector3[] foldVectors;

    private Vector3[] deployVectors;

    private float time;

    new void Awake()
    {
        foldVectors = new Vector3[]
        {
            new(0f, 180f, 0f),
            new(0f, 180f, 0f)
        };
        deployVectors = new Vector3[]
        {
            new(90f, 180f, 0f),
            new(90f, 180f, 0f)
        };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot && character.energy >= bulletCost)
        {
            if (time >= fireRate)
            {
                ShootMachineGun(WeaponArm.Left, bullet, lightBarrels, bulletInaccuracy, 0f, bulletCost);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrels, missleMaterial, Color.clear, missleCost, missleCooldown);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, rightHold);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
        Deploy(rightCannon, foldVectors[0]);
        Deploy(leftCannon, foldVectors[1]);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
        Deploy(rightCannon, foldVectors[0]);
        Deploy(leftCannon, foldVectors[1]);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.Left);
        base.OnRangedWeak(context);
        Deploy(rightCannon, foldVectors[0]);
        Deploy(leftCannon, foldVectors[1]);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedStrong(context);
        Deploy(rightCannon, deployVectors[0]);
        Deploy(leftCannon, deployVectors[1]);
        barrel = 0;
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                barrel = 0;
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

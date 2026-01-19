using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class SilverboltII : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] heavyBarrels;

    public GameObject thrown;

    public GameObject blast;

    public GameObject missle;

    public Material missleMaterial;

    private GameObject[] holds;

    public int angle;

    public int force;

    new void Awake()
    {
        holds = new GameObject[2] { rightHold, leftHold };
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = Throw(WeaponArm.Both, thrown, rightSword, holds, 180f, 90f, angle, force, true);
        }
        if (heavyShoot)
        {
            heavyShoot = ShootBolt(WeaponArm.None, blast, missle, heavyBarrels, missleMaterial, Color.clear);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.None);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Throw);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.Both);
        right = true;
        left = false;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm(WeaponArm.None);
        barrel = 0;
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

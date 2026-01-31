using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Megatron : BeastWarrior
{
    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject blast;

    public GameObject missle;

    public LineRenderer laser;

    public Material missleMaterial;

    public Color laserColor;

    public float missleCooldown;

    public int missleCost;

    public float laserCooldown;

    public float laserInaccuracy;

    public int laserCost;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            lightShoot = ShootBolt(WeaponArm.None, blast, missle, lightBarrels, missleMaterial, Color.clear, missleCost, missleCooldown);
        }
        else if (heavyShoot)
        {
            heavyShoot = ShootLaser(WeaponArm.Right, laser, heavyBarrel, laserColor, laserInaccuracy, laserCooldown, laserCost);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeWeak(context);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnMeleeStrong(context);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("WeaponMode", (int)WeaponMode.None);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.None);
        base.OnRangedWeak(context);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("WeaponMode", (int)WeaponMode.Bend);
        animator.SetInteger("Weapon", weapon);
        character.OverrideArm(WeaponArm.Right);
        base.OnRangedStrong(context);
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
                if (canShoot && character.energy >= laserCost)
                {
                    heavyShoot = context.performed;
                    canShoot = !heavyShoot;
                }
                break;
        }
    }
}

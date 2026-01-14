using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MaxB : BeastWarrior
{
    public GameObject claw;

    public GameObject gun;

    public GameObject clawHolster;

    public GameObject gunHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLightMissle();
        }
        if (heavyShoot)
        {
            ShootHeavyMissle();
        }
    }

    void ShootLightMissle()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, lightBarrel, missleMaterial);
        lightShoot = false;
    }

    void ShootHeavyMissle()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrel, missleMaterial);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(claw, clawHolster);
        Equip(gun, gunHolster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(claw, leftHold);
        Equip(gun, gunHolster);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(claw, clawHolster);
        Equip(gun, rightHold);
        character.OverrideArm("Right");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(claw, leftHold);
        Equip(gun, gunHolster);
        character.OverrideArm("Left");
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

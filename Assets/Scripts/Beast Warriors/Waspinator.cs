using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Waspinator : BeastWarrior
{
    public GameObject launcher;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public LineRenderer laser;

    public GameObject explosion;

    public GameObject missle;

    public Color laserColor;

    public Material missleMaterial;

    public float laserInaccuracy;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLaser();
        }
        if (heavyShoot)
        {
            ShootMissle();
        }
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, lightBarrels[0], laserColor);
        RaycastLaser(laser, direction, layerMask, lightBarrels[1], laserColor);
        lightShoot = false;
    }

    void ShootMissle()
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
        Equip(launcher, holster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, hold);
        character.OverrideArm("Right");
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

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Longrack : BeastWarrior
{
    public GameObject knife;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightGun;

    public GameObject leftGun;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject explosion;

    public GameObject missle;

    public Color laserColor;

    public Material missleMaterial;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private int barrel;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = 90;
        base.Awake();
    }

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
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, lightBarrel, laserColor);
        lightShoot = false;
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrels[barrel], missleMaterial);
        barrel = barrel == (heavyBarrels.Length - 1) ? 0 : barrel + 1;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm("None");
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(knife, hold);
        character.OverrideArm("None");
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm("Left");
        Deploy(rightGun, foldAngle, 180f, 0f);
        Deploy(leftGun, foldAngle, 180f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(knife, holster);
        character.OverrideArm("None");
        Deploy(rightGun, deployAngle, 180f, 0f);
        Deploy(leftGun, deployAngle, 180f, 0f);
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

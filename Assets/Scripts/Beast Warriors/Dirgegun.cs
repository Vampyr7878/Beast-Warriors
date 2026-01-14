using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Dirgegun : BeastWarrior
{
    public GameObject launcher;

    public GameObject holster;

    public GameObject hold;

    public GameObject backLauncher;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = -90;
        deployAngle = -325;
        base.Awake();
    }

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
        Equip(launcher, holster);
        character.OverrideArm("None");
        Deploy(backLauncher, foldAngle, 0f, -180f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm("None");
        Deploy(backLauncher, foldAngle, 0f, -180f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        character.OverrideArm("None");
        Deploy(backLauncher, deployAngle, 0f, -180f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, hold);
        character.OverrideArm("Right");
        Deploy(backLauncher, foldAngle, 0f, -180f);
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

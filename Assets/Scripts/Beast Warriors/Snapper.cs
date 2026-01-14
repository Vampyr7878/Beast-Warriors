using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Snapper : BeastWarrior
{
    public GameObject cannon;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    public float fireRate;

    public float bulletInaccuracy;

    private float time;

    private int barrel;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun();
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            ShootMissle();
        }
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
    }

    void ShootMissle()
    {
        animator.SetTrigger("Shoot");
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
        Equip(cannon, holster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, hold);
        character.OverrideArm("Right");
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
                heavyShoot = context.performed;
                break;
        }
    }
}

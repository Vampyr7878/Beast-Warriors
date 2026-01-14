using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Prowl : BeastWarrior
{
    public GameObject tail;
    
    public GameObject fork;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject bullet;

    public Color laserColor;

    public float laserInaccuracy;

    public float fireRate;

    public float bulletInaccuracy;

    private float time;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLaser();
        }
        if (heavyShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun();
                time = 0;
            }
            time += Time.deltaTime;
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

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, heavyBarrels[0]);
        RaycastBullet(bullet, direction, layerMask, heavyBarrels[1]);
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(false);
        fork.SetActive(true);
        Equip(fork, hold);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        tail.SetActive(true);
        fork.SetActive(false);
        Equip(fork, holster);
        character.OverrideArm("Both");
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

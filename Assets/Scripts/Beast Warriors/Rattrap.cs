using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Rattrap : BeastWarrior
{
    public GameObject rifleFront;

    public GameObject rifleBack;

    public GameObject bomb;

    public GameObject frontHolster;

    public GameObject backHolster;

    public GameObject left;

    public GameObject front;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject bullet;

    public GameObject thrown;

    public float fireRate;

    public float bulletInaccuracy;

    public int angle;

    public int force;

    private float time;

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
            ThrowBomb();
        }
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrel);
    }

    void ThrowBomb()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y - 180, 0f);
        Vector3 aim = Quaternion.AngleAxis(-angle, cameraAimHelper.right) * cameraAimHelper.forward;
        ThrownProjectile(thrown, bomb, direction, aim, transform.forward, hold, force);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, backHolster);
        Equip(bomb, left);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, hold);
        Equip(bomb, left);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, front);
        Equip(rifleBack, hold);
        Equip(bomb, left);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rifleFront, frontHolster);
        Equip(rifleBack, backHolster);
        Equip(bomb, hold);
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

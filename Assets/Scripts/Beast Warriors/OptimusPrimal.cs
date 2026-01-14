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

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

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
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
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
        Equip(flail, rightHold);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("None");
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm("None");
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("Left");
        Deploy(rightCannon, foldAngle, 0f, 0f);
        Deploy(leftCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(flail, flailHolster);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("None");
        Deploy(rightCannon, deployAngle, 0f, 0f);
        Deploy(leftCannon, deployAngle, 0f, 0f);
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

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Rhinox : BeastWarrior
{
    public GameObject sword;

    public GameObject gun;

    public GameObject rightBlaster;

    public GameObject leftBlaster;

    public GameObject swordHolster;

    public GameObject gunHolster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    private int barrel;

    new void Awake()
    {
        foldAngle = 180;
        deployAngle = 0;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootBolt();
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

    void ShootBolt()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = boltColor;
        colors[0].time = 0f;
        colors[1].color = boltColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, bolt, direction, direction, lightBarrels[0], boltColor, boltColor, g);
        ParticleProjectile(flash, bolt, direction, direction, lightBarrels[1], boltColor, boltColor, g);
        lightShoot = false;
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, heavyBarrels[barrel]);
        barrel = barrel == (heavyBarrels.Length - 1) ? 0 : barrel + 1;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(gun, gunHolster);
        character.OverrideArm("None");
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, hold);
        Equip(gun, gunHolster);
        character.OverrideArm("None");
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(gun, gunHolster);
        character.OverrideArm("Both");
        Deploy(rightBlaster, 0f, deployAngle, 0f);
        Deploy(leftBlaster, 0f, deployAngle, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(gun, hold);
        character.OverrideArm("Right");
        Deploy(rightBlaster, 0f, foldAngle, 0f);
        Deploy(leftBlaster, 0f, foldAngle, 0f);
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
                time = fireRate;
                barrel = 0;
                break;
        }
    }
}

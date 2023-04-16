using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class SeaClamp : BeastWarrior
{
    public GameObject launcher;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightClaws;

    public GameObject leftClaws;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject bullet;

    public GameObject flash;

    public GameObject bolt;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    private int barrel;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = 160;
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
            ShootBolt();
        }
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel + 4]);
        barrel = barrel == (lightBarrels.Length - 5) ? 0 : barrel + 1;
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
        ParticleProjectile(flash, bolt, direction, direction, heavyBarrels[0], boltColor, boltColor, g);
        ParticleProjectile(flash, bolt, direction, direction, heavyBarrels[1], boltColor, boltColor, g);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        Deploy(rightClaws, 0f, -deployAngle, 80f);
        Deploy(leftClaws, 0f, deployAngle + 180, -80f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, holster);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(launcher, hold);
        Deploy(rightClaws, 0f, foldAngle, 80f);
        Deploy(leftClaws, 0f, foldAngle + 180, -80f);
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

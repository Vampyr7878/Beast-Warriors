using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Jetstorm : BeastWarrior
{
    public GameObject rightGun;

    public GameObject rightGunBarrel;

    public GameObject leftGun;

    public GameObject leftGunBarrel;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Color boltColor;

    public Color laserColor;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 180;
        deployAngle = 90;
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
            ShootLaser();
        }
    }

    void ShootBolt()
    {
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

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, heavyBarrel, laserColor);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightGun, 0f, deployAngle, 0f);
        Deploy(rightGunBarrel, foldAngle, 0f, 0f);
        Deploy(leftGun, 0f, -deployAngle, 0f);
        Deploy(leftGunBarrel, -foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightGun, 0f, foldAngle, 0f);
        Deploy(rightGunBarrel, deployAngle, 0f, 0f);
        Deploy(leftGun, 0f, -foldAngle, 0f);
        Deploy(leftGunBarrel, -deployAngle, 0f, 0f);
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

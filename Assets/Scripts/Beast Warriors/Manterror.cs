using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Manterror : BeastWarrior
{
    public GameObject rightLauncher;

    public GameObject leftLauncher;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject disc;

    public Color laserColor;

    public Color discColor;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 9;
        deployAngle = 170;
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
            ShootDisc();
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

    void ShootDisc()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 90f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = discColor;
        colors[0].time = 0f;
        colors[1].color = discColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, disc, direction, direction, heavyBarrels[0], discColor, discColor, g);
        ParticleProjectile(flash, disc, direction, direction, heavyBarrels[1], discColor, discColor, g);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightLauncher, 0f, foldAngle, 0f);
        Deploy(leftLauncher, 0f, -foldAngle, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightLauncher, 0f, deployAngle, 0f);
        Deploy(leftLauncher, 0f, -deployAngle, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightLauncher, 0f, foldAngle, 0f);
        Deploy(leftLauncher, 0f, -foldAngle, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Deploy(rightLauncher, 0f, deployAngle, 0f);
        Deploy(leftLauncher, 0f, -deployAngle, 0f);
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

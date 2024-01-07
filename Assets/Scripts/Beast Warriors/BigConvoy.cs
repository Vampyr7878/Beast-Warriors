using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BigConvoy : BeastWarrior
{
    public GameObject cannon;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightBaton;

    public GameObject leftBaton;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject explosion;

    public GameObject missle;

    public GameObject flash;

    public GameObject ball;

    public Material missleMaterial;

    public Color ballColor;

    private float foldAngle;

    private float deployAngle;

    private int barrel;

    new void Awake()
    {
        foldAngle = 80;
        deployAngle = 100;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootMissle();
        }
        if (heavyShoot)
        {
            ShootBall();
        }
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, lightBarrels[barrel], missleMaterial);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
        lightShoot = false;
    }

    void ShootBall()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = ballColor;
        colors[0].time = 0f;
        colors[1].color = ballColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, ball, direction, direction, heavyBarrel, ballColor, ballColor, g);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        Deploy(rightBaton, 0, 90, foldAngle);
        Deploy(leftBaton, 0, -90, -foldAngle);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        Deploy(rightBaton, 0, 90, -deployAngle);
        Deploy(leftBaton, 0, -90, deployAngle);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, holster);
        Deploy(rightBaton, 0, 90, foldAngle);
        Deploy(leftBaton, 0, -90, -foldAngle);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(cannon, hold);
        Deploy(rightBaton, 0, 90, foldAngle);
        Deploy(leftBaton, 0, -90, -foldAngle);
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

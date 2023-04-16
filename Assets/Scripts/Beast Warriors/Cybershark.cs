using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cybershark : BeastWarrior
{
    public GameObject head;

    public GameObject tail;

    public GameObject chestCannon;

    public GameObject headHolster;

    public GameObject tailHolster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject ball;

    public GameObject explosion;

    public GameObject missle;

    public Color ballColor;

    public Material missleMaterial;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = -90;
        deployAngle = 0;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootBall();
        }
        if (heavyShoot)
        {
            ShootMissle();
        }
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
        ParticleProjectile(flash, ball, direction, direction, lightBarrel, ballColor, ballColor, g);
        lightShoot = false;
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrel, missleMaterial);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        Equip(tail, tailHolster);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(head, headHolster);
        Equip(tail, hold);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(head, headHolster);
        Equip(tail, hold);
        Deploy(chestCannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(head, hold);
        Equip(tail, tailHolster);
        Deploy(chestCannon, deployAngle, 0f, 0f);
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

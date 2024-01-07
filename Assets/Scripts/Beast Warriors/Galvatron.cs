using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Galvatron : BeastWarrior
{
    public GameObject rightAxe;

    public GameObject leftAxe;

    public GameObject claw;

    public GameObject rightBlaster;

    public GameObject leftBlaster;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject clawHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public GameObject ball;

    public Color boltColor;

    public Color ballColor;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 90;
        deployAngle = 270;
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
            ShootBall();
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
        Equip(rightAxe, rightHold);
        Equip(leftAxe, leftHold);
        Equip(claw, clawHolster);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHolster);
        Equip(leftAxe, leftHolster);
        Equip(claw, rightHold);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHold);
        Equip(leftAxe, leftHold);
        Equip(claw, clawHolster);
        Deploy(rightBlaster, 0f, deployAngle, 90f);
        Deploy(leftBlaster, 0f, -deployAngle, -90f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightAxe, rightHolster);
        Equip(leftAxe, leftHolster);
        Equip(claw, rightHold);
        Deploy(rightBlaster, 0f, foldAngle, 90f);
        Deploy(leftBlaster, 0f, -foldAngle, -90f);
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
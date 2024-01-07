using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class LioConvoy : BeastWarrior
{
    public GameObject rightBlaster;

    public GameObject leftBlaster;

    public GameObject rightClaw;

    public GameObject leftClaw;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public Color boltColor;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 90;
        deployAngle = -90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLightBolt();
        }
        if (heavyShoot)
        {
            ShootHeavyBolt();
        }
    }

    void ShootLightBolt()
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

    void ShootHeavyBolt()
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
        Equip(rightBlaster, rightHolster);
        Equip(leftBlaster, leftHolster);
        Deploy(rightClaw, 0f, foldAngle, 90f);
        Deploy(leftClaw, 0f, -foldAngle, -90f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlaster, rightHolster);
        Equip(leftBlaster, leftHolster);
        Deploy(rightClaw, 0f, deployAngle, 90f);
        Deploy(leftClaw, 0f, -deployAngle, -90f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlaster, rightHolster);
        Equip(leftBlaster, leftHolster);
        Deploy(rightClaw, 0f, foldAngle, 90f);
        Deploy(leftClaw, 0f, -foldAngle, -90f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlaster, rightHold);
        Equip(leftBlaster, leftHold);
        Deploy(rightClaw, 0f, foldAngle, 90f);
        Deploy(leftClaw, 0f, -foldAngle, -90f);
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
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class RamHorn : BeastWarrior
{
    public GameObject fold;

    public GameObject claw;

    public GameObject holster;

    public GameObject hold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject ball;

    public Color laserColor;

    public Color ballColor;

    public float laserInaccuracy;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootLaser();
        }
        if (heavyShoot)
        {
            ShootBall();
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
        ParticleProjectile(flash, ball, direction, direction, heavyBarrels[0], ballColor, ballColor, g);
        ParticleProjectile(flash, ball, direction, direction, heavyBarrels[1], ballColor, ballColor, g);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(false);
        claw.SetActive(true);
        Equip(claw, hold);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
        character.OverrideArm("None");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        fold.SetActive(true);
        claw.SetActive(false);
        Equip(claw, holster);
        character.OverrideArm("Both");
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

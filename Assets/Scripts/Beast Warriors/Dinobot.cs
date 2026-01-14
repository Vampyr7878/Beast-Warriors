using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Dinobot : BeastWarrior
{
    public GameObject sword;

    public GameObject slash;

    public GameObject swordHolster;

    public GameObject slashHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Color boltColor;

    public Color laserColor;

    public float laserInaccuracy;

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
        ParticleProjectile(flash, bolt, direction, direction, lightBarrel, boltColor, boltColor, g);
        lightShoot = false;
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, heavyBarrels[0], laserColor);
        RaycastLaser(laser, direction, layerMask, heavyBarrels[1], laserColor);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(slash, slashHolster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, rightHold);
        Equip(slash, leftHold);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, slashHolster);
        Equip(slash, leftHold);
        character.OverrideArm("Left");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, swordHolster);
        Equip(slash, slashHolster);
        character.OverrideArm("None");
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

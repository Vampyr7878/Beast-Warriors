using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cheetor : BeastWarrior
{
    public GameObject rifle;

    public GameObject cannon;

    public GameObject rifleHolster;

    public GameObject cannonHolster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public LineRenderer laser;

    public GameObject flash;

    public GameObject ball;

    public Color laserColor;

    public Color flashColor;

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
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, lightBarrel, laserColor);
        lightShoot = false;
    }

    void ShootBall()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = flashColor;
        colors[0].time = 0f;
        colors[1].color = ballColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, ball, direction, direction, heavyBarrel, flashColor, ballColor, g);
        heavyShoot = false;
    }

    void EquipRifle(GameObject attachment)
    {
        rifle.transform.parent = attachment.transform;
        rifle.transform.localPosition = Vector3.zero;
        rifle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipCannon(GameObject attachment)
    {
        cannon.transform.parent = attachment.transform;
        cannon.transform.localPosition = Vector3.zero;
        cannon.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRifle(rifleHolster);
        EquipCannon(cannonHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRifle(rifleHolster);
        EquipCannon(cannonHolster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRifle(hold);
        EquipCannon(cannonHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRifle(rifleHolster);
        EquipCannon(hold);
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
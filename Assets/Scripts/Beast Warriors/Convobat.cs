using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Convobat : BeastWarrior
{
    public GameObject rightScimitar;

    public GameObject leftScimitar;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public LineRenderer laser;

    public GameObject sonic;

    public GameObject ripple;

    public Color laserColor;

    public Color rippleColor;

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
            ShootRipple();
        }
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, lightBarrels[0], laserColor);
        RaycastLaser(laser, direction, layerMask, lightBarrels[1], laserColor);
        lightShoot = false;
    }

    void ShootRipple()
    {
        Vector3 sonicDirection = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y + 180, 0f);
        Vector3 rippleDirection = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = rippleColor;
        colors[0].time = 0f;
        colors[1].color = rippleColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[3];
        alphas[0].alpha = 0f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 0.5f;
        alphas[2].alpha = 0f;
        alphas[2].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(sonic, ripple, sonicDirection, rippleDirection, heavyBarrels[0], rippleColor, rippleColor, g);
        ParticleProjectile(sonic, ripple, sonicDirection, rippleDirection, heavyBarrels[1], rippleColor, rippleColor, g);
        heavyShoot = false;
    }

    void EquipRightScimitar(GameObject attachment)
    {
        rightScimitar.transform.parent = attachment.transform;
        rightScimitar.transform.localPosition = Vector3.zero;
        rightScimitar.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLeftScimitar(GameObject attachment)
    {
        leftScimitar.transform.parent = attachment.transform;
        leftScimitar.transform.localPosition = Vector3.zero;
        leftScimitar.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHold);
        EquipLeftScimitar(leftHold);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
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

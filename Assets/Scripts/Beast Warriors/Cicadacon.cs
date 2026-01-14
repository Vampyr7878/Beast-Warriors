using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Cicadacon : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject cannon;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject flash;

    public GameObject ball;

    public Color flashColor;

    public Color ballColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    private int barrel;

    new void Awake()
    {
        foldAngle = 0;
        deployAngle = 90;
        base.Awake();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun();
                time = 0;
            }
            time += Time.deltaTime;
        }
        if (heavyShoot)
        {
            ShootBall();
        }
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel]);
        RaycastBullet(bullet, direction, layerMask, lightBarrels[barrel + 2]);
        barrel = barrel == (lightBarrels.Length - 3) ? 0 : barrel + 1;
    }

    void ShootBall()
    {
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

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("None");
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm("None");
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("Both");
        Deploy(cannon, foldAngle, 0f, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHolster);
        Equip(leftSword, leftHolster);
        character.OverrideArm("None");
        Deploy(cannon, deployAngle, 0f, 0f);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                barrel = 0;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

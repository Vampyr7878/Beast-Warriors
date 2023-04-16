using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Ironhide : BeastWarrior
{
    public GameObject rightClub;

    public GameObject leftClub;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightBlade;

    public GameObject leftBlade;

    public GameObject[] lightBarrels;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject bullet;

    public Color boltColor;

    public float fireRate;

    public float bulletInaccuracy;

    private float foldAngle;

    private float deployAngle;

    private float time;

    private int barrel;

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
            ShootBolt();
        }
        if (heavyShoot)
        {
            if (time >= fireRate)
            {
                ShootMachineGun();
                time = 0;
            }
            time += Time.deltaTime;
        }
    }

    void ShootBolt()
    {
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
        ParticleProjectile(flash, bolt, direction, direction, lightBarrels[barrel], boltColor, boltColor, g);
        barrel = barrel == (lightBarrels.Length - 1) ? 0 : barrel + 1;
        lightShoot = false;
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, heavyBarrels[0]);
        RaycastBullet(bullet, direction, layerMask, heavyBarrels[1]);
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        Deploy(rightBlade, 0f, deployAngle, 80f);
        Deploy(leftBlade, 0f, -deployAngle, -80f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHold);
        Equip(leftClub, leftHold);
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
        barrel = 0;
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightClub, rightHolster);
        Equip(leftClub, leftHolster);
        Deploy(rightBlade, 0f, foldAngle, 80f);
        Deploy(leftBlade, 0f, -foldAngle, -80f);
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
                time = fireRate;
                break;
        }
    }
}

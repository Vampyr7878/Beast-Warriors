using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Jetstorm : BeastWarrior
{
    public GameObject rightGun;

    public GameObject rightGunBarrel;

    public GameObject leftGun;

    public GameObject leftGunBarrel;

    public GameObject[] lightBarrels;

    public GameObject heavyBarrel;

    public GameObject flash;

    public GameObject bolt;

    public LineRenderer laser;

    public Color boltColor;

    public Color laserColor;

    public float laserInaccuracy;

    private float foldAngle;

    private float deployAngle;

    new void Awake()
    {
        foldAngle = 180;
        deployAngle = 90;
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
            ShootLaser();
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
        ParticleProjectile(flash, bolt, direction, direction, lightBarrels[0], boltColor, boltColor, g);
        ParticleProjectile(flash, bolt, direction, direction, lightBarrels[1], boltColor, boltColor, g);
        lightShoot = false;
    }

    void ShootLaser()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), 1);
        RaycastLaser(laser, direction, layerMask, heavyBarrel, laserColor);
        heavyShoot = false;
    }

    void DeployGuns(bool enable)
    {
        if (enable)
        {
            rightGun.transform.localEulerAngles = new Vector3(rightGun.transform.localEulerAngles.x, deployAngle, rightGun.transform.localEulerAngles.z);
            rightGunBarrel.transform.localEulerAngles = new Vector3(foldAngle, rightGunBarrel.transform.localEulerAngles.y, rightGunBarrel.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(leftGun.transform.localEulerAngles.x, -deployAngle, leftGun.transform.localEulerAngles.z);
            leftGunBarrel.transform.localEulerAngles = new Vector3(-foldAngle, leftGunBarrel.transform.localEulerAngles.y, leftGunBarrel.transform.localEulerAngles.z);
        }
        else
        {
            rightGun.transform.localEulerAngles = new Vector3(rightGun.transform.localEulerAngles.x, foldAngle, rightGun.transform.localEulerAngles.z);
            rightGunBarrel.transform.localEulerAngles = new Vector3(deployAngle, rightGunBarrel.transform.localEulerAngles.y, rightGunBarrel.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(leftGun.transform.localEulerAngles.x, -foldAngle, leftGun.transform.localEulerAngles.z);
            leftGunBarrel.transform.localEulerAngles = new Vector3(-deployAngle, leftGunBarrel.transform.localEulerAngles.y, leftGunBarrel.transform.localEulerAngles.z);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        DeployGuns(false);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        DeployGuns(false);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        DeployGuns(true);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        DeployGuns(false);
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

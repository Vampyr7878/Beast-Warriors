using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Wolfang : BeastWarrior
{
    public GameObject shield;

    public GameObject gun;

    public GameObject shieldHlster;

    public GameObject gunHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject explosion;

    public GameObject missle;

    public GameObject flash;

    public GameObject ball;

    public Material missleMaterial;

    public Color ballColor;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootMissle();
        }
        if (heavyShoot)
        {
            ShootBall();
        }
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, lightBarrel, missleMaterial);
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

    void EquipShield(GameObject attachment)
    {
        shield.transform.parent = attachment.transform;
        shield.transform.localPosition = Vector3.zero;
        shield.transform.localEulerAngles = Vector3.zero;
    }

    void EquipGun(GameObject attachment)
    {
        gun.transform.parent = attachment.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipShield(shieldHlster);
        EquipGun(gunHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipShield(leftHold);
        EquipGun(gunHolster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipShield(shieldHlster);
        EquipGun(gunHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipShield(shieldHlster);
        EquipGun(rightHold);
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

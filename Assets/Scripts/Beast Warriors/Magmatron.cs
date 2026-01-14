using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Magmatron : BeastWarrior
{
    public GameObject sword;

    public GameObject holster;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject[] heavyBarrels;

    public GameObject flash;

    public GameObject bolt;

    public GameObject explosion;

    public GameObject missle;

    public Color boltColor;

    public Material missleMaterial;

    private int barrel;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ShootBolt();
        }
        if (heavyShoot)
        {
            ShootMissle();
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

    void ShootMissle()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrels[barrel], missleMaterial);
        barrel = barrel == (heavyBarrels.Length - 1) ? 0 : barrel + 1;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm("None");
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, hold);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, hold);
        character.OverrideArm("Right");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(sword, holster);
        character.OverrideArm("Left");
        barrel = 0;
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

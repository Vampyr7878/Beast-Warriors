using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Silverbolt : BeastWarrior
{
    public GameObject rightBlade;

    public GameObject leftBlade;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject rightCannons;

    public GameObject leftCannons;

    public GameObject[] heavyBarrels;

    public GameObject thrown;

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    public int angle;

    public int force;

    private float foldAngle;

    private float deployAngle;

    private int barrel;

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
            ThrowBlade();
        }
        if (heavyShoot)
        {
            ShootMissle();
        }
    }

    void ThrowBlade()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x + 180, transform.eulerAngles.y + 90, 0f);
        Vector3 aim = Quaternion.AngleAxis(-angle, cameraAimHelper.right) * cameraAimHelper.forward;
        ThrownProjectile(thrown, rightBlade, direction, aim, transform.forward, rightHold, force, true);
        lightShoot = false;
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrels[barrel], missleMaterial);
        MeshProjectile(explosion, missle, direction, heavyBarrels[barrel + 3], missleMaterial);
        barrel = barrel == (heavyBarrels.Length - 4) ? 0 : barrel + 1;
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHolster);
        Equip(leftBlade, leftHolster);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHold);
        Equip(leftBlade, leftHold);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHold);
        Equip(leftBlade, leftHolster);
        Deploy(rightCannons, 0f, foldAngle, 0f);
        Deploy(leftCannons, 0f, -foldAngle, 0f);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        Equip(rightBlade, rightHolster);
        Equip(leftBlade, leftHolster);
        Deploy(rightCannons, 0f, deployAngle, 0f);
        Deploy(leftCannons, 0f, -deployAngle, 0f);
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

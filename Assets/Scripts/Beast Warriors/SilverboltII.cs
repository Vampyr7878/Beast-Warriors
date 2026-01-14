using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class SilverboltII : BeastWarrior
{
    public GameObject rightSword;

    public GameObject leftSword;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    public GameObject[] heavyBarrels;

    public GameObject thrown;

    public GameObject explosion;

    public GameObject missle;

    public Material missleMaterial;

    public int angle;

    public int force;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightShoot)
        {
            ThrowSword();
        }
        if (heavyShoot)
        {
            ShootMissle();
        }
    }

    void ThrowSword()
    {
        animator.SetTrigger("Shoot");
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x + 180, transform.eulerAngles.y + 90, 0f);
        Vector3 aim = Quaternion.AngleAxis(-angle, cameraAimHelper.right) * cameraAimHelper.forward;
        ThrownProjectile(thrown, rightSword, direction, aim, transform.forward, rightHold, force, true);
        lightShoot = false;
    }

    void ShootMissle()
    {
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y, 0f);
        MeshProjectile(explosion, missle, direction, heavyBarrels[0], missleMaterial);
        MeshProjectile(explosion, missle, direction, heavyBarrels[1], missleMaterial);
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
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
        character.OverrideArm("None");
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.enabled = true;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHolster);
        character.OverrideArm("Right");
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.enabled = false;
        animator.SetInteger("Weapon", weapon);
        Equip(rightSword, rightHold);
        Equip(leftSword, leftHold);
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

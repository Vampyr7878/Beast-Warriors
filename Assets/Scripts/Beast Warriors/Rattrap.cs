using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Rattrap : BeastWarrior
{
    public GameObject gunFront;

    public GameObject gunBack;

    public GameObject bomb;

    public GameObject frontHolster;

    public GameObject backHolster;

    public GameObject left;

    public GameObject front;

    public GameObject hold;

    public GameObject lightBarrel;

    public GameObject heavyBarrel;

    public GameObject bullet;

    public GameObject thrown;

    public float fireRate;

    public float bulletInaccuracy;

    private float time;

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
            ThrowBomb();
        }
    }

    void EquipGunFront(GameObject attachment)
    {
        gunFront.transform.parent = attachment.transform;
        gunFront.transform.localPosition = Vector3.zero;
        gunFront.transform.localEulerAngles = Vector3.zero;
    }

    void EquipGunBack(GameObject attachment)
    {
        gunBack.transform.parent = attachment.transform;
        gunBack.transform.localPosition = Vector3.zero;
        gunBack.transform.localEulerAngles = Vector3.zero;
    }

    void EquipBomb(GameObject attachment)
    {
        bomb.transform.parent = attachment.transform;
        bomb.transform.localPosition = Vector3.zero;
        bomb.transform.localEulerAngles = Vector3.zero;
    }

    void ShootMachineGun()
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        animator.SetTrigger("Shoot");
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), 1);
        RaycastBullet(bullet, direction, layerMask, lightBarrel);
    }

    void ThrowBomb()
    {
        animator.SetTrigger("Shoot");
        Vector3 aim = Quaternion.AngleAxis(-30, cameraAimHelper.right) * cameraAimHelper.forward;
        Vector3 direction = new(-cameraAimHelper.eulerAngles.x, transform.eulerAngles.y - 180, 0f);
        GameObject t = Instantiate(thrown);
        t.transform.position = hold.transform.position;
        t.transform.eulerAngles = direction;
        Rigidbody b = t.GetComponent<Rigidbody>();
        b.AddForce(aim * 500, ForceMode.Impulse);
        heavyShoot = false;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGunFront(frontHolster);
        EquipGunBack(backHolster);
        EquipBomb(left);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGunFront(frontHolster);
        EquipGunBack(backHolster);
        EquipBomb(left);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGunFront(front);
        EquipGunBack(hold);
        EquipBomb(left);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGunFront(frontHolster);
        EquipGunBack(backHolster);
        EquipBomb(hold);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                lightShoot = context.performed;
                time = fireRate;
                break;
            case 4:
                heavyShoot = context.performed;
                break;
        }
    }
}

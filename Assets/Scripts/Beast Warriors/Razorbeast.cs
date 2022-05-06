using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Razorbeast : BeastWarrior
{
    public GameObject gun;

    public GameObject holster;

    public GameObject hold;

    public GameObject rightGun;

    public GameObject leftGun;

    float foldAngle;

    float deployAngle;

    new void Awake()
    {
        foldAngle = -40;
        deployAngle = -80;
        base.Awake();
    }

    void EquipGun(GameObject attachment)
    {
        gun.transform.parent = attachment.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;
    }

    void DeployGuns(bool enable)
    {
        if (enable)
        {
            rightGun.transform.localEulerAngles = new Vector3(deployAngle, rightGun.transform.localEulerAngles.y, rightGun.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(deployAngle, leftGun.transform.localEulerAngles.y, leftGun.transform.localEulerAngles.z);
        }
        else
        {
            rightGun.transform.localEulerAngles = new Vector3(foldAngle, rightGun.transform.localEulerAngles.y, rightGun.transform.localEulerAngles.z);
            leftGun.transform.localEulerAngles = new Vector3(foldAngle, leftGun.transform.localEulerAngles.y, leftGun.transform.localEulerAngles.z);
        }
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(false);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(false);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
        DeployGuns(true);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(hold);
        DeployGuns(false);
    }

    public override void OnAttack(CallbackContext context)
    {
        switch (weapon)
        {
            case 3:
                Debug.Log("Light Fire");
                break;
            case 4:
                Debug.Log("Heavy Fire");
                break;
        }
    }
}

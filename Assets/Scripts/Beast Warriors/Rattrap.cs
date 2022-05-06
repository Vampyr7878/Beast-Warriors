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

    public GameObject weaponHold;

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
        EquipGunBack(weaponHold);
        EquipBomb(left);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGunFront(frontHolster);
        EquipGunBack(backHolster);
        EquipBomb(weaponHold);
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

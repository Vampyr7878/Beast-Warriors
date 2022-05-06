using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Convobat : BeastWarrior
{
    public GameObject rightScimitar;

    public GameObject leftScimitar;

    public GameObject rightHolster;

    public GameObject leftHolster;

    public GameObject rightHold;

    public GameObject leftHold;

    void EquipRightScimitar(GameObject attachment)
    {
        rightScimitar.transform.parent = attachment.transform;
        rightScimitar.transform.localPosition = Vector3.zero;
        rightScimitar.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLeftScimitar(GameObject attachment)
    {
        leftScimitar.transform.parent = attachment.transform;
        leftScimitar.transform.localPosition = Vector3.zero;
        leftScimitar.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHold);
        EquipLeftScimitar(leftHold);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightScimitar(rightHolster);
        EquipLeftScimitar(leftHolster);
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

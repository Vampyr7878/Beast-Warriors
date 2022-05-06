using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Insecticon : BeastWarrior
{
    public GameObject rightSickle;

    public GameObject leftSickle;

    public GameObject crossbow;

    public GameObject right;

    public GameObject left;

    public GameObject holster;

    public GameObject rightHold;

    public GameObject leftHold;

    void EquipRightSickle(GameObject attachment)
    {
        rightSickle.transform.parent = attachment.transform;
        rightSickle.transform.localPosition = Vector3.zero;
        rightSickle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipLeftSickle(GameObject attachment)
    {
        leftSickle.transform.parent = attachment.transform;
        leftSickle.transform.localPosition = Vector3.zero;
        leftSickle.transform.localEulerAngles = Vector3.zero;
    }

    void EquipCrossbow(GameObject attachment)
    {
        crossbow.transform.parent = attachment.transform;
        crossbow.transform.localPosition = Vector3.zero;
        crossbow.transform.localEulerAngles = Vector3.zero;
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(rightHold);
        EquipLeftSickle(leftHold);
        EquipCrossbow(holster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(holster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipRightSickle(right);
        EquipLeftSickle(left);
        EquipCrossbow(rightHold);
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

using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Armordillo : BeastWarrior
{
    public GameObject mace;

    public GameObject gun;

    public GameObject maceHolster;

    public GameObject gunHolster;

    public GameObject hold;

    void EquipMace(GameObject attachment)
    {
        mace.transform.parent = attachment.transform;
        mace.transform.localPosition = Vector3.zero;
        mace.transform.localEulerAngles = Vector3.zero;
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
        EquipMace(maceHolster);
        EquipGun(gunHolster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(hold);
        EquipGun(gunHolster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipGun(gunHolster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipMace(maceHolster);
        EquipGun(hold);
    }
}

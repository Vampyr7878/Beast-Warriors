using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Snapper : BeastWarrior
{
    public GameObject gun;

    public GameObject holster;

    public GameObject hold;

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
        EquipGun(holster);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(holster);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        EquipGun(hold);
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

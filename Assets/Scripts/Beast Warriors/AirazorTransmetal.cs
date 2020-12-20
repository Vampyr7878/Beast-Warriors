using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class AirazorTransmetal : BeastWarrior
{
    public GameObject rightSkid;

    public GameObject leftSkid;

    public GameObject rightGun;

    public GameObject leftGun;

    void GunMode(bool enable)
    {
        rightSkid.SetActive(!enable);
        leftSkid.SetActive(!enable);
        rightGun.SetActive(enable);
        leftGun.SetActive(enable);
    }

    public override void OnMeleeWeak(CallbackContext context)
    {
        weapon = 1;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        GunMode(false);
    }

    public override void OnMeleeStrong(CallbackContext context)
    {
        weapon = 2;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        GunMode(false);
    }

    public override void OnRangedWeak(CallbackContext context)
    {
        weapon = 3;
        animator.SetLayerWeight(1, 0f);
        animator.SetInteger("Weapon", weapon);
        GunMode(true);
    }

    public override void OnRangedStrong(CallbackContext context)
    {
        weapon = 4;
        animator.SetLayerWeight(1, 1f);
        animator.SetInteger("Weapon", weapon);
        GunMode(false);
    }
}

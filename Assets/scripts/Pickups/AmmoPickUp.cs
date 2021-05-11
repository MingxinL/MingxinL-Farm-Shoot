using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private bool collected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            //PlayerHealthController.instance.HealPlayer(healAmount);
            PlayerController.instance.activeGun.GetAmmo();
            Destroy(gameObject);
            collected = true;
            AudioManger.instance.PlaySFX(3);
        }
    }
}

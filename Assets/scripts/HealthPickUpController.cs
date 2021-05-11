using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUpController : MonoBehaviour
{
    public int healAmount;

    public bool isCollected;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isCollected)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManger.instance.PlaySFX(5);
        }
    }
    
}

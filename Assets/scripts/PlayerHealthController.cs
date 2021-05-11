using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int maxHealth, currentHealth;
    public float inbincibleLength = 1f;
    private float invincCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "Health :" + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invincCounter <= 0)
        {
            currentHealth -= damageAmount;
            //Debug.Log("Damage");
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDied();
                AudioManger.instance.StopBGM();
                AudioManger.instance.PlaySFX(1);
            }
            invincCounter = inbincibleLength;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "Health :" + currentHealth + "/" + maxHealth;
        }
        
    }


    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        Debug.Log("healling");
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "Health :" + currentHealth + "/" + maxHealth;
    }
}

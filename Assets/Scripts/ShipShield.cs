using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShield : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int currentHealth;
    [SerializeField] int regenerateAmount = 1;
    [SerializeField] float regenerateRate = 2f;

    private void Start()
    {
        currentHealth = maxHealth;

        Invoke("Regenerate", regenerateRate);
    }

    void Regenerate() {

        if (currentHealth < maxHealth)
        {
            currentHealth += regenerateAmount;
        }

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
     
    }

    public void TakeDamage(int dmg = 1) {
        currentHealth -= dmg;

        if (currentHealth < 1) {
            Debug.Log("SHIP DESTROYED!");
        }
    }
    
}

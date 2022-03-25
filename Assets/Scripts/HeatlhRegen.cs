using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatlhRegen : PlayerManager
{
	
	public float oxygenIncrease = 20f;
	
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            GameObject.Destroy(gameObject);
            collision.gameObject.GetComponentInParent<PlayerManager>().currentOxygen += 20f;
        }
    }
}

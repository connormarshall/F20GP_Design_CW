using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatlhRegen : PlayerManager
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            Destroy(gameObject);
            currentOxygen += 5;
        }
    }
}

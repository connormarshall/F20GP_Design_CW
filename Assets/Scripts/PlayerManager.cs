using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float currentOxygen;                         //creating some variables to enable
    private const float maxOxygen = 100f;               //an oxygen system within the player

    private const float healthDecreaseRate = 1.5f;      //oxygen decrease rates
                                                        //oxygen decrease is low due to fun, best for player to
                                                        //to feel like they are always gonna win, not lose
    void Start()
    {
        currentOxygen = maxOxygen;      //sets the player oxygen count to it's max value at the beginning of game
    }

    
    void Update()
    {
        currentOxygen -= healthDecreaseRate * Time.deltaTime;   //decreases player oxygen count over time
    }
}

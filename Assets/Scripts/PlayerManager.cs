using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float currentOxygen;                         //creating some variables to enable
    public float maxOxygen = 100f;               		//an oxygen system within the player

	public float currentHull = 100f;
	public float maxHull = 100f;

    private const float hullDecreaseRate = 1.5f;      //oxygen decrease rates
                                                        //oxygen decrease is low due to fun, best for player to
                                                        //to feel like they are always gonna win, not lose
														
    void Start()
    {
        currentOxygen = maxOxygen;      //sets the player oxygen count to it's max value at the beginning of game
    }

    
    void Update()
    {
		if(currentOxygen > 0.01f)
			currentOxygen -= hullDecreaseRate * 1.5f * Time.deltaTime;   //decreases player oxygen count over time
		else {
			currentOxygen = 0f;
			// death handling
			Application.Quit();
		}
    }
	
	public void DecreaseHull(Vector3 position, Vector3 normal)
	{
		if(currentHull != 0f)
			currentHull -= 10f;
		
		// spawn hull repair event object
		if(currentHull <= 0.001f)
		{
			// Eject player from ship
			
			// spawn hullRepair object at position and normal
			
		}
		
	}
	
}

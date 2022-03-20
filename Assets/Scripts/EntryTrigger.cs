using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTrigger : MonoBehaviour
{
    public GameObject ship;
	private CharacterControl pilotController;
	
	private void OnTriggerEnter(Collider hit)
	{
		if(hit.tag == "CharacterTrigger")
		{
			pilotController = hit.gameObject.GetComponentInParent(typeof(CharacterControl)) as CharacterControl;
			
			if(pilotController != null)
				pilotController.ship = this.ship;
		
		}
	}
	
	private void OnTriggerExit(Collider hit)
	{
		if(hit.tag == "CharacterTrigger")
		{
			if(pilotController != null)
				pilotController.ship = null;
		}
	}
	
}

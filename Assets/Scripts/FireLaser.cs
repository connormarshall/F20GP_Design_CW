using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaser : MonoBehaviour
{
	
	private RaycastHit hit;
	public GameObject laserPrefab;
	public float range = 1000f;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }
	
	void shoot()
	{
		Vector3 pos = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		Ray ray = Camera.main.ScreenPointToRay(pos);
		if(Physics.Raycast(ray, out hit, range))
		{
			GameObject laser = GameObject.Instantiate(laserPrefab, transform.position, transform.rotation) as GameObject;
			GameObject.Destroy(laser, 2f);
		}
	}
	
}

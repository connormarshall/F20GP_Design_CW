using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
	
	public float speed = 100f;
	public GameObject sparkParticles;
	public PlayerManager playerManager;
	
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
    }
	
	void OnCollisionEnter(Collision c)
	{
		// Spawn spark from shot
		ContactPoint hit = c.contacts[0];
		GameObject.Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
		
		// Decrease hull strength of ship
		playerManager.DecreaseHull(hit.point, hit.normal);
		
		// Destroy laser
		Destroy(gameObject);
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    public float spinSpeedMIN = 1f;
    public float spinSpeedMAX = 5f;
    public float thrustMIN = 0.5f;
    public float thrustMAX = 1f;

    private float spinSpeed;
	private float thrust;
	
	private Rigidbody rb;
	
	public Transform asteroidPrefab;
	private float health = 20f;
	
	private float immunityTime = 2f;
	
    
	void Awake()
	{
		spinSpeed = Random.Range(spinSpeedMIN, spinSpeedMAX);
        thrust = Random.Range(thrustMIN, thrustMAX);

        rb = GetComponent<Rigidbody>();
	}
	
    void Start()
    {
		
		// If asteroid is small enough to pick up, outline it
		if(transform.localScale.magnitude < 150f)
			GetComponent<Outline>().enabled = true;
		else
			GetComponent<Outline>().enabled = false;
		
    }
	
	void Update()
	{
		// Countdown to 0f, after which DecreaseHealth() will run
		if(immunityTime > 0.001f)
			immunityTime -= Time.deltaTime;
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddRelativeForce(transform.forward * thrust * Time.deltaTime, ForceMode.Impulse);
        rb.AddRelativeTorque(Vector3.up * spinSpeed * Time.deltaTime);
    }
	
	
	// Mining laser accesses this on raycast hit
	public void DecreaseHealth(float damage)
	{
		
		if(this.health < 0.001f)
			BreakAsteroid();
		else
			this.health -= damage * Time.deltaTime;
		
	}
	
	private void BreakAsteroid()
	{
		// If asteroid is too small, don't do anything
		if(transform.localScale.magnitude < 150f)
			return;
		
		// Generate smaller asteroids
		int numDivisions = Random.Range(2, 4);
		for(int i = 0; i < numDivisions; i++) {
			
			float r = Random.Range(0.5f, 0.7f);
			Vector3 scale = transform.localScale * r;
			
			// If smaller than this, don't bother generating it
			if(scale.magnitude < 50f)
				continue;
			
			Transform temp = Instantiate(
				asteroidPrefab,
				transform.position + Random.insideUnitSphere,
				Random.rotation
			);
			temp.localScale = scale;
			
		}
		
		// Destroy this one
		GameObject.Destroy(this.gameObject);
	}
	
}

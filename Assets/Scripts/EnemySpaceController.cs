using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySpaceController : MonoBehaviour
{
	
	public Rigidbody rb;
	public Transform spaceship;
	public Transform muzzle;
	public GameObject laserPrefab;
	
	public GameObject lastLaser;
	public int fireTime = 300;
	public int timeSinceShot;
	
	public float health = 40f;
	public PlayerManager playerManager;
	public AudioSource laserSound;
	public AudioSource explosionSound;
	public AudioSource enemySound;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
		timeSinceShot = fireTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.LookAt(spaceship);
		Vector3 distance = spaceship.position - transform.position;
		
		
		if(Random.Range(0f, 100f) > 99f && !enemySound.isPlaying)
		{
			enemySound.pitch += Random.Range(-0.3f, 0.3f);
			enemySound.Play();
		}
		
		if(!enemySound.isPlaying)
		{
			enemySound.pitch = 1f;
		}
		
		if(distance.magnitude < 200f && distance.magnitude > 20f)
		{
			 rb.AddRelativeForce(transform.forward * (distance.magnitude) * Time.deltaTime * 10f);
			 
		}
		
		if(distance.magnitude < 50f && timeSinceShot == fireTime)
		{
			// shoot laser
			RaycastHit hit;
			if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, 100f)) {
				lastLaser = Instantiate(laserPrefab, muzzle) as GameObject;
				lastLaser.GetComponent<LaserBehaviour>().playerManager = this.playerManager;
				timeSinceShot = 0;
				laserSound.Play();
				Destroy(lastLaser, 2f);
			} else {
			}
		}
		
		if(timeSinceShot != fireTime)
		{
			timeSinceShot++; 
		}
		
    }
	
	
	// Mining laser accesses this on raycast hit
	public void DecreaseHealth(float damage)
	{
		
		if(this.health < 0.001f)
		{
			explosionSound.Play();
			Destroy(gameObject);
		} else {
			this.health -= damage * Time.deltaTime;
		}
	}
	
}

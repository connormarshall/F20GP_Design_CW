using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public Transform asteroidPrefab;
    public int fieldRadius = 100;
    public int asteroidCount = 250;
	public float minAsteroidRadius = 0.5f;
	public float maxAsteroidRadius = 5f;

    void Start()
    {
        for (int i = 0; i < asteroidCount; i++) { 
            Transform temp = Instantiate(asteroidPrefab, transform.position + Random.insideUnitSphere * fieldRadius, Random.rotation);
			temp.localScale = temp.localScale * Random.Range(minAsteroidRadius, maxAsteroidRadius);
			
        }
    }
	
	
}

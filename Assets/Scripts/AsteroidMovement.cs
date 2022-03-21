using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float spinSpeedMIN = 1f;
    public float spinSpeedMAX = 5f;
    public float thrustMIN = 0.5f;
    public float thrustMAX = 1f;

    public float spinSpeed;

    
    void Start()
    {
        spinSpeed = Random.Range(spinSpeedMIN, spinSpeedMAX);
        float thrust = Random.Range(thrustMIN, thrustMAX);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}

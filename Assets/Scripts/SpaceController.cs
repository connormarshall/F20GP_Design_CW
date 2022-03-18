using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    //Setting speed related movement speeds for the player ship
    public float thrustSpeed = 20f;
    public float liftSpeed = 4.5f;                                 //NOTE FOR GROUP: Editable to suit final speed preferences. - love Brendan
    public float strafeSpeed = 6f;

    //Stores the input by the user whilst actively moving
    private float activeThrustSpeed; 
    private float activeLiftSpeed;
    private float activeStrafeSpeed;

    private float thrustAcceleration = 2.5f;           //how fast the ship moves from 0-1mph etc
    private float strafeAcceleration = 2f;              //want this to take half a second to accelerate
    private float liftAcceleration = 2f;
	
	// Stores input values from the ShipController action map
	private float thrustInput = 0f;
	private float strafeInput = 0f;
	private float liftInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gets input for axes when moving the ship
        activeThrustSpeed = Mathf.Lerp(activeThrustSpeed, thrustInput * thrustSpeed, thrustAcceleration * Time.deltaTime);  //allows for acceleration in the ship
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, strafeInput * strafeSpeed, strafeAcceleration * Time.deltaTime); 
        activeLiftSpeed = Mathf.Lerp(activeLiftSpeed, liftInput * liftSpeed, liftAcceleration * Time.deltaTime);

        //applying the input to the ship (movement)
        transform.position += transform.forward * activeThrustSpeed * Time.deltaTime; //always goes thrust based on rotation of the ship
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;    //same but for left and right
        transform.position += transform.up * activeLiftSpeed * Time.deltaTime;   //same but for Lifting
    }
	
	
	// Input System callbacks
	public void OnThrust(InputAction.CallbackContext context)
	{
		thrustInput = context.ReadValue<float>();
	}
	
	public void OnStrafe(InputAction.CallbackContext context)
	{
		strafeInput = context.ReadValue<float>();
	}
	
	public void OnLift(InputAction.CallbackContext context)
	{
		liftInput = context.ReadValue<float>();
	}
	
}

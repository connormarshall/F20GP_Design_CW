using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    //Setting speed related movement speeds for the player ship
    public float thrustSpeed = 500f;
    public float liftSpeed = 100f;                                 //NOTE FOR GROUP: Editable to suit final speed preferences. - love Brendan
    public float strafeSpeed = 200f;
	public float rollSpeed = 200f;
	public float turnSpeed = 300f;
	
	// Deccelerates ship when no input
	private float deccelerationRate = 0.9f;
	private float rotDeccelerationRate = 0.1f;
	private float thrustDrift = 0f;
	private float strafeDrift = 0f;
	private float liftDrift = 0f;
	private float rotDrift = 0f;
	
	// Stores input values from the ShipController action map
	private float thrustInput = 0f;
	private float strafeInput = 0f;
	private float liftInput = 0f;
	private float rollInput = 0f;
	private Vector2 mouseInput = new Vector2(0f, 0f);
	
	// Rigidbody of ship
	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		// Get rigidbody
		rb = GetComponent<Rigidbody>();
    }

	
	void FixedUpdate()
	{
		UpdateMovement();
	}
	
	void UpdateMovement()
	{
		// Thrust
		if(thrustInput > 0.1f || thrustInput < -0.1f)
		{
			rb.AddRelativeForce(Vector3.forward * thrustInput * thrustSpeed * Time.deltaTime);
			thrustDrift = thrustSpeed;
		} else {
			rb.AddRelativeForce(Vector3.forward * thrustDrift * Time.deltaTime);
			thrustDrift *= deccelerationRate;
		}
		
		// Strafe
		if(strafeInput > 0.1f ||strafeInput < -0.1f)
		{
			rb.AddRelativeForce(Vector3.right * strafeInput * strafeSpeed * Time.deltaTime);
			strafeDrift = strafeSpeed;
		} else {
			rb.AddRelativeForce(Vector3.right * strafeDrift * Time.deltaTime);
			strafeDrift *= deccelerationRate;
		}
		
		// Lift
		if(liftInput > 0.1f || liftInput < -0.1f)
		{
			rb.AddRelativeForce(Vector3.up * liftInput * liftSpeed * Time.deltaTime);
			liftDrift = liftSpeed;
		} else {
			rb.AddRelativeForce(Vector3.up * liftDrift * Time.deltaTime);
			liftDrift *= deccelerationRate;
		}
		
		// Pitch
		rb.AddRelativeTorque(
			Vector3.right 
			* Mathf.Clamp(-mouseInput.y, -1f, 1f)
			* turnSpeed
			* Time.deltaTime
		);
		
		// Yaw
		rb.AddRelativeTorque(
			Vector3.up 
			* Mathf.Clamp(mouseInput.x, -1f, 1f)
			* turnSpeed
			* Time.deltaTime
		);
		
		// Roll
		rb.AddRelativeTorque(
			Vector3.forward 
			* rollInput
			* rollSpeed
			* Time.deltaTime
		);
		
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
	
	public void OnMouse(InputAction.CallbackContext context)
	{
		mouseInput = context.ReadValue<Vector2>();
	}
	
	public void OnRoll(InputAction.CallbackContext context)
	{
		rollInput = context.ReadValue<float>();
	}
	
}

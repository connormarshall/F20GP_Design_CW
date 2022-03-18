using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
	
	// forward / backward
	public float thrust = 1000f;
	// up / down
	public float liftThrust = 1000f;
	// left / right
	public float strafeThrust = 1000f;
	// pitch (mouse Y)
	public float pitchTorque = 300f;
	// yaw (mouse X)
	public float yawTorque = 300f;
	
	
	/* Glide reduction / drag for ships to slow down automatically,
	 * but for the character we want the danger of floating indefinitely
	 * to be a mechanic.
	 */
	 
	private float thrustInput;
	private float liftInput;
	private float strafeInput;
	private Vector2 mouseInput;
	
	
	// References for the Pelivs
	public Transform pelvisTrans;
	public ConfigurableJoint pelivsConfigJoint;
	public Rigidbody pelvisRigidbody;
	
	// Animator of the animation model
	public Animator targetAnimator;
	
	// Main camera
	public Transform cam;
	// Transform of cam follow target
	public Transform rootTrans;
	
	private float pitch = 0.0f;
	private float yaw = 0.0f;
	
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
    }

	// Update is called once per frame
	void Update()
	{
		yaw += mouseInput.x * yawTorque;
		pitch -= mouseInput.y * pitchTorque;
		
		
		/* 360 rotation is susceptible to gimbal lock. Unity applies code in reverse order
		 * e.g. 3rd * 2nd * 1st. Since we are not applying roll to the character on the
		 * z axis, by applying it 3rd in the sum Gimbal lock is constrained to the unused
		 * z axis of rotation.
		 *
		 * This gimbal lock issue I suspect is to do with the bad pelvis mesh rotation again,
		 * which is also why pitch is around the z axis and yaw around the x.
		 */
		this.pelivsConfigJoint.targetRotation = Quaternion.AngleAxis(0, Vector3.up)
			* Quaternion.AngleAxis(pitch, Vector3.forward)
			* Quaternion.AngleAxis(yaw, Vector3.right);
			
	}
   
    void FixedUpdate()
    {
		UpdateMovement();
    }
	
	void UpdateMovement() {
		
		// Thrust
		if(thrustInput > 0.1f || thrustInput < -0.1f)
			pelvisRigidbody.AddRelativeForce(Vector3.up * thrustInput * thrust * Time.deltaTime);
		// Strafe
		if(strafeInput > 0.1f || strafeInput < -0.1f)
			pelvisRigidbody.AddRelativeForce(-Vector3.forward * strafeInput * strafeThrust * Time.deltaTime);
		// Lift
		if(liftInput > 0.1f || liftInput < -0.1f)
			pelvisRigidbody.AddRelativeForce(-Vector3.right * liftInput * liftThrust * Time.deltaTime);
		
		
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
	
	
}

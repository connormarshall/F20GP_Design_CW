using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;

public class CharacterControl : MonoBehaviour
{
	// If active, movement will update.
	public bool active = true;
	
	// Transformation of armature root bone
	public Transform rootTrans;
	public Rigidbody rootRb;
	
	// forward / backward
	public float thrust = 1000f;
	// up / down
	public float liftThrust = 1000f;
	// left / right
	public float strafeThrust = 1000f;
	// roll
	public float yawTorque = 300f;
	// pitch (mouse Y)
	public float pitchTorque = 300f;
	// yaw (mouse X)
	public float rollTorque = 20f;
	
	/* decceleration / drag for ships to slow down automatically,
	 * but for the character we want the danger of floating indefinitely
	 * to be a mechanic.
	 */
	 
	private float thrustInput;
	private float liftInput;
	private float strafeInput;
	private Vector2 mouseInput;
	private float rollInput;
	private float zoomInput;
	private float interactInput;
	
	// References for the Pelivs
	public Transform pelvisTrans;
	public ConfigurableJoint pelivsConfigJoint;
	public Rigidbody pelvisRigidbody;
	
	// Animator of the animation model
	public Animator targetAnimator;
	
	// Player camera
	public CinemachineVirtualCamera playerCam;
	// Zoomed in camera
	public CinemachineVirtualCamera zoomCam;
	
	// pitch / yaw deltas
	private float pitch = 0.0f;
	private float yaw = 0.0f;
	
	// Ship that the player is either in or in the entry trigger of;
	public GameObject ship;
	// Input map to toggle when controlling player
	public PlayerInput input;

	public GameObject hud;
	private HudController hc;

	// Start is called before the first frame update
	void Start()
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		hc = hud.GetComponent<HudController>();
	}

	// Update is called once per frame
	void Update()
	{
		
		// Change cinemachine cam priority to zoom in
		if(zoomInput > 0.1f)
		{
			playerCam.Priority = 11;
			zoomCam.Priority = 12;
		} else {
			zoomCam.Priority = 11;
			playerCam.Priority = 12;
		}
		
		/* Everything that should still happen while movement is disabled
		 * should go before this return;
		 */
		if(!active)
			return;
		
		yaw += mouseInput.x * yawTorque;
		pitch -= mouseInput.y * pitchTorque;
		
		
		/* 360 rotation is susceptible to gimbal lock. Unity applies code in reverse order
		 * e.g. 3rd * 2nd * 1st. This gimbal lock issue I suspect is to do with the bad 
		 * pelvis mesh rotation again.
		 * 
		 * So make sure wherever rotations are applied, they're applied to objects whos
		 * default rotation is aligned with the global axes (0, 0, 0 in the editor). If an 
		 * object isn't e.g. the pelivs, parent it (or in this case, attach its joint) to an 
		 * empty game object that is.
		 */
		 
		 
		// TODO: smooth decceleration for rotation
		// pitch + yaw don't deccelerate at all, roll is a hard stop because it's button-based
		// decceleration curve should be quite sharp for aiming purposes.
		
		rootTrans.Rotate(
			pitch * Time.deltaTime,
			yaw * Time.deltaTime,
			rollInput * rollTorque * Time.deltaTime,
			Space.Self
		);
		
	}
   
    void FixedUpdate()
    {
		/* Everything that should still happen while movement is disabled
		 * should go before this return;
		 */
		if(!active)
			return;
		
		UpdateMovement();
    }
	
	void UpdateMovement() {
		
		// Thrust
		if(thrustInput > 0.1f || thrustInput < -0.1f)
			rootRb.AddRelativeForce(Vector3.forward * thrustInput * thrust * Time.deltaTime);
		// Strafe
		if(strafeInput > 0.1f || strafeInput < -0.1f)
			rootRb.AddRelativeForce(Vector3.right * strafeInput * strafeThrust * Time.deltaTime);
		// Lift
		if(liftInput > 0.1f || liftInput < -0.1f)
			rootRb.AddRelativeForce(Vector3.up * liftInput * liftThrust * Time.deltaTime);
		
	}
	
	
	void BoardShip()
	{
		// Stop movement
		this.active = false;
		
		// Parent to ship transform so player follows ship
		transform.parent = ship.transform;
		
		// Set as the occupant
		SpaceController shipControl = this.ship.GetComponent<SpaceController>();
		shipControl.occupant = this;
		
		// Teleport animation
			
		// Disable player object
		this.gameObject.SetActive(false);
		
		// Change cinemachine cam priority
		shipControl.shipCam.Priority = 15;
		
		// Enable ship movement
		PlayerInput shipInput = ship.GetComponent<PlayerInput>();
		shipInput.enabled = true;

		hc.EnterShipHUD();
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
	
	public void OnZoom(InputAction.CallbackContext context)
	{
		zoomInput = context.ReadValue<float>();
	}
	
	public void OnInteract(InputAction.CallbackContext context)
	{
		
		/* Depending on future features this can be made conditional
		 * to call various functions for different sorts of interactions.
		 */
		
		if(ship != null && context.action.triggered)
			BoardShip();
		
	}
	
	
	
}

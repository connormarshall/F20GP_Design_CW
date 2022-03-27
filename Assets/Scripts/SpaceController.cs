using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine;
using Cinemachine;
using VolumetricLines;

public class SpaceController : MonoBehaviour
{
	
	// Input System script of the ship (for toggling)
	PlayerInput input;
	// Mass of the ship to multiply forces byte
	public float mass = 1000f;
	
    // Setting speed related movement speeds for the player ship
    public float thrustSpeed = 500f;
    public float liftSpeed = 100f;                                 //NOTE FOR GROUP: Editable to suit final speed preferences. - love Brendan
    public float strafeSpeed = 200f;
	public float rollSpeed = 200f;
	public float xTurnSpeed = 1000f;
	public float yTurnSpeed = 500f;
	
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
	
	private float mainFireInput = 0f;
	private VolumetricLineBehavior currentLaser;
	private GameObject laser;
	public float asteroidDamage = 10f;
	
	// Rigidbody of ship
	private Rigidbody rb;
	// Occupant(s)
	public CharacterControl occupant;
	// Cinemachine cam for ship
	public CinemachineVirtualCamera shipCam;
	
	// Prefab for laser
	public GameObject laserPrefab;
	// Prefab for spark
	public GameObject sparkParticles;
	// transform for muzzle laser is fired from
	public Transform muzzle;
	// Layer to ignore for laser raycast
	public LayerMask ignoredLayer;

	public GameObject hud;
	private HudController hc;

	// Start is called before the first frame update
	void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		hc = hud.GetComponent<HudController>();

		// Get rigidbody
		rb = GetComponent<Rigidbody>();
		// Get input script
		input = GetComponent<PlayerInput>();
    }
	
	void Update()
	{
		
		// Update laser position
		if(currentLaser != null)
		{
			// If not holding button anymore, stop shooting
			if(mainFireInput < 0.1f)
			{
				AudioSource source = laser.GetComponent<AudioSource>();
				source.mute = true;
				currentLaser.EndPos = new Vector3(0f, 0f, 0f);
				StartCoroutine(DestroyLaser(source));
				return;
			}
			
			RaycastHit hit;
			Vector3 shotExtreme = Camera.main.transform.position + Camera.main.transform.forward * 1000f;
			
			// Camera position moved forward to prevent raycasting behind ship
			Vector3 castPosition = Camera.main.transform.position + Camera.main.transform.forward * 30f;
			
			// If hit, set shot to hit point, else shoot to the point at the extreme end of the laser's range.
			if(Physics.Raycast(castPosition, Camera.main.transform.forward, out hit, 1000f,  ~ignoredLayer))
			{
				currentLaser.EndPos = muzzle.InverseTransformPoint(hit.point) - new Vector3(0f, 0f, 0.2f);
				GameObject.Instantiate(sparkParticles, hit.point, Quaternion.LookRotation(hit.normal));
				
				// If an asteroid is hit, decrease its health
				if(hit.collider.CompareTag("Asteroid"))
				{
					AsteroidBehaviour ast = hit.collider.gameObject.GetComponent<AsteroidBehaviour>();
					ast.DecreaseHealth(asteroidDamage);
				}
				
			} else
				currentLaser.EndPos = muzzle.InverseTransformPoint(shotExtreme);
		}
		
	}
	
	public IEnumerator DestroyLaser(AudioSource source)
	{
		GameObject toDestroy = laser;
		laser = null;
		currentLaser = null;
		
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(toDestroy);
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
			rb.AddRelativeForce(Vector3.forward * thrustInput * thrustSpeed * mass * Time.deltaTime);
			thrustDrift = thrustSpeed;
		} else {
			rb.AddRelativeForce(Vector3.forward * thrustDrift * mass * Time.deltaTime);
			thrustDrift *= deccelerationRate;
		}
		
		// Strafe
		if(strafeInput > 0.1f ||strafeInput < -0.1f)
		{
			rb.AddRelativeForce(Vector3.right * strafeInput * strafeSpeed * mass *  Time.deltaTime);
			strafeDrift = strafeSpeed;
		} else {
			rb.AddRelativeForce(Vector3.right * strafeDrift * mass *  Time.deltaTime);
			strafeDrift *= deccelerationRate;
		}
		
		// Lift
		if(liftInput > 0.1f || liftInput < -0.1f)
		{
			rb.AddRelativeForce(Vector3.up * liftInput * liftSpeed * mass *  Time.deltaTime);
			liftDrift = liftSpeed;
		} else {
			rb.AddRelativeForce(Vector3.up * liftDrift * mass *  Time.deltaTime);
			liftDrift *= deccelerationRate;
		}
		
		// Pitch
		rb.AddRelativeTorque(
			Vector3.right 
			* Mathf.Clamp(-mouseInput.y, -1f, 1f)
			* yTurnSpeed
			* mass
			* Time.deltaTime
		);
		
		// Yaw
		rb.AddRelativeTorque(
			Vector3.up 
			* Mathf.Clamp(mouseInput.x, -1f, 1f)
			* xTurnSpeed
			* mass
			* Time.deltaTime
		);
		
		// Roll
		rb.AddRelativeTorque(
			Vector3.forward 
			* rollInput
			* rollSpeed
			* mass
			* Time.deltaTime
		);
		
	}
	
	void ExitShip()
	{
		// Stop movement
		//this.active = false;
		
		// Teleport animation
			
		// Enable player object
		occupant.gameObject.SetActive(true);
		
		// Change cinemachine cam priority
		shipCam.Priority = 10;
		
		// Disable ship input
		this.input.enabled = false;
		
		
		// Unparent from ship
		occupant.transform.parent = null;
		
		// Set movement active
		occupant.active = true;

		hc.ExitShipHUD();
	}
	
	
	void FireLaser() {
		
		if(laser != null)
			return;
		
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f)) {
			// Create line renderer parented to the muzzle
			laser = GameObject.Instantiate(laserPrefab, muzzle) as GameObject;
			// Update positions to between the muzzle and the raycast
			Vector3 shotExtreme = Camera.main.transform.position + Camera.main.transform.forward * 1000f;
			
			// Camera position moved forward to prevent raycasting behind ship
			Vector3 castPosition = Camera.main.transform.position + Camera.main.transform.forward * 30f;
			
			// Set reference for update method
			currentLaser = laser.GetComponent<VolumetricLineBehavior>();
			
			// Set start point for laser
			currentLaser.StartPos = new Vector3(0f, 0f, 0f);
			
			// If hit, set shot to hit point, else shoot to the point at the extreme end of the laser's range.
			if(Physics.Raycast(castPosition, Camera.main.transform.forward, out hit, 1000f,  ~ignoredLayer))
				currentLaser.EndPos = muzzle.InverseTransformPoint(hit.point);
			else
				currentLaser.EndPos = muzzle.InverseTransformPoint(shotExtreme);

		}
		
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
	
	public void OnEject(InputAction.CallbackContext context)
	{
		if(occupant != null && context.action.triggered)
		{
			ExitShip();
		}
	}
	
	public void OnMainFire(InputAction.CallbackContext context)
	{
		mainFireInput = context.ReadValue<float>();
		if(occupant != null)
			FireLaser();
	}
	
}

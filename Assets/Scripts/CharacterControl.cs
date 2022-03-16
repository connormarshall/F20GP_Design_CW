using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
	
	public float speed = 200f;
	
	public Transform hipTrans;
	public ConfigurableJoint hipJoint;
	public Rigidbody hipRigid;
	
	public Animator targetAnimator;
	private bool moving = false;
	
	public Transform cam;
	
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// Move hips based on inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		
		// Problem with the prefab's pelvis rotation, guy has the mesh rotated wrong.
		// Fix with blender, import the mesh, rotate it right, reimport.
		Vector3 movement = vertical * hipTrans.up + horizontal * -hipTrans.forward;
		
		// If moving
		if(movement.magnitude >= 0.1f)
		{
			this.moving = true;
			this.hipRigid.AddForce(movement.normalized * this.speed);
		
		} else {
			this.moving = false;
		}
		
		// Trigger animations
		
		this.targetAnimator.SetBool("Run", this.moving);
		
		// Rotate to camera
		Quaternion camForward = Quaternion.LookRotation(cam.forward);
		
		this.hipJoint.targetRotation = new Quaternion(
			camForward.y,
			0f,
			0f,
			camForward.w
		);
		
		
			
    }
}

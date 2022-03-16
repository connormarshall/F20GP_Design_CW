using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyToRagdoll : MonoBehaviour
{
	
	public Transform targetLimb;
	public ConfigurableJoint cJoint;
	
	Quaternion targetInitRotation;
	
    // Start is called before the first frame update
    void Start()
    {
		this.cJoint = this.GetComponent<ConfigurableJoint>();
		this.targetInitRotation = this.targetLimb.transform.localRotation;
        
    }

    // Update is called once per physics frame
    void FixedUpdate()
    {
        this.cJoint.targetRotation = copyRotation();
    }
	
	private Quaternion copyRotation() {
		return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitRotation;
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    //Setting speed related movement speeds for the player ship
    public float forwardSpeed = 20f;
    public float hoverSpeed = 4.5f;                                 //NOTE FOR GROUP: Editable to suit final speed preferences. - love Brendan
    public float strafeSpeed = 6f;

    //Stores the input by the user whilst actively moving
    private float activeForwardSpeed; 
    private float activeHoverSpeed;
    private float activeStrafeSpeed;

    private float forwardAcceleration = 2.5f;           //how fast the ship moves from 0-1mph etc
    private float strafeAcceleration = 2f;              //want this to take half a second to accelerate
    private float hoverAcceleration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gets input for axes when moving the ship
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);  //allows for acceleration in the ship
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime); 
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        //applying the input to the ship (movement)
        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime; //always goes forward based on rotation of the ship
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;    //same but for left and right
        transform.position -= transform.up * activeHoverSpeed * Time.deltaTime;   //same but for hovering
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpgateTrigger : MonoBehaviour
{
	
	public int nextSceneNumber = 1;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter(Collider c)
	{
		SceneManager.LoadScene(nextSceneNumber);
	}
}

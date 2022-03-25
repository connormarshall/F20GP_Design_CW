using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{

    public Text altText, hullLabel;
    public RectTransform o2, hull;
	public Image rocket, ammo;
	
	public PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        o2.localScale = new Vector3(playerManager.currentOxygen / playerManager.maxOxygen, 1f, 1f);
    }

    void ShowHelpWindow()
    {

    }

    void HideHelpWindow()
    {

    }

    // Update is called once per frame
    void Update()
    {
        o2.localScale = new Vector3(playerManager.currentOxygen / playerManager.maxOxygen, 1f, 1f);
    }
}

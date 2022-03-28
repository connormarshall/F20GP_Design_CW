using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HudController : MonoBehaviour
{

    public Text altText, hullLabel, helpText;
    public RectTransform o2, hull;
	public Image rocket, ammo, hullBG, hullFG, helpWindow;
	
	public PlayerManager playerManager;

    private Color32 activeColour = new Color32(255, 255, 255, 255);
    private Color32 inactiveColour = new Color32(0, 0, 0, 255);
	
	public bool showHelp = true;

    // Start is called before the first frame update
    void Start()
    {
        o2.localScale = new Vector3(playerManager.currentOxygen / playerManager.maxOxygen, 1f, 1f);
        altText.text = "0";

        ExitShipHUD();
        HideHelpWindow();
    }

    public void EnterShipHUD()
    {
        hullBG.enabled = true;
        hullFG.enabled = true;
        hullLabel.enabled = true;

        altText.text = "100";
        rocket.color = activeColour;
        ammo.color = activeColour;
    }

    public void ExitShipHUD()
    {
        hullBG.enabled = false;
        hullFG.enabled = false;
        hullLabel.enabled = false;

        altText.text = "0";
        rocket.color = inactiveColour;
        ammo.color = inactiveColour;
    }

    public void ShowHelpWindow()
    {
        helpText.enabled = true;
        helpWindow.enabled = true;
    }

    public void HideHelpWindow()
    {
        helpText.enabled = false;
        helpWindow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        o2.localScale = new Vector3(playerManager.currentOxygen / playerManager.maxOxygen, 1f, 1f);
		hull.localScale = new Vector3(playerManager.currentHull / playerManager.maxHull, 1f, 1f);

        if (showHelp)
        {
            ShowHelpWindow();
        }
        else
        {
            HideHelpWindow();
        }
    }
	
	public void toggleHelp(InputAction.CallbackContext context)
	{
		if(context.action.triggered)
			showHelp = !showHelp;
	}
	
}

using UnityEngine;
using System.Collections;

public class DisplayGUIMessage : MonoBehaviour 
{
	private ControllablePlayer player;

	private string message;
	private string buttonMessage = "RESPAWN";

	private float topLeftX = Screen.width / 4;
	private float topLeftY = Screen.height / 4;
	private float boxWidth = Screen.width / 2;
	private float boxHeight = Screen.height / 2;

	private float buttonWidth = 100f;
	private float buttonHeight = 30f;
	private float buttonX;
	private float buttonY;

	void Awake()
	{
		buttonX = (Screen.width / 2) - buttonWidth;
		buttonY = (Screen.width / 2) - buttonHeight;
	}

	//sets the message to be displayed
	public void SetMessage(string messageToShow)
	{
		message = messageToShow;
	}

	//displays the proper message
	void OnGUI()
	{
		if(player.GetDead())
		{
			GUI.Box(new Rect(topLeftX, topLeftY, boxWidth, boxHeight),
			        new GUIContent(message));

			if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight),
			               buttonMessage))
				player.Respawn();
		}
	}

}

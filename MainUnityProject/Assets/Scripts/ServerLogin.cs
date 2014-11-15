using UnityEngine;
using System.Collections;

public class ServerLogin : MonoBehaviour 
{
	private const int MAX_LENGTH = 25;
	private const int MAIN_LEVEL_INDEX = 1;

	private string login = "Enter username";
	private string password = "Enter password";
	private string buttonMessage = "Login";

	private float topLeftX = Screen.width / 4;
	private float topLeftY = Screen.height / 4;
	private float boxWidth = Screen.width / 2;
	private float boxHeight = Screen.height / 2;

	private float textFieldWidth = 200f;
	private float textFieldHeight = 25f;
	private float textFieldX;
	private float buffer = 5f;
	private float loginY;
	private float passwordY;

	private float buttonWidth = 75f;
	private float buttonHeight = 30f;
	private float buttonX;
	private float buttonY;

	private string confirmBoxMessage = "Account not found. \nDo you want to create an account?";
	private float confirmBoxX;
	private float confirmBoxY;
	private float confirmBoxHeight;
	private float confirmBoxWidth;

	private float confirmButtonsWidth = 50f;
	private float confirmButtonsHeight = 30f;
	private float confirmButtonsY;
	private float yesButtonX;
	private float denyButtonX;
	private float buttonBuffer = 5f;

	private bool showNewAccount = false;
	private bool firstBoxEnabled = true;
	private bool secondBoxEnabled = false;

	void Awake()
	{
		textFieldX = (Screen.width / 2) - (textFieldWidth / 2);
		loginY = (Screen.height / 2) - textFieldHeight - buffer;
		passwordY = loginY + textFieldHeight + buffer;

		buttonX = (Screen.width / 2) - (buttonWidth / 2);
		buttonY = passwordY + textFieldHeight + (2 * buffer); 

		confirmBoxWidth = Screen.width / 4;
		confirmBoxHeight = Screen.height / 4;
		confirmBoxX = (Screen.width / 2) - (confirmBoxWidth / 2);
		confirmBoxY = (Screen.height / 2) - (confirmBoxHeight / 2);

		confirmButtonsY = (Screen.height / 2);
		yesButtonX = (Screen.width / 2) - confirmButtonsWidth;
		denyButtonX = yesButtonX + confirmButtonsWidth + buttonBuffer;
	}

	// Update is called once per frame
	void OnGUI () 
	{
		GUI.enabled = firstBoxEnabled;
		GUI.Box(new Rect(topLeftX, topLeftY, boxWidth, boxHeight),
		        new GUIContent("Please enter your server login info"));

		login = GUI.TextField (new Rect (textFieldX, loginY, textFieldWidth, textFieldHeight), 
		                      login, MAX_LENGTH);
		password = GUI.TextField (new Rect (textFieldX, passwordY, textFieldWidth, textFieldHeight), 
		                          password, MAX_LENGTH);

		if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight),
		               buttonMessage))
		{
			if (!confirmAccount(login, password))
			{
				showNewAccount = true;
				firstBoxEnabled = false;
				secondBoxEnabled = true;
			}
		}

		if (showNewAccount)
		{
			Color bgColor = Color.black;
			//bgColor.a = 100;
			GUI.backgroundColor = bgColor;
			GUI.contentColor = Color.cyan;

			GUI.enabled = secondBoxEnabled;
			GUI.Box(new Rect(confirmBoxX, confirmBoxY, confirmBoxWidth, confirmBoxHeight),
			        new GUIContent(confirmBoxMessage));

			if (GUI.Button(new Rect(yesButtonX, confirmButtonsY, 
			                        confirmButtonsWidth, confirmButtonsHeight),
			               "YES"))
			{
				Application.LoadLevel(MAIN_LEVEL_INDEX);
			}
			
			if (GUI.Button(new Rect(denyButtonX, confirmButtonsY, 
			                        confirmButtonsWidth, confirmButtonsHeight),
			               "NO"))
			{
				secondBoxEnabled = false;
				firstBoxEnabled = true;
				showNewAccount = false;
			}
		}
	}

	//determine if account exists in server -- temporarily set to false for testing
	bool confirmAccount(string login, string password)
	{
		return false;
	}
}

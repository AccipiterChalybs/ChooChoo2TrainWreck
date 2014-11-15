using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public string[] buttonList;
	public ControllablePlayer currentPlayer; //player only
	public NetworkManager networkManager;

	/* Search for input on update by calling helper function*/
	void Update() {
		CheckInput ();
	}
	
	/* (P only) Checks for input */
	void CheckInput() {
		foreach (string button in buttonList) {
			InputAction action = new InputAction();
			action.buttonName = button;
			action.value = Input.GetAxis(button);
			ApplyInput (currentPlayer, action);
		}
		//networkManager.send
	}

	
	/* (P only) Applies Input to connected player */
	void ApplyInput(ControllablePlayer player, InputAction action) {
		player.AddMove(action);
	}
	
	/* packages needed input to client_SendInput() */
	void SendInput();
}

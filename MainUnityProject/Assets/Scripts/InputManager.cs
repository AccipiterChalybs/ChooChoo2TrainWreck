using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	/* Search for input on update by calling helper function*/
	void Update();    
	
	/* (P only) Checks for input */
	void CheckInput();
	
	/* (P only) Applies Input to connected player */
	void ApplyInput();
	
	/* packages needed input to client_SendInput() */
	void SendInput();
}

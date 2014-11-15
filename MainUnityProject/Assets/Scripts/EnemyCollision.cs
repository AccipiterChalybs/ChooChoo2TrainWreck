using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour 
{
	public DisplayGUIMessage message;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			ControllablePlayer player = collision.gameObject.GetComponent<ControllablePlayer>();
			message.SetMessage("YOU ARE DEAD. WAY TO GO");
			player.Kill ();
		}
	}
}

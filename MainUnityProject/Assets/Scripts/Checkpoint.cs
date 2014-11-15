using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			ControllablePlayer player = collision.gameObject.GetComponent<ControllablePlayer>();
			player.SetCheckpoint(transform.position);
		}
	}
}
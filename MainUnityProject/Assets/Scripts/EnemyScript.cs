using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public enum Direction{Left, Right, Forward, Backward, Up, Down};
	public Direction moveDirection;
	public float endValue;
	public float delay;
	public float speed;

	private bool isMoving;
	private bool respawning;
	private float respawnTimer;
	private Vector3 respawnLocation;

	// Use this for initialization
	void Start () 
	{
		respawnLocation = transform.position;
		respawnTimer = delay; //allow moving when game first starts
	}

	void FixedUpdate () 
	{
		MoveEnemy ();
	}

	void MoveEnemy()
	{
		switch(moveDirection)
		{
		case Direction.Left:
			if (transform.position.x > endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Right:
			if (transform.position.x < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Forward:
			if (transform.position.z < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Backward:
			if (transform.position.z > endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Up:
			if (transform.position.y < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Down:
			if (transform.position.y > endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		default:
			print ("unimplemented move");
			break;
		}
		
		if (isMoving && respawnTimer >= delay)
		{
			transform.Translate (Vector3.forward * Time.deltaTime * speed, Space.Self);
		}
		else
		{
			transform.position = respawnLocation;
			respawnTimer += Time.deltaTime;
		}
	}
}

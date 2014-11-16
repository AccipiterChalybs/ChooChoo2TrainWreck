using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public enum Direction{Left, Right, Forward, Backward, Up, Down};
	public Direction moveDirection;
	public float endValue;
	public float delay; //respawn delay
	public float speed;

	private bool isMoving;
	private Vector3 moveVector;
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
			moveVector = new Vector3(-1,0,0);
			if (transform.position.x > endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Right:
			moveVector = new Vector3(1,0,0);
			if (transform.position.x < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Forward:
			moveVector = new Vector3(0,0,1);
			if (transform.position.z < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Backward:
			moveVector = new Vector3(0,0,-1);
			if (transform.position.z > endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Up:
			moveVector = new Vector3(0,1,0);
			if (transform.position.y < endValue)
				isMoving = true;
			else
			{
				isMoving = false;
				respawnTimer = 0.0f;
			}
			break;
		case Direction.Down:
			moveVector = new Vector3(0,-1,0);
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
			transform.Translate(moveVector * Time.deltaTime * speed, Space.World);
		}
		else
		{
			transform.position = respawnLocation;
			respawnTimer += Time.deltaTime;
		}
	}
}

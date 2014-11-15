using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	public enum Direction{Left, Right, Forward, Backward, Up, Down};
	public Direction moveDirection;
	public float endValue;
	public float delay;

	private bool isMoving;
	private Vector3 respawnLocation;

	// Use this for initialization
	void Start () 
	{
		respawnLocation = (transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		moveEnemy ();
	}

	void moveEnemy()
	{
		switch(moveDirection)
		{
		case Direction.Left:
			if (transform.position.x > endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		case Direction.Right:
			if (transform.position.x < endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		case Direction.Forward:
			if (transform.position.z < endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		case Direction.Backward:
			if (transform.position.z > endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		case Direction.Up:
			if (transform.position.y < endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		case Direction.Down:
			if (transform.position.y > endValue)
				isMoving = true;
			else
				isMoving = false;
			break;
		default:
			print ("unimplemented move");
			break;
		}
		
		if (isMoving)
		{
			transform.Translate (Vector3.forward * Time.deltaTime, Space.Self);
		}
		else
		{
			transform.position = respawnLocation;
			StartCoroutine(WaitForRespawn());
		}
	}

	IEnumerator WaitForRespawn() 
	{
		yield return new WaitForSeconds(delay);
	}

}

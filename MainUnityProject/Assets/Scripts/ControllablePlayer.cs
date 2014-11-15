using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllablePlayer : MonoBehaviour {
	public enum Effect {Slow, NoMove, Stun};
	public float baseSpeed; //hops / second
	public float jumpHeight;
	public InputAction mCurrentAction;
	//private auto lastUpdateTime;
	private Vector3 actualPosition; //non-visual position
	private Vector3 destination;
	private Vector3 destinationUp;
	private Vector3 midpoint;
	private Vector3 prevUp;
	private float moveTime;
	public float minDistance = 0.025f;
	private bool isMoving;

	private RaycastHit hitInfo;
	private float lastActionTime=0;
	private float inputDelay = 0.1f;


	/* Called when object is created, like ctor 
	     * Leave this alone for now. */
	private Vector3 lastCheckpoint;
	
	public void SetCheckpoint(Vector3 newCheckpoint)
	{
		lastCheckpoint = newCheckpoint;
	}
	
	/* Called when object is created, like ctor 
	     * Leave this alone for now. */
	void Start()
	{
		destination = transform.position;
		//set gravity for ALL RIGIDBODIES ON SCREEN
		Physics.gravity = new Vector3 (0, -9.81f, 0);
	}

	/* Called every frame.
	     *  Handle updating player's position & other visual changes, as these
	     *  need to be done each frame to avoid jerky-ness. However, actions
	     *  should *not* be done here, as otherwise low FPS might lead to more
	     *  de-synchonization other the internet. */
	void Update() {
	}

	/* Called on current Player - moves player from actual position by
	     * ([CurrentTime] - lastUpdateTime) */
	void extrapolatePlayer() {
	}

	/* Called on other players, interpolates between snapshots */
	void interpolatePlayer(){
	}

	/* Called at a certain rate every second.
	     *  Handle actions and other gameplay stuff here 
	     *  (like decrementing timers), as this should coordinate
	     *  better with the server, as it will be more deterministic. */
	void FixedUpdate() {
		if (isMoving && Vector3.Distance (transform.position, destination) < minDistance) {
			Snap ();
			CheckFall();
		}
		if (!isMoving) {
			HandleAction ();
		}
		MovePlayer ();
	}

	void Snap() {
		transform.position = destination;
		isMoving = false;
		
		mCurrentAction.buttonName = null;
	}

	void CheckFall() {
		if (!RaycastCheck(transform.position + 0.25f * Vector3.up, -1 * Vector3.up, 1)) {
			destination = transform.position - 1*Vector3.up;
			prevUp = Vector3.up;
			
			moveTime = 0;
			isMoving = true;
			destinationUp = Vector3.up;
		}
	}

	/* Handles a new incoming action, by determining if/where to move
	     * (using checkMove), then sends this action to the server, then
	     * moves queuedAction up and sets it to null */
	//keyboard commands
	void HandleAction()
	{
		Vector3 vecMove = Vector3.zero;
		if (mCurrentAction.buttonName != null) {
			switch (mCurrentAction.buttonName) {
				case "Left":
					vecMove = new Vector3 (-(mCurrentAction.value), 0, 0);
				break;
				case "Right":
					vecMove = new Vector3 ((mCurrentAction.value), 0, 0);
					break;
				case "Up":
					vecMove = new Vector3 (0, 0, mCurrentAction.value);
				break;
				case "Down":
					vecMove = new Vector3 (0, 0, -mCurrentAction.value);
					break;
				default:
					print ("unimplemented control");
					break;
			}
			if (vecMove != Vector3.zero && this.CheckMove (vecMove)) {
			//		midpoint = transform.position + (vecMove) / 2.0f + Vector3.up * jumpHeight;
					destination = transform.position + (vecMove);
					if (Physics.Raycast(transform.position+0.9f*Vector3.up+vecMove, -1*Vector3.up, out hitInfo, 2f)) {
						destinationUp = hitInfo.normal;
						destination.y += 0.5f;
					} else {
						destinationUp = Vector3.up;
					}
					prevUp = Vector3.up;
				
					moveTime = 0;
					isMoving = true;
			} else {
				mCurrentAction.buttonName = null;
			}
		}
	}

	/* Moves the player's actual position, sets this.transform to it */
	void MovePlayer() {
		if (isMoving) {
			//	transform.position += Vector3.Normalize(midpoint - transform.position) * baseSpeed * Time.deltaTime;
			transform.position += Vector3.Normalize(destination - transform.position) * baseSpeed * Time.deltaTime;
			moveTime += Time.deltaTime * baseSpeed;
			transform.up = Vector3.Lerp(prevUp, destinationUp, moveTime);
		}
	}

	/* Determine what Raycast we need to do to see if an obstacle is in the
	     * way. */
	//offset = where player wants to go
	//calls raycastCheck to make sure move is possible
	bool CheckMove(Vector3 offset)
	{
		Vector3 start = transform.position+Vector3.up*0.75f;

		return !RaycastCheck (start, offset, 1);
	}

	/* Start at a point, Raycast in Dir, return false if it hits an obstacle */
	bool RaycastCheck(Vector3 start, Vector3 dir, float distance)
	{
		return Physics.Raycast (start, dir, distance);
	}

	/* Change each timer by Time.deltaTime */
	void FixedUpdateTimers() {
	}

	/* Send a move that this player should perform
	     * set mCurrentAction to it if empty, else overrides mQueuedAction */
	public void AddMove(InputAction newAction)
	{
		if (Time.time - lastActionTime > inputDelay) {
			lastActionTime = Time.time;
			if (mCurrentAction.buttonName == null)
				mCurrentAction = newAction;
		}
	}

	/* P only
	     * Called by the server when this player gets hit */
	void Incap(){}

	/* Moves Player back to respawn position */
	void Respawn( Vector3 Position ){}

	/* For Later: Allows objects to apply effects on player */
	void ApplyEffect(Effect e, int value){}
}

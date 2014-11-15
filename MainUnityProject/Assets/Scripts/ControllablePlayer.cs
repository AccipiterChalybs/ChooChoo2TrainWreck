using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllablePlayer : MonoBehaviour {
	public enum Effect {Slow, NoMove, Stun};
	public float baseSpeed; //hops / second
	public InputAction mCurrentAction;

	private InputAction mQueuedAction;
	//private auto lastUpdateTime;
	private Vector3 actualPosition; //non-visual position
	private Vector3 destination;
	private Vector3 midpoint;
	private Vector3 lastCheckpoint;
	private bool dead = false;
	
	public void SetCheckpoint(Vector3 newCheckpoint)
	{
		lastCheckpoint = newCheckpoint;
	}

	public bool GetDead()
	{
		return dead;
	}

	public void Kill()
	{
		dead = true;
	}

	/* Moves Player back to respawn position */
	public void Respawn( )
	{
		dead = false;
		transform.position = lastCheckpoint;
	}
	
	/* Called when object is created, like ctor 
	     * Leave this alone for now. */
	void Start()
	{
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
		if (!dead)
		{
			HandleAction ();
		}
	}

	/* Handles a new incoming action, by determining if/where to move
	     * (using checkMove), then sends this action to the server, then
	     * moves queuedAction up and sets it to null */
	//keyboard commands
	void HandleAction()
	{
		if (mCurrentAction.buttonName != null) {
						switch (mCurrentAction.buttonName) {
						case "Horizontal":
								Vector3 horizMove = new Vector3 (mCurrentAction.value, 0, 0);
								if (this.CheckMove (horizMove)) {
										transform.Translate (horizMove);
								}
								break;
						case "Vertical":
				Vector3 vertMove = new Vector3 (0, 0, mCurrentAction.value);
								if (this.CheckMove (vertMove)) {
										transform.Translate (vertMove);
								}
								break;
						default:
								print ("unimplemented control");
								break;
						}

						mCurrentAction = mQueuedAction;
						mQueuedAction.buttonName = null;
				}
	}

	/* Moves the player's actual position, sets this.transform to it */
	void MovePlayer(){
	}

	/* Determine what Raycast we need to do to see if an obstacle is in the
	     * way. */
	//offset = where player wants to go
	//calls raycastCheck to make sure move is possible
	bool CheckMove(Vector3 offset)
	{
		Vector3 start = transform.position;

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
		if (mCurrentAction.buttonName == null)
			mCurrentAction = newAction;
		else
			mQueuedAction = newAction;
	}

	/* P only
	     * Called by the server when this player gets hit */
	void Incap(){}

	/* For Later: Allows objects to apply effects on player */
	void ApplyEffect(Effect e, int value){}
}

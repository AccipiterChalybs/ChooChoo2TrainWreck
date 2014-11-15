using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllablePlayer : MonoBehaviour {
	public enum Effect {Slow, NoMove, Stun};
	public float baseSpeed; //hops / second
	private InputAction mCurrentAction;
	private InputAction mQueuedAction;
	//private auto lastUpdateTime;
	private Vector3 actualPosition; //non-visual position
	private Vector3 destination;
	private Vector3 midpoint;


	/* Called when object is created, like ctor 
	     * Leave this alone for now. */
	void Start();    

	/* Called every frame.
	     *  Handle updating player's position & other visual changes, as these
	     *  need to be done each frame to avoid jerky-ness. However, actions
	     *  should *not* be done here, as otherwise low FPS might lead to more
	     *  de-synchonization other the internet. */
	void Update();

	/* Called on current Player - moves player from actual position by
	     * ([CurrentTime] - lastUpdateTime) */
	void extrapolatePlayer(); 

	/* Called on other players, interpolates between snapshots */
	void interpolatePlayer();

	/* Called at a certain rate every second.
	     *  Handle actions and other gameplay stuff here 
	     *  (like decrementing timers), as this should coordinate
	     *  better with the server, as it will be more deterministic. */
	void FixedUpdate();

	/* Handles a new incoming action, by determining if/where to move
	     * (using checkMove), then sends this action to the server, then
	     * moves queuedAction up and sets it to null */
	//keyboard commands
	void HandleAction()
	{
		switch (mCurrentAction.buttonName)
		{
			case "Horizontal":
				Vector3 horizMove = new Vector3(mCurrentAction.value, 0, 0);
				if(this.CheckMove(horizMove))
				{
					transform.Translate(horizMove);
				}
			case "Vertical":
				Vector3 vertMove = new Vector3(0, mCurrentAction.value, 0);
				if(this.CheckMove(vertMove))
				{
					transform.Translate(vertMove);
				}
			default:
				print("unimplemented control");
		}

		mCurrentAction = mQueuedAction;
		mQueuedAction = null;
	}

	/* Moves the player's actual position, sets this.transform to it */
	void MovePlayer();

	/* Determine what Raycast we need to do to see if an obstacle is in the
	     * way. */
	//offset = where player wants to go
	//calls raycastCheck to make sure move is possible
	bool CheckMove(Vector3 offset)
	{
		Vector3 start = transform.position;

		return RaycastCheck (start, offset);
	}

	/* Start at a point, Raycast in Dir, return false if it hits an obstacle */
	bool RaycastCheck(Vector3 start, Vector3 dir, float distance)
	{
		return Physics.Raycast (start, dir, distance);
	}

	/* Change each timer by Time.deltaTime */
	void FixedUpdateTimers();

	/* Send a move that this player should perform
	     * set mCurrentAction to it if empty, else overrides mQueuedAction */
	void AddMove(InputAction newAction)
	{
		if (mCurrentAction == null)
			mCurrentAction = newAction;
		else
			mQueuedAction = newAction;
	}

	/* P only
	     * Called by the server when this player gets hit */
	void Incap();

	/* Moves Player back to respawn position */
	void Respawn( Vector3 Position );

	/* For Later: Allows objects to apply effects on player */
	void ApplyEffect(Effect e, int value);
}

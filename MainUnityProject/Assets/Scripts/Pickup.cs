using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
	public enum Effect{SPEED};
	public Effect powerType;
	public float speedIncrease = 10f;
	public float powerupDuration = 10f; //the amount of time powerups last

	private float powerupTimer = 0f; //the time left for the powerup's effect
	private bool speedIsIncreased = false;

	private ControllablePlayer player;

	/* should just call onPickup, which will be overriden for each powerup */
	//Make sure to distinguish which player picks it up
	void OnCollisionEnter( Collision collision)
	{
		if(collision.gameObject.GetComponent<ControllablePlayer>() != null)
		{
			//set the player for future reference
			player = collision.gameObject.GetComponent<ControllablePlayer>();

			//calling on pickup on this passing a referance to controllable player	
			this.onPickup (player);
			this.collider.enabled = false;
			this.renderer.enabled = false;
		}
	}

	void Update()
	{
		powerupTimer += Time.deltaTime;

		//if out of powerupDuration, then turn off powerup and display the powerup oncsreen again
		if (powerupTimer >= powerupDuration)
		{
			//take away the speed boost
			if (speedIsIncreased)
			{
				speedIsIncreased = false;
				player.SetSpeed(-speedIncrease);
				this.collider.enabled = true;
				this.renderer.enabled = true;
			}
		}

	}

	void onPickup(ControllablePlayer player)
	{
		switch (powerType)
		{
		case Effect.SPEED:
			player.SetSpeed(speedIncrease);
			powerupTimer = 0f;
			speedIsIncreased = true;
			break;
		default:
			print("that powerup is unavailable");
			break;
		}

		//star
			//immunity for 10 seconds
				//if there is a talent system this can either 
					//increase to 20 seconds or 
					//have 90 seconds of projectile protection or
					
					// more common drop timer (something to affect type of drops given
		//shield
			//absorbs a certain amount of damage
				//similair to star can be better or worse
			
		//wind, blows away enemies

		//magnet, pulls power ups 3 blocks away towards frogger

		//Hearts finish the level, collecting different types 
			//froggers share heart pieces
			
			//are heart pieces randomly generated or not?
				//if heart pieces have fixed locations do the types of heart pieces at different locations change?

			

		//Movements speed increase?, speed up the jump speed.
			//this would increase the speed after ray is called

		//
	}
}

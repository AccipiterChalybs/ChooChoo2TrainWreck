using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	/* should just call onPickup, which will be overriden for each powerup */
	//Make sure to distinguish which player picks it up
	void OnCollisionEnter( Collision obj)
	{
		/* TODO: This is causing some error?
		if(obj.gameObject.GetComponent<ControllablePlayer> != null){
				this.onPickup (obj.gameObject.GetComponent<ControllablePlayer> ());
			//calling on pickup on this passing a referance to controllable player
			}*/
	}
	void onPickup()
	{
		//needs to find the identity of the pickupable object/power up
		//obj.gameObject.GetName<Pickup object>

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

//	void OnCollisionEnter( Collision obj);

//	void onPickup();
	
//	void OnCollisionEnter( Collision obj);

//	void onPickup();

//	void OnCollisionEnter( Collision obj);

//	void onPickup();
}

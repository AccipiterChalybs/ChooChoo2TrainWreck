using UnityEngine;
using System;

public class PlayerData
{
	public string username;
	public int score;
	public float position_x;
	public float position_y;
	public float position_z;

	// Username constructor
	public PlayerData () {
		this.username = "";
		score = 0;
		position_x = 0;
		position_y = 0;
		position_z = 0;
	}

	// Parametrized constructor
	public PlayerData ( string username, int score, float x, float y, float z ) {
		this.username = username;
		this.score = score;
		this.position_x = x;
		this.position_y = y;
		this.position_z = z;
	}
}
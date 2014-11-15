using UnityEngine;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public class AccountManager : MonoBehaviour {
	/* This class should perform high level functions interacting with SQL database */

	public string username; // SQL username
	public string password; // SQL password
	public string email;	// SQL email
	public SQLManager sqlMan;
	public SqlConnection myConnection;

	bool CheckLogin() {

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection

		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, 50);
		usernameParam.Value = username;

		string commandString = "SELECT password FROM users WHERE username = '@usernameVal'";

		//User provides username, and password
		SqlCommand  checkCommand = new SqlCommand (commandString, myConnection);
		checkCommand.Parameters.Add (usernameParam);

		// Execute the query
		SqlDataReader reader = checkCommand.ExecuteReader ();

		// Check if we got results
		if (reader.Read ()) {
			if ( reader["password"].ToString().Equals(HashPass (password)) ) {
				// Password is correct
				myConnection.Close ();
				return true;
			}
		}

		// Close the Connection
		myConnection.Close ();

		return false;
	}
	
	bool NewAccount() {

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection
		
		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, 50);
		usernameParam.Value = username;

		// Parametrized email as @emailVal VarChar size 50
		SqlParameter emailParam = new SqlParameter("@emailVal", SqlDbType.VarChar, 50);
		emailParam.Value = email;

		// Parametrized password as @passwordVal VarChar size 100
		SqlParameter passwordParam = new SqlParameter("@passwordVal", SqlDbType.VarChar, 100);
		// Needs to hash
		passwordParam.Value = HashPass (password);

		string commandString = "INSERT INTO users (username, email, password) " + 
								"VALUES ('@usernameVal','@passwordVal','@emailVal');";
		
		//User provides username, and password
		SqlCommand  registerCommand = new SqlCommand (commandString, myConnection);
		registerCommand.Parameters.Add (usernameParam);
		registerCommand.Parameters.Add (emailParam);
		registerCommand.Parameters.Add (passwordParam);

		// ExecuteNonQuery returns # of rows affected
		if ( registerCommand.ExecuteNonQuery () != 0 ) {
			//Registration Success
			myConnection.Close ();
			return true;
		}

		// Close the Connection
		myConnection.Close ();
		return false;
	}
	
	PlayerData getPlayerData( string user_name ) {

		PlayerData data = new PlayerData(); // Return value

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection

		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, 50);
		usernameParam.Value = user_name;

		string commandString = "SELECT * FROM userData WHERE username = '@usernameVal';";
		
		//User provides username, and password
		SqlCommand  dataCommand = new SqlCommand (commandString, myConnection);
		dataCommand.Parameters.Add (usernameParam);

		// Execute the query
		SqlDataReader reader = dataCommand.ExecuteReader ();
		
		// Check if we got results
		if (reader.Read ()) {

			data.username = reader["username"].ToString();
			data.score = Convert.ToInt32 (reader["score"].ToString ());
			data.position_x = Convert.ToSingle (reader["position_x"].ToString());
			data.position_y = Convert.ToSingle (reader["position_y"].ToString());
			data.position_z = Convert.ToSingle (reader["position_z"].ToString());

			// Success
			return data;
		}

		// Fail
		// How to check if failed? Just check if username is "" (empty).
		return data;
	}

	string HashPass(string password)
	{
		SHA256 sha = new SHA256CryptoServiceProvider();
		
		//compute hash from the bytes of text
		sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password + email));
		
		//get hash result after compute it
		byte[] result = sha.Hash;
		
		StringBuilder strBuilder = new StringBuilder();
		for (int i = 0; i < result.Length; i++)
		{
			//change it into 2 hexadecimal digits
			//for each byte
			strBuilder.Append(result[i].ToString("x2"));
		}
		
		return strBuilder.ToString();
	}
}
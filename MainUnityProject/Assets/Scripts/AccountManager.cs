using UnityEngine;
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
		usernameParam = username;

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
				return true;
			}
			else {
				// Password is incorrect
				return false;
			}
		}

		
		// Close the Connection
		myConnection.Close ();
	}
	
	bool NewAccount() {

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection
		
		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, 50);
		usernameParam = username;

		// Parametrized email as @emailVal VarChar size 50
		SqlParameter emailParam = new SqlParameter("@emailVal", SqlDbType.VarChar, 50);
		emailParam = email;

		// Parametrized password as @passwordVal VarChar size 100
		SqlParameter passwordParam = new SqlParameter("@passwordVal", SqlDbType.VarChar, 100);
		// Needs to hash
		passwordParam = HashPass (password);

		string commandString = "INSERT INTO users (username, email, password) " + 
								"VALUES ('@usernameVal','@passwordVal','@emailVal');";
		
		//User provides username, and password
		SqlCommand  registerCommand = new SqlCommand (commandString, myConnection);
		registerCommand.Parameters.Add (usernameParam);
		registerCommand.Parameters.Add (emailParam);
		registerCommand.Parameters.Add (passwordParam);


		if (registerCommand.ExecuteNonQuery ()) {
			//Registration Success
			return true;
		}
		else {
			// Registration Failure
			return false;
		}

		// Close the Connection
		myConnection.Close ();
	}
	
	PlayerData getPlayerData( string user_name ) {

		PlayerData data; // Return value

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection

		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, 50);
		usernameParam = user_name;

		string commandString = "SELECT * FROM userData WHERE username = '@usernameVal';";
		
		//User provides username, and password
		SqlCommand  dataCommand = new SqlCommand (commandString, myConnection);
		dataCommand.Parameters.Add (usernameParam);

		// Execute the query
		SqlDataReader reader = dataCommand.ExecuteReader ();
		
		// Check if we got results
		if (reader.Read ()) {

			data.username = reader["username"].ToString();
			data.score = reader["score"];
			data.position_x = reader["position_x"];
			data.position_y = reader["position_y"];
			data.position_z = reader["position_z"];

			// Success
			return data;
		}

		// Fail
		return 0;
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
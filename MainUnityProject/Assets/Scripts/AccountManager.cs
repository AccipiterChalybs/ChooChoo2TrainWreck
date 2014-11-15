using UnityEngine;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class AccountManager : MonoBehaviour {
	/* This class should perform high level functions interacting with SQL database */

	// Length of VARCHAR in SQL DB
	public const int USERNAME_LENGTH = 50;
	public const int EMAIL_LENGTH = 50;
	public const int PASS_LENGTH = 100;

	// Detailed Success and Error strings
	public const string SUCCESS_STR = "Success.";
	public const string USERNAME_ERR_STR = "Username contains invalid characters. Must be 4-10 characters long.";
	public const string EMAIL_ERR_STR = "Please enter a valid email address.";
	public const string PASS_ERR_STR = "Password must be between 4-50 characters.";
	public const string CREATE_ACC_ERR_STR = "Unable to create a new account.";
	public const string LOGIN_ERR_STR = "Invalid login credentials";

	// Fields the user input
	public string username; // SQL username
	public string password; // SQL password
	public string email;	// SQL email

	// SQL use objects
	public SQLManager sqlMan;
	public SqlConnection myConnection;

	// Returns a string indicating success/failure: SUCCES_STR, USERNAME_ERR_STR, etc...
	String CheckLogin() {

		// Check validity
		string fieldValidityStatus = checkUsername (user_name);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid username!
			return fieldValidityStatus;
		}
		
		fieldValidityStatus = checkEmail (email);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid email!
			return fieldValidityStatus;
		}
		
		fieldValidityStatus = checkPassword (password);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid password!
			return fieldValidityStatus;
		}

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
				return SUCCESS_STR;
			}
		}

		// Close the Connection
		myConnection.Close ();

		return LOGIN_ERR_STR;
	}
	
	String NewAccount() {

		string fieldValidityStatus = checkUsername (user_name);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid username!
			return fieldValidityStatus;
		}

		fieldValidityStatus = checkEmail (email);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid email!
			return fieldValidityStatus;
		}

		fieldValidityStatus = checkPassword (password);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid password!
			return fieldValidityStatus;
		}

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection
		
		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, USERNAME_LENGTH);
		usernameParam.Value = username;

		// Parametrized email as @emailVal VarChar size 50
		SqlParameter emailParam = new SqlParameter("@emailVal", SqlDbType.VarChar, EMAIL_LENGTH);
		emailParam.Value = email;

		// Parametrized password as @passwordVal VarChar size 100
		SqlParameter passwordParam = new SqlParameter("@passwordVal", SqlDbType.VarChar, PASS_LENGTH);
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
			return SUCCESS_STR;
		}

		// Close the Connection
		myConnection.Close ();
		return CREATE_ACC_ERR_STR;
	}
	
	PlayerData getPlayerData( string user_name ) {

		string fieldValidityStatus = checkUsername (user_name);
		if (!fieldValidityStatus.Equals (SUCCESS_STR)) {
			// Invalid username! Unimplemented!
		}

		PlayerData data = new PlayerData(); // Return value

		// Set up a SQLManager and get a connection
		sqlMan = new SQLManager ();
		myConnection = sqlMan.Start (); // Opens connection

		// Parametrized username as @usernameVal VarChar size 50
		SqlParameter usernameParam = new SqlParameter("@usernameVal", SqlDbType.VarChar, USERNAME_LENGTH);
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

	//Returns a string that indicates either success or error in some way
	String checkUsername (string username) {
		
		// Allowed usernames length 4-10 characters long, A-Z enforced first char, A-Z and 0-9 rest of chars, no symbols
		Regex usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{3,9}$");

		// Check username
		Match usernameMatch = usernameRegex.Match(username);
		if (!usernameMatch.Success) {
			return USERNAME_ERR_STR;
		}

		//Everything else is correct
		return SUCCESS_STR;
	}

	string checkPassword ( string password ) {
		// Check password (just checking the length, has to be between 4-50 characters)
		if ((password.Length < 4) || (password.Length > 50)) {
			return PASS_ERR_STR;
		}

		//Everything else is correct
		return SUCCESS_STR;
	}

	string checkEmail ( string email ) {
		Regex emailRegex = new Regex (@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
		
		// Check email
		if (! Regex.IsMatch (emailString, emailRegex, RegexOptions.IgnoreCase)) {
			return EMAIL_ERR_STR;
		}
		
		//Everything else is correct
		return SUCCESS_STR;
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
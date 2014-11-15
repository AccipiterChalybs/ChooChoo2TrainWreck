using UnityEngine;
using System.Collections;
using System.Data.SqlClient;

public class SQLManager : MonoBehaviour {
	
	// Use this for initialization
	public SqlConnection Start () {
		myConnection = new SqlConnection("user id=username;" + 
		                                 "password=password;server=serverurl;" + 
		                                 "Trusted_Connection=yes;" + 
		                                 "database=database; " + 
		                                 "connection timeout=30");

		// Try Connecting
		try
		{
			myConnection.Open();
			return myConnection;
		}

		// Catch error connecting to SQL DB
		catch(Exception e)
		{
			//Console.WriteLine(e.ToString());
		}
	}
	
	// Update is called once per frame
	void Update () {}
}

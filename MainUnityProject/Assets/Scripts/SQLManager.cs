using UnityEngine;
using System.Collections;
using System.Data.SqlClient;

public class SQLManager : MonoBehaviour {
	
	// Use this for initialization
	public SqlConnection Start () {
		SqlConnection myConnection = new SqlConnection("user id=username;" + 
		                                 "password=password;server=serverurl;" + 
		                                 "Trusted_Connection=yes;" + 
		                                 "database=database; " + 
		                                 "connection timeout=30");

		// Try Connecting
		myConnection.Open();
		return myConnection;
	}
	
	// Update is called once per frame
	void Update () {}
}

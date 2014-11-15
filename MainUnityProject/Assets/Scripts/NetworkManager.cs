using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	bool isServer;
	auto ipAddress;
	auto port;
	auto password;
	
	/* server starts hosting a game, or client attempts to connect */
	void setupNetwork();
	
	/* P only */
	/* Wrapper method to change which RPC sending input causes */
	/* Make sure to send a timestamp! */
	void client_SendInput();
	
	/* S only */
	void server_PlayerJoin();
	
	void server_SendFullRefresh();
	
	void server_SendPlayerDeath();
	
	void server_SendPlayerPosition();
	
	void server_SendEnemyPosition();
	
	void server_SendEnvironmentState()
}

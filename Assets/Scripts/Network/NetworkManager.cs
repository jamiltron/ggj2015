using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the high-level network communication and state in the game.
/// </summary>
public class NetworkManager : MonoBehaviour
{
	public const int SERVER_PORT = 9999;
	public const int MAX_PLAYERS = 8;
	public const string GAME_TYPE_NAME = "GGJ2015_SoundBeasts";
	public const string LOBBY_SCENE = "Lobby";
	public const string GAME_SCENE = "GameScene";

	public GameObject PlayerPrefab;

	/// <summary>
	/// Called by Unity to initialize networking by starting a server or connecting to a running server.
	/// </summary>
	public void Start()
	{
		if(!Network.isClient && !Network.isServer)
		{
			// Not connected, so start a new server.
			Debug.Log("Initializing server using port " + SERVER_PORT + "...");
			Network.InitializeServer(MAX_PLAYERS - 1, SERVER_PORT, false);
		}
		else
		{
			// Presumably we've turned of the message queue while switching scenes, so turn it back on.
			Network.isMessageQueueRunning = true;

			// We connected to server, so instantiate our player.
			Network.Instantiate(PlayerPrefab, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation, 0);

		}
	}

	/// <summary>
	/// Called when the server has been initialized.
	/// </summary>
	public void OnServerInitialized()
	{
		Debug.Log("Successfully Initialized server using port " + SERVER_PORT + ".");
		MasterServer.RegisterHost(GAME_TYPE_NAME, Environment.UserName + "'s game");
	}

	/// <summary>
	/// Called when a player has connected to this server.
	/// </summary>
	public void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("New player " + player + " has connected from " + player.ipAddress + ":" + player.port + ".");
	}

	/// <summary>
	/// Called when a player has disconnected from this server.
	/// </summary>
	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Player " + player + " has disconnected.");
	}

	/// <summary>
	/// Called when this client has been disconnected from the server.
	/// </summary>
	public void OnDisconnectedFromServer(NetworkDisconnection mode)
	{
		switch(mode)
		{
			case NetworkDisconnection.LostConnection:
				Debug.LogWarning("Unintentionally lost connection to server.");
				Application.LoadLevel(LOBBY_SCENE);
				break;
			case NetworkDisconnection.Disconnected:
				Debug.Log("Disconnected from the server.");
				Application.LoadLevel(LOBBY_SCENE);
				break;
			default:
				Debug.LogError("Disconnection type " + mode + " is not supported.");
				break;
		}
	}

	/// <summary>
	/// Sends the specified message to be logged by the specified client.
	/// </summary>
	public void LogClientMessage(string msg, NetworkPlayer player)
	{
		networkView.RPC("LogClientMessageRpc", player, msg);
	}

	/// <summary>
	/// Prints the specified message to the console.
	/// </summary>
	[RPC] private void LogClientMessageRpc(string msg, NetworkMessageInfo info)
	{
		// Only log on client.
		if(!Network.isClient)
			return;

		Debug.Log(msg);
	}
}
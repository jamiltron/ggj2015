using UnityEngine;

/// <summary>
/// Displays a simple user interface that allows the player to host a game or join an existing game.
/// </summary>
public class LobbyGui : MonoBehaviour
{
	#region Private Fields

	private string _serverIP;
	private string _manualIP = "localhost";
	private string _error;

	#endregion
	#region Unity Events

	/// <summary>
	/// Called by Unity to display the GUI.
	/// </summary>
	public void OnGUI()
	{
		if(!Network.isClient && !Network.isServer)
		{
			if(GUILayout.Button("Host Server"))
			{
				Application.LoadLevel(NetworkManager.GAME_SCENE);
				// The scene itself will start the server.
			}

			GUILayout.Label("");

			// Display list of LAN servers.
			MasterServer.RequestHostList(NetworkManager.GAME_TYPE_NAME);
			HostData[] hosts = MasterServer.PollHostList();
			foreach(HostData host in hosts)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(host.gameName, GUILayout.Width(80));
				GUILayout.Label(host.connectedPlayers + "/" + host.playerLimit, GUILayout.Width(80));
				GUILayout.Label(host.ip[0] + ":" + host.port, GUILayout.Width(140));
				if(GUILayout.Button("Join"))
				{
					_serverIP = host.ip[0];
					Network.Connect(host);
					Debug.Log("Attempting to connect to server at " + host + ".");
				}
				GUILayout.EndHorizontal();
			}
			if(hosts.Length == 0)
				GUILayout.Label("No servers found.");

			// Connect to manual IP address?
			GUILayout.Label("");
			GUILayout.Label("Connect to IP address or hostname:");
			_manualIP = GUILayout.TextField(_manualIP);
			if(GUILayout.Button("Connect"))
			{
				_serverIP = _manualIP;
				Network.Connect(_manualIP, NetworkManager.SERVER_PORT);
				Debug.Log("Attempting to connect to server at " + _manualIP + ".");
			}

			// Error?
			if(!string.IsNullOrEmpty(_error))
			{
				GUI.color = Color.red;
				GUILayout.Label(_error);
				GUI.color = Color.white;
			}
		}
//		else if(Network.status == NetworkStatus.Connecting)
//		{
//			GUILayout.Label("Connecting to server at " + _serverIP + "...");
//		}
		else if(Network.isClient)
		{
			GUILayout.Label("Connected to server at " + _serverIP + ". Please wait for the game to start.");
		}
//		else if(Network.status == NetworkStatus.Disconnecting)
//		{
//			GUILayout.Label("Disconnecting from server at " + _serverIP + "...");
//		}
//		else
//		{
//			Debug.LogError("Unknown Network.Status '" + Network.status + "'.");
//		}
	}

	/// <summary>
	/// Called by uLink when a connection has been successfully established with the server.
	/// </summary>
	public void OnConnectedToServer()
	{
		Debug.Log("Successfully connected to the server.");
		Network.isMessageQueueRunning = false; // Stop message queue until scene is fully loaded.
		Application.LoadLevel(NetworkManager.GAME_SCENE);
	}

	/// <summary>
	/// Called by uLink when a connection attempt has failed.
	/// </summary>
	public void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.LogError("Unable to connect to server: " + error);
		_error = "Unable to connect to server: " + error;
	}

	#endregion
}
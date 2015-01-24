using UnityEngine;

public class DestroyOnDisconnected : MonoBehaviour
{
	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		if (player == networkView.owner)
			Network.Destroy(gameObject);
	}
}
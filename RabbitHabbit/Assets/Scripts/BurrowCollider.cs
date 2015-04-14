using UnityEngine;
using System.Collections;

public class BurrowCollider : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();
			gamePlayController.RestartPannel.SetActive(true);

			GameObject[] allWolves = GameObject.FindGameObjectsWithTag("Wolf");
			foreach (GameObject wolf in allWolves)
			{
				wolf.GetComponent<BehaviourRecorder>().EndRecorder(false);
			}
		}
	}
}

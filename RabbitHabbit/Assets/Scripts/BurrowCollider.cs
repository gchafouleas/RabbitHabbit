using UnityEngine;
using System.Collections;

public class BurrowCollider : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			GameObject[] allWolves = GameObject.FindGameObjectsWithTag("Wolf");
			foreach (GameObject wolf in allWolves)
			{
				wolf.GetComponent<BehaviourRecorder>().EndRecorder(false);
			}
			gamePlayController.wins++; 
			gamePlayController.winText.text = "Wins: "+gamePlayController.wins;  
			gamePlayController.winText.text = "Loses: "+gamePlayController.loses;
			GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();
			gamePlayController.RestartPannel.SetActive(true);
		}
	}
}

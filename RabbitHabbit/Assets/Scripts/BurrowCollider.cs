using UnityEngine;
using System.Collections;

public class BurrowCollider : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		
		if(collider.CompareTag("Rabbit"))
		{
			if (!GameObject.FindObjectOfType<RabbitAI>())
			{
				Wolf[] allWolves = GameObject.FindObjectsOfType<Wolf>();
				foreach (Wolf wolf in allWolves)
				{
					wolf.gameObject.GetComponent<BehaviourRecorder>().EndRecorder(false);
				}
				GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();
				gamePlayController.wins++;
				gamePlayController.winText.text = "Wins: " + gamePlayController.wins;
				gamePlayController.losesText.text = "Loses: " + gamePlayController.loses;
				gamePlayController.RestartPannel.SetActive(true);
			}
			else
			{
				Wolf[] allWolves = GameObject.FindObjectsOfType<Wolf>();
				foreach (Wolf wolf in allWolves)
				{
					wolf.gameObject.GetComponent<BehaviourRecorder>().EndRecorder(false);
				}
				GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();
				gamePlayController.OnRestartClick();
				GameObject.FindObjectOfType<WolfArraySeralizer>().RunThroughComplete();
			}

			
		}
	}
}

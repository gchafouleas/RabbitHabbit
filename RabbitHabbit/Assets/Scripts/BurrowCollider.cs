using UnityEngine;
using System.Collections;

public class BurrowCollider : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();  
			gamePlayController.wins++; 
			gamePlayController.winText.text = "Wins: "+gamePlayController.wins;  
			gamePlayController.winText.text = "Loses: "+gamePlayController.loses;
			gamePlayController.RestartPannel.SetActive(true); 
		}
	}
}

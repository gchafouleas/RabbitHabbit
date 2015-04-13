using UnityEngine;
using System.Collections;

public class BurrowCollider : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			GamePlayController gamePlayController = GameObject.FindObjectOfType<GamePlayController>();  
			gamePlayController.RestartPannel.SetActive(true); 
		}
	}
}

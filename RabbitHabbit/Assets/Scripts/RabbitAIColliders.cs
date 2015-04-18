using UnityEngine;
using System.Collections;

public class RabbitAIColliders : MonoBehaviour {
	public RabbitAI rabbitAI;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Wolf"))
		{			
			rabbitAI.SpottedAWolf(collider.gameObject);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Wolf"))
		{
			rabbitAI.LostSightOfWolf(collider.gameObject);
		}
	}
}

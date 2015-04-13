using UnityEngine;
using System.Collections;

public class PuddleColision : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			collider.GetComponent<RabbitBehavior>().IsWalkingOnWater = true;
			collider.GetComponent<RabbitBehavior>().waterTimerElapsed = false; 
			var pelette = (GameObject)Instantiate (Resources.Load ("SentPellette"));
			pelette.transform.position = new Vector3 (collider.transform.position.x, collider.transform.position.y, collider.transform.position.z); 
			collider.GetComponent<RabbitBehavior>().LastPlacedSentPelette.GetComponent<SentPelette> ().NextSentPelette = pelette.GetComponent<SentPelette> ();
		}
	}
	void OnTriggerExit(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			collider.GetComponent<RabbitBehavior>().IsWalkingOnWater = false; 
		}
	}
}

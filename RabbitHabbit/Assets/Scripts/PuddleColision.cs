using UnityEngine;
using System.Collections;

public class PuddleColision : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Rabbit"))
		{
			collider.GetComponent<RabbitBehavior>().waterTimerElapsed = false; 
			var pelette = (GameObject)Instantiate (Resources.Load ("SentPelette"));
			pelette.transform.position = new Vector3 (collider.transform.position.x, collider.transform.position.y, collider.transform.position.z); 
			collider.GetComponent<RabbitBehavior>().LastPlacedSentPelette.GetComponent<SentPelette> ().NextSentPelette = pelette.GetComponent<SentPelette> ();
		}
	}
}

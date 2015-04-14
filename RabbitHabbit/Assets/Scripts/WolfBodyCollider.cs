using UnityEngine;
using System.Collections;

public class WolfBodyCollider : MonoBehaviour 
{
	public Wolf wolf;
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag.Equals("Pellet"))
		{
			Debug.Log("Found Pellet");
			wolf.currentWolfState = GlobalVars.wolfState.Sniff;
			wolf.pellet = collider.gameObject.GetComponent<SentPelette>();
		}
	}
}
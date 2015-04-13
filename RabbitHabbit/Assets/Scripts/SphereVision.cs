using UnityEngine;
using System.Collections;

public class SphereVision : MonoBehaviour 
{
    public Wolf wolf;
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag.Equals("Rabbit"))
		{ 
            wolf.RabbitDetected(collider.gameObject);
		}
		if(collider.CompareTag("Wolf"))
		{
			wolf.SpottedWolfFriend(collider.gameObject.GetComponent<Wolf>());
		}
    }

	void OnTriggerStay(Collider collider)
	{
		if (collider.tag.Equals("Rabbit"))
		{
			wolf.MaintainSight();
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Wolf"))
		{
			wolf.WolfFriendLeft(collider.gameObject.GetComponent<Wolf>());
		}
		if (collider.tag.Equals("Rabbit"))
			wolf.RabbitLost();
	}
}

using UnityEngine;
using System.Collections;

public class BushCollider : MonoBehaviour {

	void start()
	{
		//trying to make bushes transparant 
		var color = this.GetComponent<MeshRenderer>().material.color; 
		color.a = 0f;
		this.renderer.material.color = color; 

	}
	void OnTriggerEnter(Collider collider) 
	{
		if(collider.gameObject.GetComponent<RabbitBehavior>() != null)
		{
			collider.gameObject.GetComponent<RabbitBehavior>().IsHidden = true; 
		}
	}
}

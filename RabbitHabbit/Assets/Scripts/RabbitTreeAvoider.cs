using UnityEngine;
using System.Collections;

public class RabbitTreeAvoider : MonoBehaviour {

	public RabbitAI rabbit;
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == "sycamore")
		{
			rabbit.treeInTheWay = true;
		}
		if (collider.CompareTag("Boundary"))
		{
			rabbit.boundaryInWay = true;
		}
	}
}

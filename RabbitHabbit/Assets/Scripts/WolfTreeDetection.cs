using UnityEngine;
using System.Collections;

public class WolfTreeDetection : MonoBehaviour
{
	public Wolf wolf;
    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.name == "sycamore")
		{
			wolf.treeInTheWay = true;
		}
    }

}

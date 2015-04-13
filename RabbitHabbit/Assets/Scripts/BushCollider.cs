using UnityEngine;
using System.Collections;

public class BushCollider : MonoBehaviour 
{
    public bool rabbitInBush = false;
	void start()
	{
		//trying to make bushes transparant 
		var color = this.GetComponent<MeshRenderer>().material.color; 
		color.a = 0f;
		this.renderer.material.color = color; 
	}

	void OnTriggerEnter(Collider collider) 
	{
		if(collider.tag.Equals("Rabbit"))
		{
            this.rabbitInBush = true;
			collider.gameObject.GetComponent<RabbitBehavior>().IsHidden = true;
            collider.gameObject.GetComponent<RabbitBehavior>().movementSpeed = .8f;
		}
	}

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Rabbit"))
        {
            this.rabbitInBush = false;
            collider.gameObject.GetComponent<RabbitBehavior>().IsHidden = false;
            collider.gameObject.GetComponent<RabbitBehavior>().movementSpeed = 1.5f;
        }
    }
}

using UnityEngine;
using System.Collections;

public class FrontVision : MonoBehaviour 
{
    public Wolf wolf;
    void OnTriggerStay(Collider collider)
    {
        if (collider.tag.Equals("Rabbit"))
            wolf.RabbitDetected(collider.gameObject);
    }
}

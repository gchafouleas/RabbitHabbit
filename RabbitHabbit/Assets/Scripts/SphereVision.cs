using UnityEngine;
using System.Collections;

public class SphereVision : MonoBehaviour 
{
    public Wolf wolf;
    void OnTriggerStay(Collider collider)
    {
        if(collider.tag.Equals("Rabbit"))
            wolf.RabbitDetected();
    }
}

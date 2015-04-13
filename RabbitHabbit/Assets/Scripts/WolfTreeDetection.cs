using UnityEngine;
using System.Collections;

public class WolfTreeDetection : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.GetComponent<Wolf>().treeInTheWay = true;
    }

}

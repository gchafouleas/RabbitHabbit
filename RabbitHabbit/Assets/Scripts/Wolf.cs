using UnityEngine;
using System.Collections;

public class Wolf : MonoBehaviour 
{
    private bool avoidBushStalking;
    private bool avoidBushCharging;
    private bool stalkBehindRabbit;
    private bool stalkBesideRabbit;
    public GameObject rabbit;
    public bool treeInTheWay = false;
    //Kinematic movement variables
    private Vector3 directionVector = Vector3.zero;
    private float currentVelocity = 0f, maxRotateVelocity = 1.5f, maxSeekVelocity = 10f, maxFleeVelocity = 8f;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    private void Wander()
    {

    }

    private void Charge()
    {
    }

    private void RunToHowl()
    {

    }

    private void Stalk()
    {

    }

    private void SniffTrail()
    {

    }

    private void Howl()
    {
        GameObject[] wolves = GameObject.FindGameObjectsWithTag("Wolf");
        foreach (GameObject wolf in wolves)
        {
            if (wolf != this.gameObject)
                wolf.GetComponent<Wolf>().HowlHeard(this.gameObject);
        }
    }

    private void AvoidBushStalking()
    {

    }

    public void HowlHeard(GameObject wolfTarget)
    {
        KinematicSeek(wolfTarget);
        if (treeInTheWay)
        {
        }
        Debug.Log("Howl was heard");
    }
    public void KinematicSeek(GameObject target)
    {
        directionVector = (target.transform.position - transform.position);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (maxSeekVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
    }

    public void KinematicArrive(GameObject target)
    {
        directionVector = (target.transform.position - transform.position);
        directionVector.Normalize();
        currentVelocity = (maxFleeVelocity * (target.transform.position - transform.position).magnitude / 15f);
        if (currentVelocity > maxFleeVelocity)
            currentVelocity = maxFleeVelocity;
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (currentVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
    }

    public void KinematicPursuit(GameObject target)
    {
        float estimatedArrivalTime = (target.transform.position - transform.position).magnitude / maxSeekVelocity;
        Vector3 nextTargetPosition = target.transform.position + (target.GetComponent<RabbitBehavior>().Velocity * estimatedArrivalTime) * target.transform.forward.normalized;
        directionVector = (nextTargetPosition - transform.position);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (maxSeekVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
    }

    public void KinematicFlee(GameObject target)
    {
        directionVector = (transform.position - target.transform.position);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (maxFleeVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
    }

    public void KinematicEvade(GameObject target)
    {
        float estimatedArrivalTime = (target.transform.position - transform.position).magnitude / maxSeekVelocity;
        Vector3 nextTargetPosition = target.transform.position + (target.GetComponent<RabbitBehavior>().Velocity * estimatedArrivalTime) * target.transform.forward.normalized;
        directionVector = (transform.position - nextTargetPosition);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (maxFleeVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
    }

    public void RabbitDetected()
    {
        Debug.Log("Rabbit Detected");
    }

    public void treeDetected()
    {

    }
}
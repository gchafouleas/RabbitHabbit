using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : MonoBehaviour 
{
    private bool avoidBushStalking;
    private bool avoidBushCharging;
    private bool stalkBehindRabbit;
    private bool stalkBesideRabbit;

	private List<Wolf> wolveFriendsNear = new List<Wolf>();

	public GameObject rabbit;

	#region Counters
	private float lostSightCounter = 0f;
	private const float lostSightThreshold = 1f; //time till counter expires

	private bool smeltRabbit = false;
	private float lostSmellCounter = 0f;
	private const float lostSmellThreshold = 5f; //5 seconds if no smell

	private bool heardHowl = false;
	private float howlCounter = 0f;
	private const float howlThreshold = 10f;

	private float stateCounter = 0f; //how long you've been in one state for.
	private const float stateCounterStayThreshold = 30f; //Do not change, tied to event name
	private bool stateStayEventAlerted = false;
	#endregion
	//Kinematic movement variables
    private Vector3 directionVector = Vector3.zero;
    private float currentVelocity = 0f, maxRotateVelocity = 1.5f, maxSeekVelocity = 10f, maxFleeVelocity = 8f;
	public BehaviourMatrix behaviourMatrix;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateSmellHandler();
		UpdateHowlHandler();
		UpdateStateCounterHandler();

	}

	private void UpdateStateCounterHandler()
	{
		if(!stateStayEventAlerted)
		{
			stateCounter += Time.deltaTime;
			if(stateCounter > stateCounterStayThreshold)
			{
				stateStayEventAlerted = true;
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.InStateFor30Seconds, true);
			}
		}
	}

	public void StateWasUpdated()
	{
		if(stateStayEventAlerted)
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.InStateFor30Seconds,false);

		stateStayEventAlerted = false;
		stateCounter = 0f;
	}

	public void SpottedWolfFriend(Wolf wolfFriendSpotted)
	{
		if (!wolveFriendsNear.Contains(wolfFriendSpotted))
		{
			wolveFriendsNear.Add(wolfFriendSpotted);
			WolfFriendsNearEventCaller();
		}
		
	}
	public void WolfFriendLeft(Wolf wolfFriendLeft)
	{
		if (wolveFriendsNear.Contains(wolfFriendLeft))
		{
			wolveFriendsNear.Remove(wolfFriendLeft);
			WolfFriendsNearEventCaller();
		}
	}

	private void WolfFriendsNearEventCaller()
	{ //this 4x event call is somewhat calculation intesive unfortunatly
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.OneWolfNear, false);
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.TwoWolvesNear, false);
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.ThreeWolvesNear, false);

		switch(wolveFriendsNear.Count)
		{			
			case 1:
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.OneWolfNear, true);
				break;
			case 2:
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.TwoWolvesNear, true);
				break;
			case 3:
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.ThreeWolvesNear, true);
				break;
			default:
				break;			
		}
	}

	private void UpdateSmellHandler()
	{
		if (smeltRabbit)
		{
			lostSightCounter += Time.deltaTime;
			if (lostSmellCounter > lostSmellThreshold)
			{
				smeltRabbit = false;
				lostSmellCounter = 0f;
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SmellRabbit, false);

			}
		}
	}
	private void UpdateHowlHandler()
	{
		if (heardHowl)
		{
			howlCounter += Time.deltaTime;
			if (howlCounter > howlThreshold)
			{
				heardHowl = false;
				howlCounter = 0f;
				behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.HearHowl, false);
			}
		}
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
		smeltRabbit = true;
		lostSmellCounter = 0;
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SmellRabbit, true);

    }

    private void Howl()
    {
        GameObject[] wolves = GameObject.FindGameObjectsWithTag("Wolf");
        foreach (GameObject wolf in wolves)
        {
            if (wolf != this.gameObject)
                wolf.GetComponent<Wolf>().HowlHeard(this.transform.position);
        }
    }

    private void AvoidBushStalking()
    {
    }

    public void HowlHeard(Vector3 wolfLocation)
    {
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.HearHowl, true);
		heardHowl = true;			
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

    public void RabbitDetected(GameObject _rabbit)
    {
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SeeRabbit, true);
		rabbit = _rabbit;
        Debug.Log("Rabbit Detected");
    }
}
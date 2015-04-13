using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : MonoBehaviour 
{
    private bool avoidBushStalking;
    private bool avoidBushCharging;
    private bool stalkBehindRabbit;
    private bool stalkBesideRabbit;
    public GameObject rabbit;
    public bool treeInTheWay = false;
	public GlobalVars.wolfState currentWolfState = GlobalVars.wolfState.Wander;
    //Kinematic movement variables
	public float wanderTimer;
	public float stopWanderTimer = 0.2f;
	Vector3 wanderDirection;
	public float moveSpeed = 5f;


	private List<Wolf> wolveFriendsNear = new List<Wolf>();
	public bool losingRabbit = false;
	public float lostSightTime = 0f;
	public float LOST_SIGHT_TIME_MAX = 5f;

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
		wanderDirection = new Vector3(Random.rotation.x, 0f, Random.rotation.z);
		transform.rotation = Quaternion.LookRotation(wanderDirection, Vector3.up);
	}

	
	// Update is called once per frame
	void Update ()
	{
		UpdateSmellHandler();
		UpdateHowlHandler();
		UpdateStateCounterHandler();

		wanderTimer = Mathf.Max(0, wanderTimer - Time.deltaTime);
		SeeingRabbit();

		switch (currentWolfState)
		{
			case GlobalVars.wolfState.ChaseRabbit:
				Charge();
				break;
			case GlobalVars.wolfState.Howl:
				Howl();
				break;
			case GlobalVars.wolfState.Sniff:
				SniffTrail();
				break;
			case GlobalVars.wolfState.Stalk:
				Stalk();
				break;
			case GlobalVars.wolfState.Wander:
				Wander();
				break;
		}
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

	public void StateWasUpdated(GlobalVars.wolfState newState)
	{
		if(stateStayEventAlerted)
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.InStateFor30Seconds,false);

		stateStayEventAlerted = false;
		stateCounter = 0f;
		currentWolfState = newState;
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
		//if you have idled long enough, find a new wander direction and go directly toward it
		if (stopWanderTimer == 0)
		{
			wanderDirection = new Vector3(Random.rotation.x, 0f, Random.rotation.z);
			transform.rotation = Quaternion.LookRotation(wanderDirection, Vector3.up);
			wanderTimer = (Random.value) + 1f;
			stopWanderTimer = 1f;

			wanderDirection.Normalize();
			rigidbody.velocity = wanderDirection * moveSpeed;
		}
		else if (wanderTimer == 0)
		{ //if you have wandered long enough, stop moving for some time
			rigidbody.velocity = Vector3.zero;
			stopWanderTimer = Mathf.Max(0, stopWanderTimer - Time.deltaTime);
		}
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

		//should be alerted of the next node and go that way
    }

    private void Howl()
    {
        GameObject[] wolves = GameObject.FindGameObjectsWithTag("Wolf");
        foreach (GameObject wolf in wolves)
        {
            if (wolf != this.gameObject)
                wolf.GetComponent<Wolf>().HowlHeard();
        }
    }

    private void AvoidBushStalking()
    {

    }
	 

    public void HowlHeard()
    {
		if (!heardHowl)
		{
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.HearHowl, true);
			heardHowl = true;
		}
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
		losingRabbit = false;
    }

    public void treeDetected()
    {

    }
	public void MaintainSight()
	{
		RaycastHit[] hits;
		Vector3 rayToRabbit = rabbit.transform.position - transform.position;
		hits = Physics.RaycastAll(transform.position, rayToRabbit.normalized, rayToRabbit.magnitude);
		foreach (RaycastHit rayhit in hits)
		{
			if (rayhit.collider.tag == "Tree" || rayhit.collider.tag == "Bush")
			{
				if (Vector3.Distance(transform.position, rayhit.transform.position) < Vector3.Distance(transform.position, rabbit.transform.position))
				{	//Tree or Bush is between wolf and rabbit
					RabbitLost();
				}
			}
			else
			{
				losingRabbit = false;
			}
		}
	}

	public void RabbitLost()
	{
		losingRabbit = true;
	}

	public void SeeingRabbit()
	{
		if (losingRabbit)
		{
			lostSightTime += Time.deltaTime;
		}
		else
		{
			lostSightTime = 0f;
		}

		if (lostSightTime >= LOST_SIGHT_TIME_MAX)
		{
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SeeRabbit, false);
			rabbit = null;
		}
	}
}
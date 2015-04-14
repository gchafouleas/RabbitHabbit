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
    public GameObject wolfThatHowledLocation;
    public SentPelette pellet;
    public bool treeInTheWay = false;
	public GlobalVars.wolfState currentWolfState;
    //Kinematic movement variables
	public float wanderTimer;
	public float stopWanderTimer = 0.2f;
	public Vector3 wanderDirection;
	public float moveSpeed = 1f;

	private List<Wolf> wolveFriendsNear = new List<Wolf>();
	public bool losingRabbit = false;
	public float lostSightTime = 0f;
	public float LOST_SIGHT_TIME_MAX = 5f;
	public float MAX_STALK_DISTANCE = 7f;
    private Vector3 treeAvoidanceTarget = Vector3.zero;
	#region Counters
	private float lostSightCounter = 0f;
	private const float lostSightThreshold = 1f; //time till counter expires

	public bool smeltRabbit = false;
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
    private float currentVelocity = 0f, maxRotateVelocity = 2.5f, maxSeekVelocity = 1f, maxFleeVelocity = 8f;
	public BehaviourMatrix behaviourMatrix;
	public bool boundaryInWay = false; 
	private Vector3 boundaryAvoidanceTarget = Vector3.zero; 
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
                maxSeekVelocity = 2;
				Charge();
				break;
			case GlobalVars.wolfState.Howl:
				Howl();
				break;
			case GlobalVars.wolfState.Sniff:
                maxSeekVelocity = 1;
				SniffTrail();
				break;
			case GlobalVars.wolfState.Stalk:
                maxSeekVelocity = 1;
				Stalk();
				break;
			case GlobalVars.wolfState.Wander:
                maxSeekVelocity = 1;
				Wander();
				break;
            case GlobalVars.wolfState.RunToHowl:
                if(wolfThatHowledLocation != null)
                    RunToHowl(wolfThatHowledLocation);
                break;
		}
		AngleFix();
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
	public void AvoidBoundary()
	{
		// If the target for avoiding a tree is not set, set it
		if (boundaryAvoidanceTarget == Vector3.zero)
		{
			boundaryAvoidanceTarget = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 2);
		}
		
		// rotate towards the new tree avoidance target
		if (rotateTowards(boundaryAvoidanceTarget))
		{
			boundaryInWay = false;
			boundaryAvoidanceTarget = Vector3.zero;
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
		if(!treeInTheWay && !boundaryInWay)
		{
			//if you have idled long enough, find a new wander direction and go directly toward it
			if (stopWanderTimer == 0)
			{
				wanderDirection = new Vector3(Random.rotation.x, 0f, Random.rotation.z);
				rotateTowards (wanderDirection);
				//transform.rotation = Quaternion.LookRotation(wanderDirection, Vector3.up);
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
		else if( treeInTheWay)
		{
			AvoidTree(); 
		}
		else if(boundaryInWay)
		{
			AvoidBoundary(); 
		}
    }

    private void Charge()
    {
        if (!treeInTheWay && !boundaryInWay)
        {
            KinematicSeek(rabbit);
        }
        else if(treeInTheWay)
        {
            AvoidTree();
        }
		else if(boundaryInWay)
		{
			AvoidBoundary(); 
		}
    }

    private void RunToHowl(GameObject wolfLocation)
    {
        if (!treeInTheWay && !boundaryInWay)
        {
            KinematicSeek(wolfLocation);
        }
        else if(treeInTheWay)
        {
            AvoidTree(); 
        }
		else if(boundaryInWay)
		{
			AvoidBoundary(); 
		}
		if (Vector3.Distance(this.gameObject.transform.position, wolfLocation.transform.position) < 2)
		{
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.HearHowl, false);
			wolfThatHowledLocation = null;
		}
    }

    private void AvoidTree()
    {
        // If the target for avoiding a tree is not set, set it
        if (treeAvoidanceTarget == Vector3.zero)
        {
            treeAvoidanceTarget = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z + 2);
        }

        // rotate towards the new tree avoidance target
        if (rotateTowards(treeAvoidanceTarget))
        {
            //If the wolf rotated towards the target starts seeking it
            KinematicSeekVector(treeAvoidanceTarget);
        }

        // If the distance between the wolf and the avoidance target is small, the tree should not be in the way.
        if (Vector3.Distance(this.transform.position, treeAvoidanceTarget) < 0.1f)
        {
            treeInTheWay = false;
            treeAvoidanceTarget = Vector3.zero;
        }

    }

    private void Stalk()
    {
		if (Vector3.Distance(transform.position, rabbit.transform.position) > MAX_STALK_DISTANCE)
		{
			if (!treeInTheWay && !boundaryInWay)
			{
				KinematicSeek(rabbit);
			}
			else if(treeInTheWay)
			{
				AvoidTree();
			}
			else if(boundaryInWay)
			{
				AvoidBoundary(); 
			}
		}
		else
		{
			rotateTowards(rabbit.transform.position);
		}
    }

    private void SniffTrail()
    {
		smeltRabbit = true;
		lostSmellCounter = 0;
		behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SmellRabbit, true);
		if(pellet !=null)
		{
			//should be alerted of the next node and go that way
			if (!treeInTheWay && ! boundaryInWay)
			{
				KinematicSeekVector(pellet.transform.position);
			}
			else if(boundaryInWay)
			{
				AvoidBoundary(); 
			}
			else if(treeInTheWay)
			{
				AvoidTree();
			}
			
			if (Vector3.Distance(this.transform.position, pellet.transform.position) < 0.1)
			{
				if (pellet.NextSentPelette != null)
				{
					pellet = pellet.NextSentPelette;
				}
				else
				{
					smeltRabbit = false;
					lostSmellCounter = 0f;
					behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.SmellRabbit, false);
				}
			}
		}

    }

    public void Howl()
    {
		Wolf[] wolves = GameObject.FindObjectsOfType<Wolf>();
		AudioSource sound = this.gameObject.GetComponent<AudioSource> ();
		sound.Play (); 
        foreach (Wolf wolf in wolves)
        {
            if (wolf != this.gameObject)
                wolf.gameObject.GetComponent<Wolf>().HowlHeard(this.gameObject);
        }
    }

    private void AvoidBushStalking()
    {

    }
	 

    public void HowlHeard(GameObject wolfObject)
    {
        wolfThatHowledLocation = wolfObject;
		if (!heardHowl)
		{
			behaviourMatrix.RecieveEvent(GlobalVars.wolfEvent.HearHowl, true);
			heardHowl = true;
		}
    }
    public void KinematicSeekVector(Vector3 target)
    {
        directionVector = (target - transform.position);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
        Vector3 newPos = transform.position + (maxSeekVelocity * Time.deltaTime) * directionVector;
        transform.position = newPos;
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

	private void AngleFix()
	{
		transform.position = new Vector3(transform.position.x, 0.81f, transform.position.z);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(eulerAngles);
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

    public bool rotateTowards(Vector3 targetPosition)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
        if (transform.rotation == targetRotation)
        {
            return true;
        }
        else
        {
            return false;
        }
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
		if (!rabbit)
		{
			RabbitLost();
			return;
		}
		RaycastHit[] hits;
		Vector3 rayToRabbit = rabbit.transform.position - transform.position;
		hits = Physics.RaycastAll(transform.position, rayToRabbit.normalized, rayToRabbit.magnitude);
		foreach (RaycastHit rayhit in hits)
		{
			if (rayhit.collider.gameObject.name == "sycamore" || rayhit.collider.gameObject.name == "Bush 05 prefab")
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
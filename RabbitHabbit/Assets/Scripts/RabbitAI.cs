using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RabbitAI : MonoBehaviour {

	bool hidingInBush = false;
	bool wolfIsHowling = false;
	public RabbitBehavior rabbit;
	public List<GameObject> wolvesSpotted = new List<GameObject>();
	public Transform holeLocation;
	 public bool treeInTheWay = false;	
	public bool boundaryInWay = false;
	private Vector3 treeAvoidanceTarget = Vector3.zero;
	private Vector3 boundaryAvoidanceTarget = Vector3.zero;
	private float currentVelocity = 0f, maxRotateVelocity = 2.5f, maxSeekVelocity = 1f, maxFleeVelocity = 8f;

	private Vector3 directionVector = Vector3.zero;

	void Start()
	{
		Time.timeScale = 10f;
	}

	void Update() 
	{
		if (!rabbit)
			rabbit = GameObject.FindObjectOfType<RabbitBehavior>();
		if (!holeLocation)
			holeLocation = GameObject.FindObjectOfType<BurrowCollider>().transform;

		this.gameObject.transform.position = rabbit.transform.position;
		this.gameObject.transform.rotation = rabbit.transform.rotation;
		this.gameObject.transform.eulerAngles = new Vector3(rabbit.transform.rotation.x, rabbit.transform.rotation.y +45f, rabbit.transform.rotation.z); //dirty fix to rabbit ai problem

		RunToHole();

	}
	
	public void SpottedAWolf(GameObject wolf)
	{
		if(!wolvesSpotted.Contains(wolf))
		{
			wolvesSpotted.Add(wolf);
		}
	}

	public void LostSightOfWolf(GameObject wolf)
	{
		if (wolvesSpotted.Contains(wolf))
		{
			wolvesSpotted.Remove(wolf);
		}
	}
	

	public void RunAway()
	{
		Transform closestWolf = null;
		float closestDistance = 9999f;

		foreach(GameObject wolf in wolvesSpotted)
		{
			float wolfRabbitDistance = Vector3.Distance(wolf.transform.position, rabbit.transform.position);
			if(wolfRabbitDistance < closestDistance)
			{
				closestDistance = wolfRabbitDistance;
				closestWolf = wolf.transform;
			}
		}
		if(closestWolf)
			KinematicFlee(closestWolf);
	}


	public void RunToHole()
	{		 
        if (!treeInTheWay && !boundaryInWay)
        {
            KinematicSeek(holeLocation);
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

	public void KinematicSeek(Transform target)
    {
		Vector3 newOriantation = target.position - rabbit.transform.position;
		newOriantation.y = rabbit.transform.forward.y;
		rabbit.FaceTarget(newOriantation,rabbit.gameObject);
		Vector3 velocity = (maxSeekVelocity * Time.deltaTime) * rabbit.transform.forward;
		Vector3 newPosition = new Vector3(rabbit.transform.position.x + velocity.x, rabbit.transform.position.y, rabbit.transform.position.z + velocity.z);
		rabbit.transform.position = newPosition;

		/*
        directionVector = (target.position - rabbit.transform.position);
        directionVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionVector);
		rabbit.transform.rotation = Quaternion.Lerp(rabbit.transform.rotation, targetRotation, rabbit.rotateSpeed * Time.deltaTime);
		Vector3 newPos = transform.position + (rabbit.movementSpeed * Time.deltaTime) * directionVector;
        rabbit.transform.position = newPos;	*/	
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
	public bool rotateTowards(Vector3 targetPosition)
	{
		Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
		Quaternion negTargetRotation = targetRotation; //make a negation of the targetRotation and test if it is that aswell
		negTargetRotation.w = -negTargetRotation.w;
		negTargetRotation.x = -negTargetRotation.x;
		negTargetRotation.y = -negTargetRotation.y;
		negTargetRotation.z = -negTargetRotation.z;
		if (transform.rotation == targetRotation || transform.rotation == negTargetRotation)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void KinematicSeekVector(Vector3 target)
	{
		directionVector = (target - rabbit.transform.position);
		directionVector.Normalize();
		Quaternion targetRotation = Quaternion.LookRotation(directionVector);
		rabbit.transform.rotation = Quaternion.Lerp(rabbit.transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
		Vector3 newPos = rabbit.transform.position + (maxSeekVelocity * Time.deltaTime) * directionVector;
		rabbit.transform.position = newPos;
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

	public void KinematicFlee(Transform target)
	{
		directionVector = (rabbit.transform.position - target.position);
		directionVector.Normalize();
		Quaternion targetRotation = Quaternion.LookRotation(directionVector);
		rabbit.transform.rotation = Quaternion.Lerp(rabbit.transform.rotation, targetRotation, maxRotateVelocity * Time.deltaTime);
		Vector3 newPos = rabbit.transform.position + (maxFleeVelocity * Time.deltaTime) * directionVector;
		rabbit.transform.position = newPos;
	}

	public void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Wolf"))
		{
			Debug.Log("collision with wolf");
			GamePlayController gpc = GameObject.FindObjectOfType<GamePlayController>();			
			Wolf[] allWolves = GameObject.FindObjectsOfType<Wolf>();
			foreach (Wolf wolf in allWolves)
			{
				wolf.gameObject.GetComponent<BehaviourRecorder>().EndRecorder(true);
			}
			gpc.OnRestartClick();
			GameObject.FindObjectOfType<WolfArraySeralizer>().RunThroughComplete();
		}
	}
}

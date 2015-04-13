using UnityEngine;
using System.Collections;

public class RabbitBehavior : MonoBehaviour
{
    private float velocity = 0f;

    public float Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
	#region Variables
	[SerializeField]
	private Animation rabbitAnimation;
	private CharacterController characterController; 
	private Vector3 movementVector = new Vector3(); 
	private const int IDLE_ANIMATION = 0;
	private const int RUN_ANIMATION = 1;
	private const int WALK_ANIMATION = 2;

	public GameObject RabbitObject; 
	public GameObject MainCamera; 

	[SerializeField]
	private float rotateSpeed = 3.0f;
	[SerializeField]
	public float movementSpeed = 10f;
    private float rotationSpeed = 10f;
	private const float DISTANCE_BW_PELETTES = 5f;  
	public bool CanAddPelettes = false; 
	public bool IsHidden = false;
	public GameObject LastPlacedSentPelette; 
	public bool waterTimerElapsed = true; 
	private const float TIMER = 20f; 
	private float current_Timer = 0f; 
	#endregion 
	// Use this for initialization
	void Start () 
	{
		characterController = GetComponent<CharacterController> (); 
		SetInitialPellets (); 
	}
	public void SetInitialPellets()
	{
		LastPlacedSentPelette = (GameObject)Instantiate (Resources.Load("SentPellette")); 
		LastPlacedSentPelette.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
		CanAddPelettes = true; 
	}
	// Update is called once per frame
	void Update () 
	{
		MoveCharacter (); 
		CheckIfAddPelette (); 

	}
	private void MoveCharacter()
	{
        movementVector = transform.right * Input.GetAxis("LeftJoystickY") * movementSpeed;
		characterController.Move (movementVector*Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("RightJoystickX") * rotationSpeed, 0);
	}
	private void CheckIfAddPelette()
	{
		var distance = Vector3.Distance (this.transform.position, LastPlacedSentPelette.transform.position); 
		if (distance >= DISTANCE_BW_PELETTES && CanAddPelettes && waterTimerElapsed) {
			CanAddPelettes = false; 
			var pelette = (GameObject)Instantiate (Resources.Load ("SentPellette"));
			pelette.transform.position = new Vector3 (this.transform.position.x, transform.position.y, this.transform.position.z); 
			Debug.Log (LastPlacedSentPelette); 
			LastPlacedSentPelette.GetComponent<SentPelette> ().NextSentPelette = pelette.GetComponent<SentPelette> ();
			LastPlacedSentPelette = pelette; 
			CanAddPelettes = true; 
		}
		else if(!waterTimerElapsed)
		{
			current_Timer += Time.deltaTime; 
			if(current_Timer >= TIMER)
			{
				var pelette = (GameObject)Instantiate (Resources.Load ("SentPellette"));
				pelette.transform.position = new Vector3 (this.transform.position.x, transform.position.y, this.transform.position.z); 
				LastPlacedSentPelette = pelette; 
				CanAddPelettes = true; 
				current_Timer = 0f; 
				waterTimerElapsed = true; 
			}
		}
	}
	private void  FaceTarget(Vector3 target, GameObject ObjectToRotate) 
	{
		Vector3 velocityDirection = (target).normalized;
		//velocityDirection.y = ObjectToRotate.transform.forward.y;
		Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
		float angle = Quaternion.Angle(ObjectToRotate.transform.rotation, lookRotation);
		float timeToComplete = angle / 150f;
		float donePercentage = Mathf.Min(1F, Time.deltaTime / timeToComplete);
		ObjectToRotate.transform.rotation = Quaternion.Slerp(ObjectToRotate.transform.rotation, lookRotation, donePercentage);
	}
}

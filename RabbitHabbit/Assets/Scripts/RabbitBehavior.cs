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
	public Animation rabbitAnimation;
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
	public float movementSpeed = 1.5f;
    private float rotationSpeed = 50f;
	private const float DISTANCE_BW_PELETTES = 5f;  
	public bool CanAddPelettes = false; 
	public bool IsHidden = false;
	public GameObject LastPlacedSentPelette; 
	public bool waterTimerElapsed = true; 
	private const float TIMER = 20f; 
	private float current_Timer = 0f; 
	public AudioClip walking; 
	public AudioClip water_walking; 
	private bool StartWalking = true; 
	private float SoundTimer = 0.7f; 
	private const float MAX_SOUND_TIMER = 0.7f; 
	public bool IsWalkingOnWater = false; 
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
		AudioSource sound = this.gameObject.GetComponent<AudioSource> (); 
		Debug.Log(walking.isReadyToPlay);
        //Prevents the Rabbit from moving backwards
        if (Input.GetAxis ("LeftJoystickY") <= -1) {
						movementVector = transform.right * Input.GetAxis ("LeftJoystickY") * movementSpeed;
						characterController.Move (movementVector * Time.deltaTime);
						rabbitAnimation.CrossFade ("run", 0.01f);
			SoundTimer += Time.deltaTime; 
			if(SoundTimer >= MAX_SOUND_TIMER)
			{
				SoundTimer = 0f; 
				if(IsWalkingOnWater)
				{
					sound.PlayOneShot(water_walking);
				}
				else
				{
					sound.PlayOneShot(walking);
				}			 
				 
			}
			 
		} 
		else 
		{
			rabbitAnimation.CrossFade ("lookOut", 0.01f);
			SoundTimer = 0.7f; 
			sound.Stop(); 
		}
        transform.Rotate(0, Input.GetAxis("RightJoystickX") * rotationSpeed * Time.deltaTime, 0);
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

	public void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.CompareTag("Wolf"))
		{
			GamePlayController gpc = GameObject.FindObjectOfType<GamePlayController>();
			gpc.loses++; 
			gpc.winText.text = "Wins: "+gpc.wins;  
			gpc.losesText.text = "Loses: "+gpc.loses;
			gpc.RestartPannel.SetActive(true);
			Wolf[] allWolves = GameObject.FindObjectsOfType<Wolf>();
 			foreach(Wolf wolf in allWolves)
			{
				wolf.gameObject.GetComponent<BehaviourRecorder>().EndRecorder(true);
			}
			
		}
	}
}

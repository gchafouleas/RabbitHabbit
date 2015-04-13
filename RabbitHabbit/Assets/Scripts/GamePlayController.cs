using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class GamePlayController : MonoBehaviour {

	public GameObject worldPrefab; 
	public GameObject burrowPrefab; 
	public GameObject Rabbit; 
	public GameObject RestartPannel; 
	public Image Arrow; 
	// Use this for initialization
	void Start ()
	{
		Instantiate (Rabbit); 
		Instantiate (worldPrefab); 
		Instantiate (burrowPrefab); 
	}
	void Update()
	{
		var rabbit = GameObject.FindObjectOfType <RabbitBehavior>(); 
		var burrow = GameObject.FindObjectOfType<BurrowCollider> (); 
		Debug.Log (burrow.gameObject.transform.position); 
		Vector3 direction = burrow.gameObject.transform.position - rabbit.transform.position; 
		var angle = Vector3.Angle (rabbit.transform.forward, direction); 
		Vector3 velocityDirection = (direction).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(velocityDirection);
		//float angle = Quaternion.Angle(Arrow.rectTransform.rotation, lookRotation); 
		float timeToComplete = angle / 150f;
		float donePercentage = Mathf.Min(1F, Time.deltaTime / timeToComplete);
		Arrow.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
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
	public void OnRestartClick()
	{
		var rabbit = GameObject.FindGameObjectWithTag ("Rabbit"); 
		rabbit.GetComponent<RabbitBehavior> ().CanAddPelettes = false; 
		SentPelette[] pellets = GameObject.FindObjectsOfType<SentPelette> (); 
		for(int i = 0; i< pellets.Length; i++)
		{
			Destroy(pellets[i].gameObject);  
		}

		rabbit.transform.position = new Vector3 (19f, 0f,0f);
		rabbit.GetComponent<RabbitBehavior> ().SetInitialPellets (); 
		RestartPannel.SetActive (false); 
		//TODO:add set up of genetic wolfs. 
	}
	public void OnReturnMainMenuClicked()
	{
		//TODO: save to player prefs
		Application.LoadLevel ("MainMenu"); 
	}
}

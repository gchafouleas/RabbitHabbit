using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class GamePlayController : MonoBehaviour {

	public GameObject worldPrefab; 
	public GameObject burrowPrefab; 
	public GameObject Rabbit; 
	public GameObject RestartPannel; 
	// Use this for initialization
	void Start ()
	{
		Instantiate (Rabbit); 
		Instantiate (worldPrefab); 
		Instantiate (burrowPrefab); 
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

		rabbit.transform.position = new Vector3 (5f, 0f,0f);
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

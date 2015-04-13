using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class GamePlayController : MonoBehaviour {

	public GameObject worldPrefab; 
	public GameObject burrowPrefab; 
	public GameObject Rabbit; 
	public GameObject RestartPannel; 
	public GameObject Wolf; 
	// Use this for initialization
	void Start ()
	{
		Instantiate (Rabbit); 
		Instantiate (worldPrefab); 
		Instantiate (burrowPrefab); 
		var wolf1 = (GameObject)Instantiate (Wolf); 
		wolf1.transform.position = new Vector3 (-60,0.8f,41); 
		var wolf2 = (GameObject)Instantiate (Wolf); 
		wolf2.transform.position = new Vector3 (-61,0.8f,41);
		var wolf3 = (GameObject)Instantiate (Wolf); 
		wolf3.transform.position = new Vector3 (-59,0.8f,41);
		var wolf4 = (GameObject)Instantiate (Wolf); 
		wolf4.transform.position = new Vector3 (-58, 0.8f, 41);
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

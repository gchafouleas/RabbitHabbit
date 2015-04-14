using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

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
		BreedHighestScoringWolves();
		GameObject[] allWolves = GameObject.FindGameObjectsWithTag("Wolf");
		allWolves[0].transform.position = new Vector3 (-60,0.8f,41); 
		allWolves[1].transform.position = new Vector3 (-61,0.8f,41);
		allWolves[2].transform.position = new Vector3 (-59,0.8f,41);
		allWolves[3].transform.position = new Vector3 (-58, 0.8f, 41);
			
			
	}
	public void OnReturnMainMenuClicked()
	{
		//TODO: save to player prefs
		Application.LoadLevel ("MainMenu"); 
	}

	private void BreedHighestScoringWolves()
	{
		GameObject[] allWolves = GameObject.FindGameObjectsWithTag("Wolf");
		//probably could do this with lambda/linq
		float highestScore = -1;
		GameObject bestWolf = null;
		float secondHighestScore = -1;
		GameObject secondBestWolf = null;

		foreach(GameObject wolf in allWolves)
		{
			float score = wolf.GetComponent<BehaviourMatrix>().pointsRebalanced;
			if(score > highestScore)
			{
				if (bestWolf)
					secondBestWolf = bestWolf;
				bestWolf = wolf;
				highestScore = score;
			}
			else if(score > secondHighestScore)
			{
				secondHighestScore = score;
				secondBestWolf = wolf;
			}
		}

		List<GameObject> childrenWolf = new List<GameObject>();
		foreach(GameObject wolf in allWolves)
		{
			if (wolf != bestWolf && wolf != secondBestWolf)
				childrenWolf.Add(wolf);
		}

		float[][,] newChildren = bestWolf.GetComponent<BehaviourMatrix>().createOffspring(secondBestWolf.GetComponent<BehaviourMatrix>().behaviourMatrix);
		childrenWolf[0].GetComponent<BehaviourMatrix>().behaviourMatrix = newChildren[0];
		childrenWolf[0].GetComponent<BehaviourMatrix>().ResetBehaviourMatrix();
		childrenWolf[1].GetComponent<BehaviourMatrix>().behaviourMatrix = newChildren[1];
		childrenWolf[1].GetComponent<BehaviourMatrix>().ResetBehaviourMatrix();
	}
}

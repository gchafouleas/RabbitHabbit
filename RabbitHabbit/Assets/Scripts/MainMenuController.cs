using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public bool learnedAI = false;
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	public void OnStartGameClick() 
	{
		Application.LoadLevel("GamePlay"); 
	}

	public void OnPremadeAIClick() 
	{
		learnedAI = true;
		Application.LoadLevel("GamePlay"); 
	}
}

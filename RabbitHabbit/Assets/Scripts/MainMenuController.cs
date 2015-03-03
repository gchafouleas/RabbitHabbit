using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	public void OnStartGameClick() 
	{
		Application.LoadLevel("GamePlay"); 
	}

	public void OnControlsClick() 
	{
		Application.LoadLevel("Controls"); 
	}
}

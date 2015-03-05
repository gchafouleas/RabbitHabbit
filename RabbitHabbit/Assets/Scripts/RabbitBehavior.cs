using UnityEngine;
using System.Collections;

public class RabbitBehavior : MonoBehaviour
{

	#region Variables
	[SerializeField]
	private Animation rabbitAnimation;

	private const int IDLE_ANIMATION = 0;
	private const int RUN_ANIMATION = 1;
	private const int WALK_ANIMATION = 2;

	[SerializeField]
	private float rotateSpeed = 3.0f;
	[SerializeField]
	private float Speed =1f;

	public bool IsHidden = false;
	#endregion 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			MoveCharacter(); 
		}
		
	}
	private void MoveCharacter() 
	{
	
	}
}

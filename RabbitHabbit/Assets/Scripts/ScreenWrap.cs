//This ScreenWrap script ensures the car wraps around the boundaries of the arena
//Works well but can be hard to see since agents do not seek the edges, they can only wander there
using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {
	
	public Wolf wolf;
	
	private bool isWrappingX = true;
	private bool isWrappingZ = true;

	//public float MSTest = 100f;
	
	void Start () {
		//rigidbody.velocity = new Vector3 (Random.value * MSTest, 0, Random.value * MSTest);
	}

	void FixedUpdate () {
		UpdateScreenWrap ();
	}

	void UpdateScreenWrap()
	{
		var newPosition = transform.position;
		int xLimit = 200; //these are the boundaries of the map, or they can be hardcoded below 
		int zLimit = 100;

		//frame skip
		if (isWrappingX)
		{
			isWrappingX = false;
			return;
		}
		if (isWrappingZ)
		{
			isWrappingZ = false;
			return;
		}//end frame skip
		
		if(isWrappingX && isWrappingZ)
		{
			return;
		}
		
		if (!isWrappingX && (transform.position.x >= 88.5f || transform.position.x <= 10f)) 
		{
			wolf.wanderDirection.x = -wolf.wanderDirection.x;
			isWrappingX = true;
		}

		
		if (!isWrappingZ && (transform.position.z >= -8.6f || transform.position.z <= -65.4f))
		{
			wolf.wanderDirection.z = -wolf.wanderDirection.z;
			isWrappingZ = true;
		}
		
		//transform.position = newPosition;
	}
}

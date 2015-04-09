using UnityEngine;
using System.Collections;

public class SentPelette : MonoBehaviour {

	private float sentIntensity = 1; 
	private const float TIMER = 10f; 
	private float timer =0; 
	public SentPelette NextSentPelette; 
	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Wolf"))
		{
			//TODO: send sentIntensity to wolf and next sent pelette to follow 
		}
	}

	void Update () 
	{
		timer += Time.deltaTime; 
		if(timer >= TIMER)
		{
			timer = 0; 
			sentIntensity -= 0.1f; 
			if(sentIntensity <= 0)
			{
				Destroy(this.gameObject); 
			}
		}
	}
}

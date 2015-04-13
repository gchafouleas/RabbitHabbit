using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourMatrixInitializer : MonoBehaviour {
			
	public float[,] Initialize()
	{
		//Use this class to set the initial values to balance the wolves
		
		float[,] toReturn = new float[GlobalVars.wolfStateLength,GlobalVars.wolfEventLength]
		{
			//EventRows  SeeRabbit,SmellRabbit,OneWolfNear,TwoWolvesNear,ThreeWolvesNear,InStateFor10Seconds, HearHowl, friendAttacks
							{0,			0,			0,			0,				0,				0,				0,				0}, //ChaseRabbit
							{0,			0,			0,			0,				0,				0,				0,				0}, //Howl
							{0,			0,			0,			0,				0,				0,				0,				0}, //Stalk
							{0,			0,			0,			0,				0,				0,				0,				0}, //Sniff
							{0,			0,			0,			0,				0,				0,				0,				0}, //Wander
		};

		return toReturn;
	}
}

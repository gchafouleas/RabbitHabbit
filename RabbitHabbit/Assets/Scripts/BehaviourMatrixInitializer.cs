using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourMatrixInitializer : MonoBehaviour {
			
	public float[,] Initialize()
	{
		//Use this class to set the initial values to balance the wolves, returns one at random
		float[,] toReturn;

		//if((float)Random.Range(-20,20)/10 )
		//if ((float)Random.Range(0, 1) == 0)
		if(true) //for testing purposes, test the balanced wolf
		{
			//balanced weights
			toReturn = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength]
			{
			//EventRows  SeeRabbit,SmellRabbit,OneWolfNear,TwoWolvesNear,ThreeWolvesNear,InStateFor30Seconds, HearHowl, friendAttacks, 
							{.8f,		0,		   .2f,		   .4f,			   .8f,				0,				.2f,		  .6f}, //ChaseRabbit
							{.6f,		0,		  -.2f,		  -.4f,			  -.6f,				0,				0,				0}, //Howl
							{.6f,		0,		   .2f,		  -.3f,		      -.6f,				0,				0,				0}, //Stalk
							{-2,		1,			0,			0,				0,				0,				0,				0}, //Sniff
							{-2,	   -2,			0,			0,				0,				3,				0,				0}, //Wander
							{-1,       -1,          0,          0,              0,              0,              2,             -1}
			};
		}
		else
		{
			//more randomized weights
			toReturn = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength]
			{
			//EventRows  SeeRabbit,SmellRabbit,OneWolfNear,TwoWolvesNear,ThreeWolvesNear,InStateFor30Seconds, HearHowl, friendAttacks, 
							{.4f,		0,		   .4f,		   .4f,			   .5f,				0,				.3f,		  .8f}, //ChaseRabbit
							{.2f,		0,		  -.6f,		  -.4f,			  -.2f,				.4f,			0,				0}, //Howl
							{.8f,		0,		   .2f,		  -.6f,		      -.6f,				.5f,			0,				0}, //Stalk
							{-1,		.4f,			0,			0,				 0,				0,				0,				0}, //Sniff
							{-2,	   -2,			0,			0,			   .2f,				3,				.2f,				0}, //Wander
							{-1,       -1,          0,          0,              0,              0,              2,             -1}  //runToHowl
			};
		}

		return toReturn;
	}
}

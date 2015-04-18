﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourMatrixInitializer : MonoBehaviour {
			
	public float[,] Initialize()
	{
		//Use this class to set the initial values to balance the wolves, returns one at random
		float[,] toReturn;

		//if((float)Random.Range(-20,20)/10 )
		//if ((float)Random.Range(0, 1) == 0)
		if(GameObject.FindObjectOfType<MainMenuController>().learnedAI) //for testing purposes, test the balanced wolf
		{
			//balanced weights
			toReturn = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength]
			{
			//EventRows  SeeRabbit,SmellRabbit,OneWolfNear,TwoWolvesNear,ThreeWolvesNear,InStateFor30Seconds, HearHowl, friendAttacks, InStateFor5Seconds
							{.8f,		0,		   .2f,		   .2f,			   .4f,				0,				.2f,		  .6f,			0}, //ChaseRabbit
							{.6f,		0,		  -.2f,		  -.4f,			  -.6f,				0,				0,				0,			0}, //Howl
							{.6f,		0,		   .2f,		  -.3f,		      -.6f,				0,				0,				0,			0}, //Stalk
							{-2,		1,			0,			0,				0,				0,				0,				0,			0}, //Sniff
							{-2,	   -2,			0,			0,				0,				3,				0,				0,			.4f}, //Wander
							{-1,       -1,          0,          0,              0,              0,              2,             -1,			0} //run to howl
			};
		}
		else
		{
			//more randomized weights
			toReturn = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength]
			{
			//EventRows  SeeRabbit,SmellRabbit,OneWolfNear,TwoWolvesNear,ThreeWolvesNear,InStateFor30Seconds, HearHowl, friendAttacks, InStateFor5Seconds
							{4f,		0,		   2.6f,		.4f,		  .4f,				0,				2.6f,		  .6f,			3.2f}, //ChaseRabbit
							{4.2f,		0,		  -.2f,		  -.4f,			  -.6f,				0,				0.2f,			0,			5}, //Howl
							{.6f,		0,		   .2f,		  -.3f,		      -.6f,				0,				0,				0,			0}, //Stalk
							{-2,		-2,			0,			0,				0,				3,				0,				0,			.4f}, //Sniff
							{-2,	   -2,			0,			0,				0,				3,				0,				0,			0}, //Wander
							{-.6f,     -1f,         3.4f,          0,              0,              0,           6.2f,             -1,	   4.2} //run to howl
			};
		}

		return toReturn;
	}
}

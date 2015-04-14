using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class BehaviourMatrix : MonoBehaviour {
	public BehaviourMatrixInitializer behaviourMatrixInitializer;
	public BehaviourRecorder behaviourRecorder;
	public List<GlobalVars.wolfEvent> encounterStarterEvents = new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.SeeRabbit, GlobalVars.wolfEvent.SmellRabbit};
	public Wolf wolfParent;
	public float pointsRebalanced = 0f;
	public GlobalVars.wolfState currentWolfState = GlobalVars.wolfState.Wander;

	public float[,] behaviourMatrix;
	public bool[] activeEvents;

	public void Start()
	{
		if(behaviourMatrixInitializer)
			behaviourMatrix = behaviourMatrixInitializer.Initialize();
		activeEvents = new bool[GlobalVars.wolfEventLength] { false, false, false, false, false, false, false, false, false };
	}

	public void ResetBehaviourMatrix()
	{
		currentWolfState = GlobalVars.wolfState.Wander;
		wolfParent.currentWolfState = GlobalVars.wolfState.Wander;
		pointsRebalanced = 0f;
		activeEvents = new bool[GlobalVars.wolfEventLength] { false, false, false, false, false, false, false, false, false };
	}
	public void RecieveEvent(GlobalVars.wolfEvent newEvent, bool eventIsActive)
	{
		//.Log("newEvent recieved: " + newEvent.ToString());
		try
		{
			activeEvents[(int)newEvent] = eventIsActive;
		} catch
		{
			Debug.Log("newEvent recieved: " + newEvent.ToString());
		}
		GlobalVars.wolfState newState =  CheckForOptimalState();
		if (currentWolfState != newState)
			ChangeState(newState);
		if(!behaviourRecorder.recorderIsActive && encounterStarterEvents.Contains(newEvent))
		{
			behaviourRecorder.StartRecorder();
		}
		if(behaviourRecorder.recorderIsActive)
		{
			behaviourRecorder.ReceiveEvent(currentWolfState, ActiveEventsToList());
		}
		//Send currentState and all ActiveEvents to recorder, if its active, itll accept and record the event list
	}

	private List<GlobalVars.wolfEvent> ActiveEventsToList()
	{
		List<GlobalVars.wolfEvent> toReturn = new List<GlobalVars.wolfEvent>();
		for(int i = 0; i < activeEvents.Length; i++)
		{
			if(activeEvents[i])
			{
				toReturn.Add((GlobalVars.wolfEvent)i);
			}
		}
		return toReturn;
	}
	public void RebalanceStates(bool encounterSuccessful, Dictionary<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> statesAndEventsInvolved)
	{
		foreach (KeyValuePair<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> kv in statesAndEventsInvolved)
		{
			foreach (GlobalVars.wolfEvent involvedEvents in kv.Value)
			{
				if (encounterSuccessful)
				{
					behaviourMatrix[(int)kv.Key, (int)involvedEvents] += GlobalVars.reward;
					pointsRebalanced += GlobalVars.reward;
				}
				else
				{
					behaviourMatrix[(int)kv.Key, (int)involvedEvents] -= GlobalVars.punishment;
					pointsRebalanced += GlobalVars.punishment;
				}
			}
		}
	}

	public float[][,] createOffspring(float[,] mate)
	{
		//given another wolves behaviourMatrix, produce two offspring using genetic algo
		float[][,] toReturn = new float[2][,];		
		float[,] child1 = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength];
		float[,] child2 = new float[GlobalVars.wolfStateLength, GlobalVars.wolfEventLength];

		for(int i_states = 0; i_states < GlobalVars.wolfStateLength; i_states++)
		{
			for(int i_events = 0; i_events < GlobalVars.wolfEventLength; i_events++)
			{
				if(i_states%2 == 0)
				{
					child1[i_states, i_events] = behaviourMatrix[i_states,i_events];
					child2[i_states, i_events] = mate[i_states, i_events];
				}
				else
				{
					child1[i_states, i_events] = mate[i_states, i_events];
					child2[i_states, i_events] = behaviourMatrix[i_states, i_events];
				}
			}
		}
		toReturn[0] = child1;
		toReturn[1] = child2;
		return toReturn;
	}

	public string Debug_OutputBehaviourMatrix()
	{ //set text allign right and it almost lines up
		string toOut = "";
		for (int i_states = 0; i_states < GlobalVars.wolfStateLength; i_states++)
		{
			toOut += ((GlobalVars.wolfState)i_states).ToString() + " , ";
		}
		toOut += "              ";
		toOut += "\r\n";


		for (int i_events = 0; i_events < GlobalVars.wolfEventLength; i_events++)
		{
			string toAdd = ((GlobalVars.wolfEvent)i_events).ToString();
			while(toAdd.Length < 28)
			{
				toAdd += " ";
			}
			toAdd += "|";
			toOut += toAdd;

			for (int i_states = 0; i_states < GlobalVars.wolfStateLength; i_states++)
			{
				//Debug.Log("string so far: " + toOut);
				toOut += behaviourMatrix[i_states, i_events] + "             ";
			}
			toOut += "\r\n";
		}
		return toOut;
	}
	private GlobalVars.wolfState CheckForOptimalState()
	{
		float[] scores = new float[GlobalVars.wolfStateLength]{0,0,0,0,0,0};
		//Chase and stalk 0,2 require wolf.rabbit
		//3 requires wolf.smeltRabbit
		//5 requires wolf.wolfThatHowledLocation
		for (int i_state = 0; i_state < GlobalVars.wolfStateLength; i_state++)
		{
			if(((i_state == 0 || i_state == 2) && (!wolfParent.rabbit)) || (i_state == 3 && !wolfParent.smeltRabbit) || (i_state == 5 && !wolfParent.wolfThatHowledLocation))
			{
				scores[i_state] = -5f;
				continue; //invalid states if no rabbit sighted or smelt
			}
			for (int i_event = 0; i_event < GlobalVars.wolfEventLength; i_event++)
				if (activeEvents[(int)i_event])
				{
					Debug.Log("Istate: " + i_state + "  i_event: " + i_event);
					scores[i_state] += behaviourMatrix[i_state, i_event];
				}
				
				
		}
				
		int winningState = 0;
		float winningScore = -999f;
		for(int i = 0; i < GlobalVars.wolfStateLength; i++)
		{
			if(scores[i] > winningScore)
			{
				
				winningScore = scores[i];
				winningState = i;
			}
		}
		
		return (GlobalVars.wolfState)winningState;
	}
	private void ChangeState(GlobalVars.wolfState newState)
	{
		currentWolfState = newState;
        wolfParent.StateWasUpdated(newState);		
	}

	
}

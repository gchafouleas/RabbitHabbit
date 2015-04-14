using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BehaviourRecorder : MonoBehaviour {

	//the recording tracks wolf events delimited by states that break perception of the rabbit
	public Dictionary<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> recording = new Dictionary<GlobalVars.wolfState,List<GlobalVars.wolfEvent>>();
	public BehaviourMatrix matrix;
	public bool recorderIsActive = false;

	void Start () {
	
	}

	void Update () {
	
	}

	/// <summary>
	/// Receives the event. Ensures new events in the same state are added to the dictionary properly
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="eventList">Event list.</param>
	public void ReceiveEvent(GlobalVars.wolfState state, List<GlobalVars.wolfEvent> eventList)
	{
		if (!recorderIsActive)
			return;

		List<GlobalVars.wolfEvent> temp = null;
		if (recording.TryGetValue (state, out temp)) //see if the dictionary entry (state) exists in the recording
		{
			foreach(GlobalVars.wolfEvent wolfEvent in eventList) //if it does, add each new wolfEvent to the list of events for that state
			{
				if (!temp.Contains (wolfEvent)) //if the event isn't already in the list
					temp.Add (wolfEvent);
			}
		}
		else
		{
			recording.Add (state, eventList);
		}
	}

	/// <summary>
	/// Starts the recorder.
	/// </summary>
	public void StartRecorder()
	{
		recording.Clear ();
		recorderIsActive = true;
	}

	/// <summary>
	/// Ends the recorder.
	/// </summary>
	public void EndRecorder(bool successfulHunt)
	{
		if (recorderIsActive)
		{
			matrix.RebalanceStates(successfulHunt, recording);
			recorderIsActive = false;
		}
		recording.Clear();
	}

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	/// <returns>A string that represents the current object.</returns>
	/// <filterpriority>2</filterpriority>
	public override string ToString ()
	{
		string temp = "";
		foreach (KeyValuePair<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> kv in recording)
		{
			temp += "State: " + kv.Key.ToString () + "; Events: ";
			foreach(GlobalVars.wolfEvent involvedEvents in kv.Value)
			{
				temp += involvedEvents.ToString () +",";
			}
			temp += "\n";
		}
		return temp;
	}
	
	public void Test()
	{
		BehaviourRecorder testRec = new BehaviourRecorder ();
		
		testRec.ReceiveEvent (GlobalVars.wolfState.Stalk, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.SmellRabbit });
		Debug.Log (testRec.ToString());
		testRec.ReceiveEvent (GlobalVars.wolfState.ChaseRabbit, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.SeeRabbit });
		Debug.Log (testRec.ToString());
		testRec.ReceiveEvent (GlobalVars.wolfState.Stalk, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.HearHowl, GlobalVars.wolfEvent.OneWolfNear });
		Debug.Log (testRec.ToString()); //new events are added to a state that already exists
		testRec.ReceiveEvent (GlobalVars.wolfState.Stalk, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.SmellRabbit });
		Debug.Log (testRec.ToString()); //an event which already exists is added to a state that already exists
	}
	
}

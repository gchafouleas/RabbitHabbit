using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BehaviourMatrixTest : MonoBehaviour {

	public BehaviourMatrix BMToTest;
	public Text testOutput;
	public void Test(){
		Dictionary<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> fakeRecording = new Dictionary<GlobalVars.wolfState,List<GlobalVars.wolfEvent>>();
		fakeRecording.Add(GlobalVars.wolfState.ChaseRabbit, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.HearHowl });

		BMToTest.RebalanceStates(false, fakeRecording);
		testOutput.text = BMToTest.Debug_OutputBehaviourMatrix();
	}
	
	public void Update()
	{
		//since the test needs to run AFTER the Start phase (since initialiers) have it run once on update
		//and disable itself instead
		Test();
		this.enabled = false;		
	}

}

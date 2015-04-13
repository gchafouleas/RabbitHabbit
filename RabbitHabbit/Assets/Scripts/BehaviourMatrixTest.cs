using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BehaviourMatrixTest : MonoBehaviour {

	public BehaviourMatrix BMToTest;
	//public Text testOutput;

	public void RunTest(){
		Dictionary<GlobalVars.wolfState, List<GlobalVars.wolfEvent>> fakeRecording = new Dictionary<GlobalVars.wolfState,List<GlobalVars.wolfEvent>>();
		fakeRecording.Add(GlobalVars.wolfState.ChaseRabbit, new List<GlobalVars.wolfEvent>() { GlobalVars.wolfEvent.HearHowl,GlobalVars.wolfEvent.friendAttak});
		BMToTest.RebalanceStates(false, fakeRecording);
		//testOutput.text = BMToTest.Debug_OutputBehaviourMatrix();
	}	
	

}

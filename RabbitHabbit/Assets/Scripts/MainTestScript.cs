using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MainTestScript : MonoBehaviour {

	public List<BehaviourMatrixTest> bmTests;
	public GeneticAlgoTest geneticTest;
	
	// Update is called once per frame
	void Update () {
		foreach (BehaviourMatrixTest bm in bmTests)
			bm.RunTest();
		geneticTest.RunTest();

		this.enabled = false;
	}
}

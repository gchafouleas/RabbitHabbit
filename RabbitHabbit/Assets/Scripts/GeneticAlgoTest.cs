using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeneticAlgoTest : MonoBehaviour {

	public BehaviourMatrix parent1;
	public BehaviourMatrix parent2;
	BehaviourMatrix child1 = new BehaviourMatrix();
	BehaviourMatrix child2 = new BehaviourMatrix();
	public Text parent1Text;
	public Text parent2Text;
	public Text child1Text;
	public Text child2Text;



	public void RunTest()
	{
		float[][,] newChildren = parent1.createOffspring(parent2.behaviourMatrix);
		child1.behaviourMatrix = newChildren[0];
		child2.behaviourMatrix = newChildren[1];

		parent1Text.text = parent1.Debug_OutputBehaviourMatrix();
		parent2Text.text = parent2.Debug_OutputBehaviourMatrix();
		child1Text.text = child1.Debug_OutputBehaviourMatrix();
		child2Text.text = child2.Debug_OutputBehaviourMatrix();
	}


}

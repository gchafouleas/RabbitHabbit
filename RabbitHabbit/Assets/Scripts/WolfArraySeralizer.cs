using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class WolfArraySeralizer : MonoBehaviour {
	/*seralizer, saves the wolf array onto a file or loads it */


	//Source:
	//http://stackoverflow.com/questions/20622005/xml-deserializer-to-deserialize-2-dimensional-array

	public int runThroughs = 0;

	public void RunThroughComplete()
	{
		runThroughs++;
		if (runThroughs == 40)
		{
			Wolf[] allWolves = GameObject.FindObjectsOfType<Wolf>();
			for (int i = 0; i < 4; i++)
			{
				WriteToFile(allWolves[i].behaviourMatrix.Debug_OutputBehaviourMatrix(), "wolf" + i.ToString());
			}
		}
	}

	public void WriteToFile(string toWrite, string filename)
	{
		System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
		file.WriteLine(toWrite);

		file.Close();
	}

	
	
}

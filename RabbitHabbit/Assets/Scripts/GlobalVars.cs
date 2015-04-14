using UnityEngine;
using System.Collections;

public class GlobalVars : MonoBehaviour {

	//Since Enum.GetLength doesn't seem to work in unity, any change to enums must alter the length vars below
	public enum wolfEvent { SeeRabbit, SmellRabbit, OneWolfNear, TwoWolvesNear, ThreeWolvesNear, InStateFor30Seconds, HearHowl, friendAttack};
	public enum wolfState { ChaseRabbit, Howl, Stalk, Sniff, Wander, RunToHowl };
	public const int wolfEventLength = 8;
	public const int wolfStateLength = 6;

	public const float punishment = .2f;
	public const float reward = .8f;

}

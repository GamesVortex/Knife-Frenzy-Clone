using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Challenge")]
public class Challenge : ScriptableObject {

	public int id;
	public int targetId;
	public Stage[] stages;
	public int coinCost;
}

[System.Serializable]
public class Stage{
	
	public int knifeCount;
	public bool doReverse;
	public float reverseAfterTime;
	public float rotationSpeed;
	public float reverseInterpolationInterval;
	public bool symetricPosition;
	public int coinAmount;
	public int preKnifeAmount;
}


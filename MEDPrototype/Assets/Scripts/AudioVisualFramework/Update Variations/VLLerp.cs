using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerController2D;

//Credit Chris W.

[RequireComponent(typeof(VirtualListener))]
public class VLLerp : MonoBehaviour {

	//public GameObject[] localWaypoints;
	//Vector3[] globalWaypoints;
	[SerializeField] private GameObject waypointA;
	[SerializeField] private GameObject waypointB;
	[SerializeField] private float minThreshold = 0;
	[SerializeField] private float maxThreshold = 5;
	[SerializeField] private bool averagePositions = true;
	[SerializeField] private float averagingAmount = 0.5f;
	private VirtualListener listener;

	//public float speed;
	//public bool cyclic;
	//public float waitTime;
	[Range(0, 2)]
	public float easeAmount;

	int fromWaypointIndex;
	//float percentBetweenWaypoints;
	float nextMoveTime;

	public void Start()
	{
		listener = GetComponent<VirtualListener>();
		/*
		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i].transform.position;// + transform.position;
		}*/
	}

	void Update()
	{
		Vector3 newPos = CalculateMovement();
		Vector3 velocity = newPos - transform.position;
		transform.Translate(velocity);
	}

	float Ease(float x)
	{
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}

	Vector3 CalculateMovement()
	{

		if (Time.time < nextMoveTime)
		{
			return Vector3.zero;
		}

		float audioPercent = (listener.getAudioVal() - minThreshold) / (maxThreshold - minThreshold);
		audioPercent = Mathf.Clamp01(audioPercent);

		float distanceBetweenWaypoints = Vector3.Distance(waypointA.transform.position, waypointB.transform.position);
		float percentBetweenWaypoints = audioPercent * distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp(waypointA.transform.position, waypointB.transform.position, easedPercentBetweenWaypoints);

		if (!averagePositions)
			return newPos;
		Vector3 avgPos = Vector3.Lerp(transform.position, newPos, averagingAmount * Time.deltaTime);

		return avgPos;
	}
}
using System;
using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinate { get; set; }
	public float Reward { get; set; }
	public GridWorld GridWorld { set => _gridWorld = value; }
	
	#endregion

	private GridWorld _gridWorld;

	private void Start()
	{
		StartCoroutine(Move(1.0f)); // TODO: Change magic number for wait time 
	}

	private IEnumerator Move(float waitTime)
	{
		yield return new WaitForSecondsRealtime(waitTime);
		//checkLegalMoves()
	}
}

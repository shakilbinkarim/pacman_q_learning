using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinates { get; set; }
	public float Reward { get; set; }

	#endregion
	
	private List<Moves> _moves;

	private void Start()
	{
		_moves = new List<Moves>();
		Move(1000.0f);  // TODO: Change magic number for wait time 
	}

	private void Move(float waitTime)
	{
		while (true)
		{
			_moves.Clear();
			float elapsedTime = 0.0f;
			while (elapsedTime < waitTime) elapsedTime += Time.deltaTime;
			_moves = Movement.GetValidMoves(Coordinates);
		}
	}
}

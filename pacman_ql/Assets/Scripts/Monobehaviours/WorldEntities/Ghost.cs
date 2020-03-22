using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinates { get; set; }
	public float Reward { get; set; }

	#endregion

	private List<Moves> _moves;
	private float _elapsedTime;
	private float _waitTime = 1.0f; // TODO: change from magic numer

	private void Start()
	{
		_moves = new List<Moves>();
		_elapsedTime = 0.0f;
	}

	private void Update()
	{
		if (_elapsedTime < _waitTime)
		{
			_elapsedTime += Time.deltaTime;
			return;
		}
		Move();
	}

	public void Move()
	{
		_moves.Clear();
		_elapsedTime = 0.0f;
		_moves = Movement.GetValidMoves(Coordinates);
		Moves selectedMove = _moves[Random.Range(0, _moves.Count)];
		Coordinates.Move(selectedMove);
	}
}

using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinates { get; set; }
	public GridWorld GridWorld { set => _gridWorld = value; }

	#endregion

	private List<Moves> _moves;
	private float _elapsedTime;
	private float _waitTime = 1.0f; // TODO: change from magic numer
	private GridWorld _gridWorld;
	private bool _canMove = false;

	private void Start()
	{
		_moves = new List<Moves>();
		_elapsedTime = 0.0f;
		_canMove = true;
	}

	//private void Update()
	//{
	//	if (_elapsedTime < _waitTime)
	//	{
	//		_elapsedTime += Time.deltaTime;
	//		return;
	//	}
	//	Move();
	//}

	private void OnEnable() => _gridWorld.MoveEntitiesEvent += Move;

	private void OnDisable() => _gridWorld.MoveEntitiesEvent -= Move;

	public void Move()
	{
		if (!_canMove) return;
		_moves.Clear();
		_elapsedTime = 0.0f;
		_moves = Movement.GetValidMoves(Coordinates);
		Moves selectedMove = _moves[Random.Range(0, _moves.Count)];
		Coordinates.Move(selectedMove);
	}

}

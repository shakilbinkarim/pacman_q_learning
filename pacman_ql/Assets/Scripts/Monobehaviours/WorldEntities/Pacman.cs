using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinates { get; set; }
	public GridWorld GridWorld { set => _gridWorld = value; }

	#endregion

	private List<Moves> _moves;
	private GridWorld _gridWorld;
	private bool _canMove = false;

	private void Start()
	{
		_moves = new List<Moves>();
		_canMove = true;
		GridWorld.MoveEntitiesEvent += this.Move;
	}

	private void OnDisable() => GridWorld.MoveEntitiesEvent -= this.Move;

	public void Move()
	{
		if (!_canMove) return;
		_moves.Clear();
		// Unity Random Range breaks Event action 
		// Moves selectedMove = _moves[Random.Range(0, _moves.Count)];
		_moves = Movement.GetValidMoves(Coordinates);
		System.Random random = new System.Random();
		int randomIndex = random.Next(0, _moves.Count);
		Moves selectedMove = _moves[randomIndex];
		Coordinates.Move(selectedMove);
	}

}

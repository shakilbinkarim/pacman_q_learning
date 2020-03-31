using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	#region Properties

	public Coordinates Coordinates { get; set; }
	public float Reward { get => GridWorld.Rewards.ghostReward;  }

	#endregion

	private List<Moves> _moves;
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
		_moves = Movement.GetValidMoves(Coordinates);
		System.Random random = new System.Random();
		int randomIndex = random.Next(0, _moves.Count);
		Moves selectedMove = _moves[randomIndex];
		Coordinates.Move(selectedMove);
	}
}

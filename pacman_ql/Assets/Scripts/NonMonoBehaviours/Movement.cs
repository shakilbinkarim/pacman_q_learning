using System.Collections.Generic;
using UnityEngine;

public enum Moves
{
	Up = 0,
	Down = 1,
	Left = 2,
	Right = 3
}

public static class Movement
{
	public static GridWorld GridWorld;

	public static List<Moves> GetValidMoves(Coordinates coordinates)
	{
		// TODO : return valid moves
		List<Wall> walls = GridWorld.Walls;
		List<Moves> moves = GetMovesList();
		Debug.Log(walls.Count);
		foreach (Wall wall in walls)
		{
			if (wall.Coordinates.X == coordinates.X + 1 && wall.Coordinates.Y == coordinates.Y) 
				moves.RemoveAll(move => move == Moves.Right);
			if (wall.Coordinates.X == coordinates.X - 1 && wall.Coordinates.Y == coordinates.Y) 
				moves.RemoveAll(move => move == Moves.Left);
			if (wall.Coordinates.Y == coordinates.Y + 1 && wall.Coordinates.X == coordinates.X) 
				moves.RemoveAll(move => move == Moves.Up);
			if (wall.Coordinates.Y == coordinates.Y - 1 && wall.Coordinates.X == coordinates.X) 
				moves.RemoveAll(move => move == Moves.Down);
		}
		return moves;
	}

	/// <summary>
	/// Returns a list containaing all moves. Possible or otherwise.
	/// </summary>
	/// <returns></returns>
	private static List<Moves> GetMovesList()
	{
		List<Moves> moves = new List<Moves>();
		moves.Add(Moves.Up);
		moves.Add(Moves.Down);
		moves.Add(Moves.Right);
		moves.Add(Moves.Left);
		return moves;
	}

}

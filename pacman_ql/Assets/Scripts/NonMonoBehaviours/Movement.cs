using System;
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
		List<Moves> moves = GetMovesList();
#if true
		GenerateValidMovesFromGrid(coordinates, moves);
#else
		GenerateValidMovesFromWallsList(coordinates, moves);
#endif

		return moves;
	}

	private static void GenerateValidMovesFromGrid(Coordinates coordinates, List<Moves> moves) 
	{
		WorldStaticEntity[,] worldStaticEntities = GridWorld.WorldStaticEntities;
		if (CheckForWall(coordinates.X + 1, coordinates.Y, worldStaticEntities))
			moves.RemoveAll(move => move == Moves.Right);
		if (CheckForWall(coordinates.X - 1, coordinates.Y, worldStaticEntities))
			moves.RemoveAll(move => move == Moves.Left);
		if (CheckForWall(coordinates.X, coordinates.Y + 1, worldStaticEntities))
			moves.RemoveAll(move => move == Moves.Up);
		if (CheckForWall(coordinates.X, coordinates.Y - 1, worldStaticEntities))
			moves.RemoveAll(move => move == Moves.Down);
	}

	private static bool CheckForWall(int x, int y, WorldStaticEntity[,] worldStaticEntities) => 
		worldStaticEntities[x, y].Type == WorldStaticEntityType.Wall;

	[Obsolete("GenerateValidMovesFromWallsList has been depricated.")]
	private static void GenerateValidMovesFromWallsList(Coordinates coordinates, List<Moves> moves)
	{

		List<Wall> walls = GridWorld.Walls;
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

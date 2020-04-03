using System;
using System.Collections.Generic;
using UnityEngine;

public enum Move
{
	Up = 0,
	Down = 1,
	Left = 2,
	Right = 3
}

public static class Movement
{
	public static GridWorld GridWorld;

	public static List<Move> GetValidMoves(Coordinates coordinates)
	{
		List<Move> moves = GetMovesList();
#if true
		GenerateValidMovesFromGrid(coordinates, moves);
#else
		GenerateValidMovesFromWallsList(coordinates, moves);
#endif

		return moves;
	}

	private static void GenerateValidMovesFromGrid(Coordinates coordinates, List<Move> moves) 
	{
		WorldStationaryEntity[,] worldStaticEntities = GridWorld.WorldStationaryEntities;
		if (CheckForWall(coordinates.X + 1, coordinates.Y, worldStaticEntities))
			moves.RemoveAll(move => move == Move.Right);
		if (CheckForWall(coordinates.X - 1, coordinates.Y, worldStaticEntities))
			moves.RemoveAll(move => move == Move.Left);
		if (CheckForWall(coordinates.X, coordinates.Y + 1, worldStaticEntities))
			moves.RemoveAll(move => move == Move.Up);
		if (CheckForWall(coordinates.X, coordinates.Y - 1, worldStaticEntities))
			moves.RemoveAll(move => move == Move.Down);
	}

	private static bool CheckForWall(int x, int y, WorldStationaryEntity[,] worldStaticEntities) => 
		worldStaticEntities[x, y].Type == WorldStaticEntityType.Wall;

	[Obsolete("GenerateValidMovesFromWallsList has been depricated.")]
	private static void GenerateValidMovesFromWallsList(Coordinates coordinates, List<Move> moves)
	{

		List<Wall> walls = GridWorld.Walls;
		Debug.Log(walls.Count);
		foreach (Wall wall in walls)
		{
			if (wall.Coordinates.X == coordinates.X + 1 && wall.Coordinates.Y == coordinates.Y)
				moves.RemoveAll(move => move == Move.Right);
			if (wall.Coordinates.X == coordinates.X - 1 && wall.Coordinates.Y == coordinates.Y)
				moves.RemoveAll(move => move == Move.Left);
			if (wall.Coordinates.Y == coordinates.Y + 1 && wall.Coordinates.X == coordinates.X)
				moves.RemoveAll(move => move == Move.Up);
			if (wall.Coordinates.Y == coordinates.Y - 1 && wall.Coordinates.X == coordinates.X)
				moves.RemoveAll(move => move == Move.Down);
		}
	}

	/// <summary>
	/// Returns a list containaing all moves. Possible or otherwise.
	/// </summary>
	/// <returns></returns>
	private static List<Move> GetMovesList()
	{
		List<Move> moves = new List<Move>();
		moves.Add(Move.Up);
		moves.Add(Move.Down);
		moves.Add(Move.Right);
		moves.Add(Move.Left);
		return moves;
	}

}

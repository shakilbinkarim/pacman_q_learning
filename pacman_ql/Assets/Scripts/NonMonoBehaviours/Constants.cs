using UnityEngine;

public static class SortingLayerNames
{
	public const string Food = "food";
	public const string Ghost = "ghost";
	public const string Pacman = "pacman";
	public const string Wall = "wall";
}

public static class MapColors
{
	public static readonly Color WallColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	public static readonly Color SmallFoodColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
	public static readonly Color BigFoodColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
}
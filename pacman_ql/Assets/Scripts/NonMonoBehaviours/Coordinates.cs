using System;

[System.Serializable]
public class Coordinates
{
	private int _x;
	private int _y;
	
	public int X
	{
		get => _x;
		set => _x = (value < 0) ? 0 : value;
	}
	public int Y
	{
		get => _y;
		set => _y = (value < 0) ? 0 : value;
	}

	public Coordinates(int x, int y)
	{
		X = x;
		Y = y;
	}

	public void Move(Moves selectedMove)
	{
		switch (selectedMove)
		{
			case Moves.Up:
				Y += 1;
				break;
			case Moves.Down:
				Y -= 1;
				break;
			case Moves.Left:
				X -= 1;
				break;
			case Moves.Right:
				X += 1;
				break;
			default:
				break;
		}
	}
}

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

	public static bool operator ==(Coordinates coord1, Coordinates coord2) => (coord1.X == coord2.X && coord1.Y == coord2.Y);
	public static bool operator !=(Coordinates coord1, Coordinates coord2) => !(coord1.X == coord2.X && coord1.Y == coord2.Y);

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}
}

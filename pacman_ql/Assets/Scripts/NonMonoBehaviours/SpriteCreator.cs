using UnityEngine;

public static class SpriteCreator
{
	public static Sprite CreateSprite(Texture2D texture2D, int pixelPerUnit, Vector2 origin)
	{
		Rect rectangle = new Rect(0,0, texture2D.width, texture2D.height);
		return Sprite.Create(texture2D, rectangle, origin, pixelPerUnit);
	}
	
}

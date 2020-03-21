using System.Collections.Generic;
using UnityEngine;

public class GridWorld : MonoBehaviour
{

	#region SerializedFields

	[SerializeField] private Texture2D map;
	[SerializeField] private Texture2D ghostOneTexture;
	[SerializeField] private Texture2D ghostTwoTexture;
	[SerializeField] private Texture2D pacmanTexture;
	[SerializeField] private Texture2D wallTexture;
	[SerializeField] private Texture2D foodTexture; // TODO: Add food texture

	#endregion

	#region Sprites

	private Sprite _wallSprite;
    private Sprite _ghostOneSprite;
    private Sprite _ghostTwoSprite;
    private Sprite _pacmanSprite;
	private Sprite _foodSprite;
    
    #endregion

    #region WorldEntititesLists

    private List<Wall> _walls;
    private List<Ghost> _ghosts;
    private List<Food> _foods;
    
    #endregion

    private void Start()
    {
	    InitLists();
	    InitSprites();
	    CreateGridWorld();
    }

    private void InitLists()
    {
	    _walls = new List<Wall>();
	    _ghosts = new List<Ghost>();
	    _foods = new List<Food>();
    }

    private void InitSprites()
    {
		_foodSprite = SpriteCreator.CreateSprite(foodTexture, foodTexture.width, Vector2.zero);
		_wallSprite = SpriteCreator.CreateSprite(wallTexture, wallTexture.width, Vector2.zero);
	    _ghostOneSprite = SpriteCreator.CreateSprite(ghostOneTexture, ghostOneTexture.width, Vector2.zero);
	    _ghostTwoSprite = SpriteCreator.CreateSprite(ghostTwoTexture, ghostOneTexture.width, Vector2.zero);
	    _pacmanSprite = SpriteCreator.CreateSprite(pacmanTexture, pacmanTexture.width, Vector2.zero);
    }
    
    private void CreateGridWorld()
    {
	    for (int i = 0; i < map.width; i++)
	    {
		    for (int j = 0; j < map.height; j++) CreateBlock(i, j);
	    }
    }

    private void CreateBlock(int x, int y)
    {
	    Color pixelColor = map.GetPixel(x, y);
	    if (pixelColor == Color.red) CreateWall(x, y); // TODO: separate map label colors to a static class 
	    else if (pixelColor == new Color(1.0f, 1.0f, 0.0f, 1.0f)) CreateSmallFood(x, y);
	    else if (pixelColor == Color.blue) CreateBigFood(x, y);
    }

    private void CreateBigFood(int x, int y)
    {
		// TODO: create Big food
		GameObject bigFoodGameObj = new GameObject($"big_food_{x}_{y}");
		SpriteRenderer spriteRenderer = bigFoodGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _foodSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Food);
		spriteRenderer.color = Color.gray;
		Food food = bigFoodGameObj.AddComponent<Food>();
		food.Coordinates = new Coordinates(x, y);
		_foods.Add(food);
		bigFoodGameObj.transform.SetParent(gameObject.transform);
		bigFoodGameObj.transform.localPosition = new Vector3(food.Coordinates.X, food.Coordinates.Y, gameObject.transform.position.z);
	}

    private void CreateSmallFood(int x, int y)
    {
		GameObject smallFoodGameObj = new GameObject($"small_food_{x}_{y}");
		SpriteRenderer spriteRenderer = smallFoodGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _foodSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Food);
		spriteRenderer.color = Color.magenta;
		Food food = smallFoodGameObj.AddComponent<Food>();
		food.Coordinates = new Coordinates(x, y);
		_foods.Add(food);
		smallFoodGameObj.transform.SetParent(gameObject.transform);
		smallFoodGameObj.transform.localPosition = new Vector3(food.Coordinates.X, food.Coordinates.Y, gameObject.transform.position.z);
	}

    private void CreateWall(int x, int y)
    {
	    GameObject wallGameObj = new GameObject($"wall_{x}_{y}");
	    SpriteRenderer spriteRenderer = wallGameObj.AddComponent<SpriteRenderer>();
	    spriteRenderer.sprite = _wallSprite;
	    spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Wall);
	    spriteRenderer.color = Color.cyan;
	    Wall wall = wallGameObj.AddComponent<Wall>();
	    wall.Coordinates = new Coordinates(x, y);
	    _walls.Add(wall);
	    wallGameObj.transform.SetParent(gameObject.transform);
	    wallGameObj.transform.localPosition = new Vector3(wall.Coordinates.X, wall.Coordinates.Y, gameObject.transform.position.z);
    }
    
}

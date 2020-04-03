using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


public class GridWorld : MonoBehaviour
{
	#region Properties
	public List<Food> RemoveFoodList { get; set; }
	public List<Wall> Walls { get => _walls; }
	public List<Ghost> Ghosts { get => _ghosts; }
	public Pacman Pacman => _pacman;
	public WorldStationaryEntity[,] WorldStationaryEntities { get => _wordlStaticEntities; }
	#endregion

	#region EventActions

	public static event Action MoveEntitiesEvent;

	#endregion
	
	public static Rewards Rewards;
	public static HyperParameters HyperParameters;
	public static Qfunction Qfunction;

	#region SerializedFields

	[SerializeField] Rewards rewards;
	[SerializeField] HyperParameters hyperParameters;
	[SerializeField] private Texture2D map;
	[SerializeField] private Texture2D pacmanTexture;
	[SerializeField] private Texture2D wallTexture;
	[SerializeField] private Texture2D foodTexture;
	[Header("Ghosts")]
	[Space(10)]
	[SerializeField] private Texture2D[] ghostTextures;
	[SerializeField] private int numberOfGhosts;
	#endregion

	#region Sprites

	private Sprite _wallSprite;
	private Sprite[] _ghostSprites;
	private Sprite _pacmanSprite;
	private Sprite _foodSprite;

	#endregion

	#region WorldEntititesLists


	private List<Wall> _walls;
	private List<Ghost> _ghosts;
	private List<Food> _foods;
	private Pacman _pacman;

	#endregion

	private WorldStationaryEntity[,] _wordlStaticEntities;
	private Timer _timer;

	private void Start()
	{
		Rewards = rewards;
		HyperParameters = hyperParameters;
		InitLists();
		InitGrid();
		InitSprites();
		CreateGridWorld();
		CreateGhosts();
		CreatePacman();
		Movement.GridWorld = this;
		RewardManager.Instance.GridWorld = this;
		Qfunction = new Qfunction(_pacman.Coordinates, GetGhostCoordinates(), WorldStationaryEntities);
	}

	public List<Coordinates> GetGhostCoordinates() 
	{
		List<Coordinates> ghostCoordinates = new List<Coordinates>();
		foreach (Ghost ghost in _ghosts)
		{
			ghostCoordinates.Add(ghost.Coordinates);
		}
		return ghostCoordinates;
	}


	private void StartUpdateEntitiesTimer()
	{
		_timer = new Timer(50);
		_timer.AutoReset = true;
		_timer.Elapsed += UpdateEntitiesCoordinate;
		_timer.Start();
	}

	private void UpdateEntitiesCoordinate(object sender, ElapsedEventArgs e) {
		MoveEntitiesEvent?.Invoke();
		RewardManager.Instance.CalculateCurrentReward(_pacman.Coordinates);
		Qfunction.UpdateQValue(_pacman.Coordinates, GetGhostCoordinates());
		//Debug.Log(RewardManager.Instance.CurrentReward);
	}

	private void OnEnable() => StartUpdateEntitiesTimer();

	private void OnDisable()
	{
		_timer.Elapsed -= UpdateEntitiesCoordinate;
	}

	private void InitGrid()
	{
		_wordlStaticEntities = new WorldStationaryEntity[map.width, map.height];
	}

	private void InitLists()
	{
		RemoveFoodList = new List<Food>();
		_walls = new List<Wall>();
		_ghosts = new List<Ghost>();
		_foods = new List<Food>();
	}

	private void Update()
	{
		RemoveFood();
		UpdateGhosts();
		UpdatePacman();
	}

	private void RemoveFood()
	{
		foreach (Food food in RemoveFoodList)
		{
			food.SpriteRenderer.enabled = false;
		}
		RemoveFoodList.Clear();
	}

	private void UpdatePacman()
	{
		_pacman.gameObject.transform.localPosition = new Vector2(_pacman.Coordinates.X, _pacman.Coordinates.Y);
	}

	private void UpdateGhosts()
	{
		foreach (Ghost ghost in _ghosts)
		{
			ghost.gameObject.transform.localPosition = new Vector2(ghost.Coordinates.X, ghost.Coordinates.Y);
		}
	}

	private void InitSprites()
	{
		_foodSprite = SpriteCreator.CreateSprite(foodTexture, foodTexture.width * 2, Vector2.zero);
		_wallSprite = SpriteCreator.CreateSprite(wallTexture, wallTexture.width, Vector2.zero);
		_pacmanSprite = SpriteCreator.CreateSprite(pacmanTexture, pacmanTexture.width, Vector2.zero);
		InitGhostSprites();
	}

	private void InitGhostSprites()
	{
		_ghostSprites = new Sprite[numberOfGhosts];
		for (int i = 0; i < numberOfGhosts; i++)
		{
			if (ghostTextures.Length <= 0) return;
			Texture2D ghostTexture = ghostTextures[i % ghostTextures.Length];
			_ghostSprites[i] = SpriteCreator.CreateSprite(ghostTexture, ghostTexture.width, Vector2.zero);
		}
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
		if (pixelColor == MapColors.WallColor) CreateWall(x, y);
		else if (pixelColor == MapColors.SmallFoodColor) CreateSmallFood(x, y);
		else if (pixelColor == MapColors.BigFoodColor) CreateBigFood(x, y);
	}
	private void CreateWall(int x, int y)
	{
		GameObject wallGameObj = new GameObject($"wall_{x}_{y}");
		SpriteRenderer spriteRenderer = wallGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _wallSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Wall);
		spriteRenderer.color = Color.white;
		Wall wall = wallGameObj.AddComponent<Wall>();
		wall.Coordinates = new Coordinates(x, y);
		WorldStationaryEntity worldStaticEntity = wallGameObj.AddComponent<WorldStationaryEntity>();
		worldStaticEntity.Type = WorldStaticEntityType.Wall;
		_wordlStaticEntities[x, y] = worldStaticEntity;

		_walls.Add(wall);
		wallGameObj.transform.SetParent(gameObject.transform);
		wallGameObj.transform.localPosition = new Vector3(wall.Coordinates.X, wall.Coordinates.Y, gameObject.transform.position.z);
	}
	
	#region Create Food
	private void CreateBigFood(int x, int y)
	{
		GameObject bigFoodGameObj = new GameObject($"big_food_{x}_{y}");
		SpriteRenderer spriteRenderer = bigFoodGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _foodSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Food);
		spriteRenderer.color = Color.yellow;
		Food food = bigFoodGameObj.AddComponent<Food>();
		food.Coordinates = new Coordinates(x, y);
		food.SpriteRenderer = spriteRenderer;
		WorldStationaryEntity worldEntity = bigFoodGameObj.AddComponent<WorldStationaryEntity>();
		worldEntity.Type = WorldStaticEntityType.BigFood;
		worldEntity.FoodObject = food;
		_wordlStaticEntities[x, y] = worldEntity;
		food.Type = FoodType.Big;
		_foods.Add(food);
		bigFoodGameObj.transform.SetParent(gameObject.transform);
		bigFoodGameObj.transform.localPosition = new Vector3(food.Coordinates.X + 0.25f, food.Coordinates.Y + 0.25f, gameObject.transform.position.z);
	}

	private void CreateSmallFood(int x, int y)
	{
		GameObject smallFoodGameObj = new GameObject($"small_food_{x}_{y}");
		SpriteRenderer spriteRenderer = smallFoodGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _foodSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Food);
		spriteRenderer.color = Color.green;
		Food food = smallFoodGameObj.AddComponent<Food>();
		food.Coordinates = new Coordinates(x, y);
		food.SpriteRenderer = spriteRenderer;
		WorldStationaryEntity worldEntity = smallFoodGameObj.AddComponent<WorldStationaryEntity>();
		worldEntity.Type = WorldStaticEntityType.SmallFood;
		worldEntity.FoodObject = food;
		_wordlStaticEntities[x, y] = worldEntity;
		food.Type = FoodType.Small;
		_foods.Add(food);
		smallFoodGameObj.transform.SetParent(gameObject.transform);
		smallFoodGameObj.transform.localPosition = new Vector3(food.Coordinates.X + 0.25f, food.Coordinates.Y + 0.25f, gameObject.transform.position.z);
	}
	#endregion

	#region Create Ghosts
	private void CreateGhosts()
	{
		for (int i = 0; i < numberOfGhosts; i++)
		{
			Coordinates coordinates = _foods[UnityEngine.Random.Range(0, _foods.Count)].Coordinates;
			CreateGhost(coordinates, i);
		}
	}

	private void CreateGhost(Coordinates coordinates, int spriteIndex)
	{
		GameObject ghostGameObj = new GameObject($"ghost_{spriteIndex}");
		SpriteRenderer spriteRenderer = ghostGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _ghostSprites[spriteIndex];
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Ghost);
		spriteRenderer.color = Color.white;
		Ghost ghost = ghostGameObj.AddComponent<Ghost>();
		ghost.Coordinates = coordinates;
		_ghosts.Add(ghost);
		ghostGameObj.transform.SetParent(gameObject.transform);
		ghostGameObj.transform.localPosition = new Vector3(ghost.Coordinates.X, ghost.Coordinates.Y, gameObject.transform.position.z);
	}
	#endregion

	#region Create Pacman
	private void CreatePacman()
	{
		GameObject pacmanGameObj = new GameObject($"pacman");
		SpriteRenderer spriteRenderer = pacmanGameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = _pacmanSprite;
		spriteRenderer.sortingLayerID = SortingLayer.NameToID(SortingLayerNames.Pacman);
		spriteRenderer.color = Color.white;
		Pacman pacman = pacmanGameObj.AddComponent<Pacman>();
		pacman.GridWorld = this;
		_pacman = pacman;
		Coordinates coordinates = CalcPacmanCoordinate();
		pacman.Coordinates = coordinates;
		pacmanGameObj.transform.SetParent(gameObject.transform);
		pacmanGameObj.transform.localPosition = new Vector3(pacman.Coordinates.X, pacman.Coordinates.Y, gameObject.transform.position.z);
	}

	private Coordinates CalcPacmanCoordinate()
	{
		Coordinates coordinates = new Coordinates(0, 0);
		bool loop = true;
		while (loop)
		{
			loop = false;
			coordinates = _foods[UnityEngine.Random.Range(0, _foods.Count)].Coordinates;
			foreach (Ghost ghost in _ghosts)
			{
				if (ghost.Coordinates == coordinates) loop = true;
			}
		}
		return coordinates;
	}
	#endregion
}

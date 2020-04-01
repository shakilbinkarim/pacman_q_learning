
using UnityEngine;

public class RewardManager : MonoBehaviour
{
	public float CurrentReward { get; set; }
	public GridWorld GridWorld { get; set; }
	private Coordinates _pacmanCoordinates;

	public static RewardManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	//public void Init() 
	//{
	//	GridWorld.CalculateReward += CalculateCurrentReward;
	//}
	/// <summary>
	/// Checks for overlap of pacman and ghosts.
	/// returns appropriate reward 
	/// </summary>
	private float CheckGhostOverlap() 
	{
		foreach (Ghost ghost in GridWorld.Ghosts)
		{
			if (ghost.Coordinates == _pacmanCoordinates) return ghost.Reward;
		}
		return 0.0f;
	}

	/// <summary>
	/// Checks for overlap of pacman and food pills based on Coordinates.
	/// Returns proper reward based on overlap with BigFood SmallFood or Empty square
	/// </summary>
	private float CheckFoodOverlap()
	{
		WorldStaticEntity[,] worldStaticEntities = GridWorld.WorldStaticEntities;
		var staticEntity = worldStaticEntities[_pacmanCoordinates.X, _pacmanCoordinates.Y];
		if (staticEntity.Type == WorldStaticEntityType.Wall) return -100.0f; // Should not come here
		else
		{
			Food food = staticEntity.FoodObject;
			if (food) {
				float reward = food.Reward;
				food.Type = FoodType.Empty;
				GridWorld.RemoveFoodList.Add(food);
				return reward; 
			}
			else Debug.LogError($"Food not found at {_pacmanCoordinates.X}, {_pacmanCoordinates.Y}");
		}
		return 0;
	}

	public void CalculateCurrentReward(Coordinates pacmanCoordinates) 
	{
		_pacmanCoordinates = pacmanCoordinates;
		CurrentReward = 0.0f;
		CurrentReward = CheckGhostOverlap() + CheckFoodOverlap();
	}

}

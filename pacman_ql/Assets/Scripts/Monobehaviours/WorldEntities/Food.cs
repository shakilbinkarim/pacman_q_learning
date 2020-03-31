using System;
using UnityEngine;

public enum FoodType
{
	Small,
	Big,
	Empty
}

public class Food : MonoBehaviour
{
	// TODO: implement food class
	public Coordinates Coordinates { get;  set; }
	public FoodType Type { get; set; }
	public float Reward {
		get
		{
			float reward = 0;
			switch (Type)
			{
				case FoodType.Small:
					reward = GridWorld.Rewards.normalFoodReward;
					break;
				case FoodType.Big:
					reward = GridWorld.Rewards.bigFoodtReward;
					break;
				case FoodType.Empty:
					reward = GridWorld.Rewards.emptyFoodReward;
					break;
			}
			return reward;
		}

		// TODO: replace magic numbers
	}
	public SpriteRenderer SpriteRenderer { get; set; }


	//public void DisableSprite() => gameObject.GetComponent<SpriteRenderer>().enabled = false;
}

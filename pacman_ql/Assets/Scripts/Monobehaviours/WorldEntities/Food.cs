using UnityEngine;

public enum FoodType
{
	Small,
	Big
}

public class Food : MonoBehaviour
{
	// TODO: implement food class
	public Coordinates Coordinates { get;  set; }
	public FoodType Type { get; set; }
	public float Reward => Type == FoodType.Big ? 5.0f : 1.0f; // replace magic numbers
}

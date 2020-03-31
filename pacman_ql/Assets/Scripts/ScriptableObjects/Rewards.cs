using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "PacmanQl/Rewards")]
public class Rewards : ScriptableObject
{
    public float ghostReward;

    [Header("Food")][Space(10)]
    public float normalFoodReward;
    public float bigFoodtReward;
    public float emptyFoodReward;

}

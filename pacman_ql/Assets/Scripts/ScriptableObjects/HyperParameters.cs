using UnityEngine;

[CreateAssetMenu(fileName = "HyperParameters", menuName = "PacmanQl/Hyper Parameters")]
public class HyperParameters : ScriptableObject
{
    [Header("Learning Parameters")]
    [Space(10)]
    public float learningRate;
    public float discountRate;
}

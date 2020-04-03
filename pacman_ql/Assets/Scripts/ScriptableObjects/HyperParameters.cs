using UnityEngine;

[CreateAssetMenu(fileName = "HyperParameters", menuName = "PacmanQl/Hyper Parameters")]
public class HyperParameters : ScriptableObject
{
    [Header("Learning Parameters")]
    [Space(10)]
    [Range(0.0f, 1.0f)]
    public float epsilon;
    [Range(0.0f, 1.0f)]
    public float epsilonDecayRate;
    [Range(0.0f, 1.0f)]
    public float learningRate;
    [Range(0.0f, 1.0f)]
    public float discountRate;
}

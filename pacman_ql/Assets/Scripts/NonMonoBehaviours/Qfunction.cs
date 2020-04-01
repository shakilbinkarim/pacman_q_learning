using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qfunction
{
    #region Weights
    
    private float _wDistToGhost1;
    private float _wDistToGhost2;
    private float _wDistToFood;
    
    #endregion

    #region Hyper Parameters

    public float LearningRate { get => _learningRate;
        set
        {
            if (!(value > 1.0 || value < 0.0)) _discountRate = value;
        }
    }
    public float DiscountRate { get => _discountRate; 
        set 
        {
            if (!(value > 1.0 || value < 0.0)) _discountRate = value;
        }   
    }
    private float _learningRate;
    private float _discountRate;

    #endregion

    private float _prevQValue;
    private Coordinates _prevPacmanCoordinates;
    private List<Coordinates> _prevGhostCoordinates;
    private Coordinates _prevFoodCoordinates;

    public float CalcQValue(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates)
    {
        float QValue = _wDistToGhost1 * DistanceToGhost(pacmanCoordinates, ghostCoordinates[0]) +
                 _wDistToGhost2 * DistanceToGhost(pacmanCoordinates, ghostCoordinates[1]) +
                 _wDistToFood * DistanceToNearestFood(pacmanCoordinates);
        return QValue;
    }

    public void UpdateQValue(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates)
    {
        float newQValue = CalcQValue(pacmanCoordinates, ghostCoordinates);
        float qDifference = newQValue + RewardManager.Instance.CurrentReward - _prevQValue;
        _wDistToGhost1 += qDifference * DistanceToGhost(_prevPacmanCoordinates, _prevGhostCoordinates[0]);
        _wDistToGhost2 += qDifference * DistanceToGhost(_prevPacmanCoordinates, _prevGhostCoordinates[1]);
        _wDistToFood += qDifference * DistanceToNearestFood(_prevFoodCoordinates);
        _prevQValue = newQValue;
    }

    private float DistanceToGhost(Coordinates pacmanCoordinates, Coordinates ghostCoordinates) => pacmanCoordinates - ghostCoordinates;

    private float DistanceToNearestFood(Coordinates pacmanCoordinates) => FindNearestFood(pacmanCoordinates) - pacmanCoordinates;

    private Coordinates FindNearestFood(Coordinates pacmanCoordinates)
    {
        throw new System.NotImplementedException();
    }

    public Moves ActionSelector(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates, List<Moves> moveList)
    {
        return Moves.Up;
    }

}

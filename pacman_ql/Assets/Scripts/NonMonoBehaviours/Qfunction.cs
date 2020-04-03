using System.Collections.Generic;
using UnityEngine;

public class QState
{
    internal Coordinates PacmanCoordinates;
    internal List<Coordinates> GhostCoordinates;
    internal Coordinates FoodCoordinates;

    public QState(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates, Coordinates foodCoordinates)
        => SetState(pacmanCoordinates, ghostCoordinates, foodCoordinates);

    internal void SetState(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates, Coordinates foodCoordinates)
    {
        PacmanCoordinates = pacmanCoordinates;
        GhostCoordinates = ghostCoordinates;
        FoodCoordinates = foodCoordinates;
    }
}


public class Qfunction
{
    #region Weights

    private float _wDistToGhost1;
    private float _wDistToGhost2;
    private float _wDistToFood;

    #endregion

    #region Hyper Parameters
    private float _learningRate;
    private float _discountRate;
    private float _epsilon;
    private float _epsilonDecayRate;
    #endregion

    private float _prevQValue;
    private float _maxDistance;
    private QState _prevQState;
    private WorldStationaryEntity[,] _worldStationaryEntities;

    public Qfunction(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates, WorldStationaryEntity[,] worldStationaryEntities)
    {
        _wDistToFood = 0;
        _wDistToGhost1 = 0;
        _wDistToGhost2 = 0;

        _worldStationaryEntities = worldStationaryEntities;
        _maxDistance = worldStationaryEntities.GetLength(0) + worldStationaryEntities.GetLength(1);

        _prevQState = new QState(pacmanCoordinates, ghostCoordinates, FindNearestFood(pacmanCoordinates));
        _prevQValue = CalcQValue(_prevQState);
        _learningRate = GridWorld.HyperParameters.learningRate;
        _discountRate = GridWorld.HyperParameters.discountRate;
        _epsilon = GridWorld.HyperParameters.epsilon;
        _epsilonDecayRate = GridWorld.HyperParameters.epsilonDecayRate;
    }

    private float CalcQValue(QState qState)
    {
        float QValue = _wDistToGhost1 * DistanceToGhost(qState.PacmanCoordinates, qState.GhostCoordinates[0]) +
                 _wDistToGhost2 * DistanceToGhost(qState.PacmanCoordinates, qState.GhostCoordinates[1]) +
                 _wDistToFood * DistanceToNearestFood(qState.PacmanCoordinates, qState.FoodCoordinates);
        return QValue;
    }

    public void UpdateQValue(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates)
    {
        QState newQState = new QState(pacmanCoordinates, ghostCoordinates, FindNearestFood(pacmanCoordinates));
        float newQValue = _discountRate*CalcQValue(newQState);
        float qDifference = _learningRate*(newQValue + RewardManager.Instance.CurrentReward - _prevQValue);
        _wDistToGhost1 += qDifference * DistanceToGhost(_prevQState.PacmanCoordinates, _prevQState.GhostCoordinates[0]);
        _wDistToGhost2 += qDifference * DistanceToGhost(_prevQState.PacmanCoordinates, _prevQState.GhostCoordinates[1]);
        _wDistToFood += qDifference * DistanceToNearestFood(_prevQState.PacmanCoordinates , _prevQState.FoodCoordinates);
        _epsilon *= _epsilonDecayRate;
        _prevQValue = CalcQValue(newQState);
        // TODO: very memory inefficient
        _prevQState = newQState;
        //Debug.Log(_prevQValue);
    }

    private float DistanceToGhost(Coordinates pacmanCoordinates, Coordinates ghostCoordinates) => 
        System.Math.Abs(pacmanCoordinates - ghostCoordinates)/_maxDistance;

    private float DistanceToNearestFood(Coordinates pacmanCoordinates, Coordinates foodCoordinates) => 
        -System.Math.Abs(foodCoordinates - pacmanCoordinates)/_maxDistance;

    private Coordinates FindNearestFood(Coordinates pacmanCoordinates)
    {
        Queue<Coordinates> frontier = new Queue<Coordinates>();
        bool[,] visitedNodes = new bool[_worldStationaryEntities.GetLength(0), _worldStationaryEntities.GetLength(1)];
        for (int i = 0; i < visitedNodes.GetLength(0); i++)
        {
            for (int j = 0; j < visitedNodes.GetLength(1); j++)
            {
                visitedNodes[i, j] = false;
            }
        }
        Coordinates rootNodeCoord = new Coordinates(0, 0);
        visitedNodes[pacmanCoordinates.X, pacmanCoordinates.Y] = true;
        frontier.Enqueue(pacmanCoordinates);

        while (frontier.Count != 0)
        {
            rootNodeCoord = frontier.Dequeue();
            Coordinates exploreNodeCoord = new Coordinates(rootNodeCoord.X, rootNodeCoord.Y);
            List<Move> moveList = Movement.GetValidMoves(rootNodeCoord);
            foreach (Move move in moveList)
            {
                // reset exploreNode after each move
                exploreNodeCoord.X = rootNodeCoord.X;
                exploreNodeCoord.Y = rootNodeCoord.Y;
                exploreNodeCoord.Move(move);
                bool isNodeVisited = visitedNodes[exploreNodeCoord.X, exploreNodeCoord.Y];
                if (_worldStationaryEntities[exploreNodeCoord.X, exploreNodeCoord.Y].Type == WorldStaticEntityType.Empty && isNodeVisited == false)
                {
                    frontier.Enqueue(rootNodeCoord);
                    visitedNodes[exploreNodeCoord.X, exploreNodeCoord.Y] = true;
                } 
                else { return exploreNodeCoord; }
                    
            }

        }
        return pacmanCoordinates;
    }

    public Move ActionSelector(Coordinates pacmanCoordinates, List<Coordinates> ghostCoordinates)
    {
        System.Random random = new System.Random();
        bool set = false;
        List<Move> moveList = Movement.GetValidMoves(pacmanCoordinates);
        Move selectedMove = moveList[0];
        float rand = (float)random.NextDouble();
        if (_epsilon > rand)
        {
            set = true;
            int randomIndex = random.Next(0, moveList.Count);
            selectedMove = moveList[randomIndex];
            return selectedMove;
        }
        Coordinates nearestFood = FindNearestFood(pacmanCoordinates);
        QState tempQState = new QState(pacmanCoordinates, ghostCoordinates, nearestFood);
        float qValue = 0.0f;
        Coordinates pacmanCoordCopy = new Coordinates(pacmanCoordinates.X, pacmanCoordinates.Y);
        foreach (Move move in moveList)
        {
            pacmanCoordCopy.Move(move);
            nearestFood = FindNearestFood(pacmanCoordinates);
            tempQState.SetState(pacmanCoordCopy, ghostCoordinates, nearestFood);
            float tempQValue = CalcQValue(tempQState);
            if (qValue < tempQValue)
            {
                qValue = tempQValue;
                selectedMove = move;
                set = true;
            }
            pacmanCoordCopy.X = pacmanCoordinates.X;
            pacmanCoordCopy.Y = pacmanCoordinates.Y;
        }
        if (set) 
            Debug.Log($"set {selectedMove}");
        else
            Debug.LogError("never Set an action BITCH!");
        return selectedMove;
    }

}

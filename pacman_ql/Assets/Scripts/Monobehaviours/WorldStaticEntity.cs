using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorldStaticEntityType
{
    Wall,
    Empty,
    BigFood,
    SmallFood
}


public class WorldStationaryEntity : MonoBehaviour
{
    public WorldStaticEntityType Type { 
        get => _type; 
        set => _type = value; 
    }

    public Food FoodObject { get; set; }

    private WorldStaticEntityType _type;
}

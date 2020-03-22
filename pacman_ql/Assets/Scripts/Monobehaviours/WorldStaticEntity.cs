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


public class WorldStaticEntity : MonoBehaviour
{
    public WorldStaticEntityType Type { 
        get => _type; 
        set => _type = value; 
    }

    private WorldStaticEntityType _type;

    ////private void Start() => _type = WorldStaticEntityType.Empty;

}

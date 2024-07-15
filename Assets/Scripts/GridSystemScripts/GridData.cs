using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, int ID, GameObject gameObject)
    {
        PlacementData data = new PlacementData(gridPosition, ID, gameObject);
        if (placedObjects.ContainsKey(gridPosition))
            throw new Exception($"Dictionary already contains this cell position {gridPosition}");
        placedObjects[gridPosition] = data;
    }

    public GameObject GetGameObjectAt(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return null;
        return placedObjects[gridPosition].gameObject;
    }

    public int GetObjectIDAt(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].ID;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        placedObjects.Remove(gridPosition);
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition)
    {
        //ya no recuerdo para que servia esta funcion, pero aca esta por si acaso
        if (placedObjects.ContainsKey(gridPosition))
            return false;
        return true;
    }
}

public class PlacementData
{
    //creo que este vector ni hace falta, porque segun yo lo que usa para encontrar los vectores esta fuera del placement data, pero por ahora lo dejo por si acaso
    public Vector3Int occupiedPosition;
    public int ID { get; private set; }
    public GameObject gameObject { get; private set; }

    public PlacementData(Vector3Int occupiedPosition, int iD, GameObject gameObject)
    {
        this.occupiedPosition = occupiedPosition;
        ID = iD;
        this.gameObject = gameObject;
    }
}

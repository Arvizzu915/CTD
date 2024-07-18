using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    [SerializeField]
    private Vector3Int minMapPos;
    [SerializeField]
    private Vector3Int maxMapPos;
    [SerializeField]
    private int[] mapObjectsID;
    [SerializeField]
    private int[] mapObjectsAmmount;
    [SerializeField]
    private Vector3Int[] mapObjectsPositions;
    
    [SerializeField]
    ObjectPlacer objectPlacer;

    [SerializeField]
    private Grid grid;

    private GridData placeableObjectsData, mapObjectsData;


    void Start()
    {

    }

    public void SetGridData(GridData placeableObjectsData, GridData mapObjectsData)
    {
        this.placeableObjectsData = placeableObjectsData;
        this.mapObjectsData = mapObjectsData;
        SetMapObjects();
    }

    private void SetMapObjects()
    {
        PlaceObject(501, new Vector3Int(5, 0, 5), 0f, 0);
        PlaceObject(501, new Vector3Int(5, 0, 6), 0f, 0);
        PlaceObject(501, new Vector3Int(5, 0, 7), 0f, 0);
        PlaceObject(501, new Vector3Int(5, 0, 8), 0f, 0);
        PlaceObject(501, new Vector3Int(5, 0, 9), 0f, 0);

        PlaceObject(0, new Vector3Int(5, 0, 7), 1f, 0);
        PlaceObject(100, new Vector3Int(5, 0, 6), 1f, 0);
        PlaceObject(100, new Vector3Int(5, 0, 5), 1f, 0);

        PlaceObject(502, new Vector3Int(7, 0, 5), 0f, 0);
        PlaceObject(503, new Vector3Int(7, 0, 7), 0f, 200);
        PlaceObject(503, new Vector3Int(7, 0, 9), 0f, 204);
    }

    private void PlaceObject(int ID, Vector3Int gridPosition, float yOffSet, int itemID)
    {
        //int selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        //int newIndex = objectPlacer.PlaceObject(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), yOffSet, index);
        //placeableObjectsData.AddObjectAt(gridPosition, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].ID, newIndex);
        Vector3 worldPosition = grid.CellToWorld(gridPosition);
        worldPosition.y += yOffSet;
        GameObject newObject = objectPlacer.CreateNewObject(ID);
        objectPlacer.MoveObject(newObject, worldPosition);
        if(ID >= 500)
        {
            mapObjectsData.AddObjectAt(gridPosition, ID, newObject);
            newObject.GetComponent<BaseStationScript>().itemID = itemID;
        }
        else
        {
            placeableObjectsData.AddObjectAt(gridPosition, ID, newObject);
        }
    }
}

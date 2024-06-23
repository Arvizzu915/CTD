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

    [SerializeField]
    private PlaceableObjectsDatabaseSO placeableObjectsDatabase;
    [SerializeField]
    private StationsDatabaseSO stationsDatabase;

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
        PlaceStation(0, new Vector3Int(5, 0, 5), -1);
        PlaceStation(0, new Vector3Int(5, 0, 6), -1);
        PlaceStation(0, new Vector3Int(5, 0, 7), -1);
        PlaceStation(0, new Vector3Int(5, 0, 8), -1);
        PlaceStation(0, new Vector3Int(5, 0, 9), -1);

        PlaceObject(0, new Vector3Int(5, 0, 7), 1f, -1);
        PlaceObject(100, new Vector3Int(5, 0, 6), 1f, -1);
        PlaceObject(100, new Vector3Int(5, 0, 5), 1f, -1);

        PlaceStation(1, new Vector3Int(7, 0, 5), -1);
        PlaceStation(2, new Vector3Int(7, 0, 7), -1);
        PlaceStation(3, new Vector3Int(7, 0, 9), -1);
    }

    private void PlaceStation(int ID, Vector3Int gridPosition, int index)
    {
        int selectedObjectIndex = stationsDatabase.stationsData.FindIndex(data => data.ID == ID);
        int newIndex = objectPlacer.PlaceStation(stationsDatabase.stationsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), index);
        mapObjectsData.AddObjectAt(gridPosition, stationsDatabase.stationsData[selectedObjectIndex].Size, stationsDatabase.stationsData[selectedObjectIndex].ID, newIndex);
    }

    private void PlaceObject(int ID, Vector3Int gridPosition, float yOffSet, int index)
    {
        int selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        int newIndex = objectPlacer.PlaceObject(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), yOffSet, index);
        placeableObjectsData.AddObjectAt(gridPosition, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].ID, newIndex);
    }
}

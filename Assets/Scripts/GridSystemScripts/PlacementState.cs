using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    TowersDatabaseSO towersDatabase;
    GridData objectsData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, TowersDatabaseSO towersDatabase, GridData objectData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.towersDatabase = towersDatabase;
        this.objectsData = objectData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = towersDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(towersDatabase.objectsPlacementData[selectedObjectIndex].Prefab, towersDatabase.objectsPlacementData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPlacementPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;
        int index = objectPlacer.PlaceObject(towersDatabase.objectsPlacementData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = objectsData;
        selectedData.AddObjectAt(gridPosition, towersDatabase.objectsPlacementData[selectedObjectIndex].Size, towersDatabase.objectsPlacementData[selectedObjectIndex].ID, index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //como por ahora solo esta el gridData de las torres, aca solo igualamos
        GridData selectedData = objectsData;

        return selectedData.CanPlaceObjectAt(gridPosition, towersDatabase.objectsPlacementData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}

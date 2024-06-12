using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData objectsData;
    ObjectPlacer objectPlacer;

    public DefaultState(Grid grid, PreviewSystem previewSystem, GridData objectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectsData = objectsData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingDefaultPreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPlacementPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        //RemovingState quita un objeto del mapa o grid
        //PlacementState agrega un objeto en el mapa o grid
        //Aca en teoria solo debe utilizar los datos del objeto del mapa o grid para ganar o perder un objeto
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}

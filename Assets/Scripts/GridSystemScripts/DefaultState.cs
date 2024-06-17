using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData placeableObjectsData;
    GridData mapObjectsData;
    ObjectPlacer objectPlacer;

    public DefaultState(Grid grid, PreviewSystem previewSystem, GridData placeableObjectsData, GridData mapObjectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.placeableObjectsData = placeableObjectsData;
        this.mapObjectsData = mapObjectsData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingDefaultPreview(Vector2Int.one);
    }

    public void EndState()
    {
        previewSystem.StopShowingDefaultPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        //tengo sueño xd, pero aca iria cada cosa
    }

    //Esta funcion solia retornar bool, pero para reutilizar codigo, ahora retorna un int que significa:
    // 0 - No es valido (false)
    // 1 - Obtiene un objeto en su mano, y no afecta de ninguna manera al objeto en el mapa
    // 2 - Obtiene un objeto en su mano, y quita ese mismo objeto del mapa
    // 3 - Obtiene un objeto en su mano, y modifica el objeto del mapa
    private int CheckSelectionValidity(Vector3Int gridPosition)
    {
        int placeableObjectID = placeableObjectsData.GetObjectIDAt(gridPosition);
        int mapObjectID = mapObjectsData.GetRepresentationIndex(gridPosition);

        // Si no hay ningun objeto, y esta un dispensador, vale
        if (placeableObjectID == -1 && mapObjectID == 2)
            return 1;
        if ((placeableObjectID >= 0 && placeableObjectID < 500) && mapObjectID == 0)
        {
            // Puede agarrar cualquier objeto que esté en una mesa basica
            return 2;
        }
        else if ((placeableObjectID >= 100 && placeableObjectID < 400) && mapObjectID == 3)
        {
            //por ahora solo es el horno, pero si hay un plato, ingrediente u objeto que esta en una estacion, puede tomarlo
            return 3;
        }

        return 0;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int validity = CheckSelectionValidity(gridPosition);
        previewSystem.UpdateDefaultPreviewPosition(grid.CellToWorld(gridPosition), validity);
    }
}

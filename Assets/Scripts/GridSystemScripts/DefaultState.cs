using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : IBuildingState
{
    Grid grid;
    PreviewSystem previewSystem;
    InventorySystem inventorySystem;
    GridData placeableObjectsData;
    GridData mapObjectsData;
    ObjectPlacer objectPlacer;

    public DefaultState(Grid grid, PreviewSystem previewSystem, InventorySystem inventorySystem, GridData placeableObjectsData, GridData mapObjectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.inventorySystem = inventorySystem;
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
        int validity = CheckSelectionValidity(gridPosition);
        if (validity == 0)
            return;
        if (validity == 1) 
        {
            inventorySystem.GetObject(placeableObjectsData.GetObjectIDAt(gridPosition), placeableObjectsData.GetRepresentationIndex(gridPosition));
            objectPlacer.RemoveObjectAt(placeableObjectsData.GetRepresentationIndex(gridPosition));
            placeableObjectsData.RemoveObjectAt(gridPosition);
        }
        else if (validity == 2)
        {
            GameObject stationGameObject = objectPlacer.GetStationWithIndex(mapObjectsData.GetRepresentationIndex(gridPosition));
            if (stationGameObject != null)
            {
                int stationReturnedObject = stationGameObject.GetComponent<BaseStationScript>().OnAccessEmpty();
                if (stationReturnedObject != -1)
                {
                    inventorySystem.GetObject(stationReturnedObject, -1);
                }
                else if(stationReturnedObject == -1)
                {
                    //Esto es mas especifico para las estaciones que solo aceptan platos, aqui pues si ya dio su item, entonces ya nomas tomas el plato vacio
                    if (placeableObjectsData.GetObjectIDAt(gridPosition) >= 100 && placeableObjectsData.GetObjectIDAt(gridPosition) < 200) 
                    {
                        objectPlacer.GetGameObjectWithIndex(placeableObjectsData.GetRepresentationIndex(gridPosition)).GetComponent<PlateModelScript>().EmptyPlate();
                        inventorySystem.GetObject(placeableObjectsData.GetObjectIDAt(gridPosition), placeableObjectsData.GetRepresentationIndex(gridPosition));
                        objectPlacer.RemoveObjectAt(placeableObjectsData.GetRepresentationIndex(gridPosition));
                        placeableObjectsData.RemoveObjectAt(gridPosition);
                    }
                    //Aca despues podria ir uno que cheque que sea un ingrediente/especia, y si la estacion devuelve que simon o algo asi, etnonces que cambie ese ingrediente/especia a su version cortada
                }

            }
        }
    }

    //Esta funcion solia retornar bool, pero para reutilizar codigo, ahora retorna un int que significa:
    // 0 - No es valido (false)
    // 1 - Obtiene un objeto en su mano, y quita ese mismo objeto del mapa
    // 2 - Obtiene un objeto en su mano, y modifica el objeto del mapa
    private int CheckSelectionValidity(Vector3Int gridPosition)
    {
        int placeableObjectID = placeableObjectsData.GetObjectIDAt(gridPosition);
        int mapObjectID = mapObjectsData.GetObjectIDAt(gridPosition);

        if ((placeableObjectID >= 0 && placeableObjectID < 500) && mapObjectID == 0)
        {
            // Puede agarrar cualquier objeto que esté en una mesa basica
            return 1;
        }
        else if (mapObjectID >= 2) 
        {
            //Se vale si apunta a un dispensador o estacion (no piso, mesa vacia, ni bote de basura)
            return 2;
        }

        return 0;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int validity = CheckSelectionValidity(gridPosition);
        previewSystem.UpdateDefaultPreviewPosition(grid.CellToWorld(gridPosition), validity);
    }
}

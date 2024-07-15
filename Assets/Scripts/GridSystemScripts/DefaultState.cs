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

    public void OnAction1(Vector3Int gridPosition)
    {
        //Como ya quiero acabar, hare este if a lo "facil" asi que probablemente no sea muy optimo
        int placeableObjectID = placeableObjectsData.GetObjectIDAt(gridPosition);
        GameObject placeableObjectGameObject = placeableObjectsData.GetGameObjectAt(gridPosition);
        int mapObjectID = mapObjectsData.GetObjectIDAt(gridPosition);

        if (mapObjectID <= 0 || mapObjectID == 2)
            return;
        if(mapObjectID == 1 && placeableObjectID != -1)
        {
            //si es mesa basica con algo, agarra el algo y lo quita del diccionario
            placeableObjectsData.RemoveObjectAt(gridPosition);
            inventorySystem.GetObject(placeableObjectID, placeableObjectGameObject);
        }
        else if(mapObjectID == 3)
        {
            //dispensador, aun no esta
        }
        else if(mapObjectID >= 4)
        {
            //en el script del stoveState faltan cosas, como que pare el timer y eso, pero de mientras 
            BaseStationScript mapStationScript = mapObjectsData.GetGameObjectAt(gridPosition).GetComponent<BaseStationScript>();
            if (mapStationScript.GetContainedItemID() != -1)
            {
                int newID = mapStationScript.GetContainedItemID();
                GameObject newObject = mapStationScript.GetContainedItemGameObject();
                mapStationScript.EmptyStation();
                inventorySystem.GetObject(newID, newObject);
            }
        }
    }

    public void OnAction2(Vector3Int gridPosition)
    {

    }

    //Esta funcion solia retornar bool, pero para reutilizar codigo, ahora retorna un int que significa:
    // 0 - No es valido (false)
    // 1 - Obtiene un objeto en su mano, y quita ese mismo objeto del mapa
    // 2 - Obtiene un objeto en su mano, y modifica el objeto del mapa
    private int CheckSelectionValidity(Vector3Int gridPosition)
    {
        //esta funcion probablemente sea eliminada, ya que por ahora no tiene ningun proposito
        return 0;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int validity = CheckSelectionValidity(gridPosition);
        previewSystem.UpdateDefaultPreviewPosition(grid.CellToWorld(gridPosition), validity);
    }
}

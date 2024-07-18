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
        GameObject mapObjectGameObject = mapObjectsData.GetGameObjectAt(gridPosition);

        if (mapObjectID <= 0)
            return;

        BaseStationScript mapObjectStationScript = mapObjectGameObject.GetComponent<BaseStationScript>();
        int objectID = mapObjectStationScript.GetContainedItemID();
        GameObject objectGameObject = mapObjectStationScript.GetContainedItemGameObject();

        if(objectID != -1)
        {
            //no hace falta revisar si el gameObject == null, ya que el inventory system puede llamar al objectPlacer para crear un nuevo objeto
            inventorySystem.GetObject(objectID, objectGameObject);
        }
    }

    public void OnAction2(Vector3Int gridPosition)
    {
        Debug.Log(mapObjectsData.GetObjectIDAt(gridPosition));
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

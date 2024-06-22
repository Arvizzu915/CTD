using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private float yOffSet = 0f;
    int ID;
    int index;
    Grid grid;
    PreviewSystem previewSystem;
    InventorySystem inventorySystem;
    PlaceableObjectsDatabaseSO placeableObjectsDatabase;
    GridData placeableObjectsData;
    GridData mapObjectsData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, int index, Grid grid, PreviewSystem previewSystem, InventorySystem inventorySystem, PlaceableObjectsDatabaseSO placeableObjectsDatabase, GridData placeableObjectsData, GridData mapObjectsData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.index = index;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.inventorySystem = inventorySystem;
        this.placeableObjectsDatabase = placeableObjectsDatabase;
        this.placeableObjectsData = placeableObjectsData;
        this.mapObjectsData = mapObjectsData;
        this.objectPlacer = objectPlacer;

        //selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        selectedObjectIndex = ID;
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingDefaultPreview(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size);
            previewSystem.StartShowingObjectPreview(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingDefaultPreview();
        previewSystem.StopShowingObjectPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        int placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == 0)
            return;
        if(placementValidity == 1)
        {
            objectPlacer.RemoveObjectAt(placeableObjectsData.GetRepresentationIndex(gridPosition));
            placeableObjectsData.RemoveObjectAt(gridPosition);
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdateDefaultPreviewPosition(cellPosition, 0);
            previewSystem.UpdateObjectPreviewPosition(cellPosition, 0, mapObjectsData.GetRepresentationIndex(gridPosition));
        }
        else if(placementValidity == 2)
        {
            inventorySystem.RemoveObject();
            objectPlacer.PermaRemoveObjectAt(index);
            //en teoria no hace falta eliminar del grid data, porque ya se elimina en todo remove object
            //placeableObjectsData.RemoveObjectAt(gridPosition);
        }
        else if(placementValidity == 3)
        {
            inventorySystem.RemoveObject();
            if(mapObjectsData.GetObjectIDAt(gridPosition) == 0)
            {
                yOffSet = 1f;
            }
            else if(mapObjectsData.GetObjectIDAt(gridPosition) == -1)
            {
                yOffSet = 0f;
            }
            int newIndex = objectPlacer.PlaceObject(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), yOffSet, index);
            placeableObjectsData.AddObjectAt(gridPosition, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].ID, newIndex);
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdateDefaultPreviewPosition(cellPosition, 0);
            previewSystem.UpdateObjectPreviewPosition(cellPosition, 0, mapObjectsData.GetRepresentationIndex(gridPosition));
        }
        else if(placementValidity == 4)
        {
            inventorySystem.RemoveObject();
            objectPlacer.PermaRemoveObjectAt(placeableObjectsData.GetRepresentationIndex(gridPosition));
            placeableObjectsData.RemoveObjectAt(gridPosition);
            if (mapObjectsData.GetObjectIDAt(gridPosition) == 0)
            {
                yOffSet = 1f;
            }
            else if (mapObjectsData.GetObjectIDAt(gridPosition) == -1)
            {
                yOffSet = 0f;
            }
            int newIndex = objectPlacer.PlaceObject(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), yOffSet, index);
            placeableObjectsData.AddObjectAt(gridPosition, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size, placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].ID, newIndex);
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdateDefaultPreviewPosition(cellPosition, 0);
            previewSystem.UpdateObjectPreviewPosition(cellPosition, 0, mapObjectsData.GetRepresentationIndex(gridPosition));
        }
        else if(placementValidity == 5)
        {
            //lo hare luego tengo sueño xd
        }
    }

    //Esta funcion solia retornar bool, pero para reutilizar codigo, ahora retorna un int que significa:
    // 0 - No es valido (false)
    // 1 - Conserva objeto en mano, y quita objeto en mapa
    // 2 - Pierde objeto en mano, y no afecta objeto en mapa
    // 3 - Pierde objeto en mano, y pone objeto en mapa
    // 4 - Pierde objeto en mani y sustituye objeto en mapa (quita el que está y pone otro)
    // 5 - Pierde objeto en mano, y modifica objeto en mapa
    private int CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //No queria hacer esto de puro if else, pero la verdad despues de 4 dias no se me ocurre otra opcion
        int selectedObjectID = placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].ID;
        int placeableObjectID = placeableObjectsData.GetObjectIDAt(gridPosition);
        int mapObjectID = mapObjectsData.GetObjectIDAt(gridPosition);

        //A menos que sea la pala, puede tirarse a la basura
        if (selectedObjectID != 0 && mapObjectID == 1)
            return 2;
        if (selectedObjectID == 0)
        {
            //Si es una pala, entonces puede ponerse en cualquier torre siempre y cuando no esté en una mesa basica, o en una mesa basica, siempre y cuando este vacia
            if (placeableObjectID >= 400 && mapObjectID != 0)
            {
                return 1;
            }
            else if (placeableObjectID == -1 && mapObjectID == 0)
            {
                return 3;
            }
        }
        else if (selectedObjectID >= 100 && selectedObjectID < 400)
        {
            //Si es recipiente, ingrediente, o especia, y esta en una mesa basica
            if(mapObjectID == 0)
            {
                //Si esta vacio el lugar, adelante
                if (placeableObjectID == -1)
                {
                    return 3;
                }
                //si es un recipiente puede ponerse en un ingrediente o especia
                else if ((selectedObjectID >= 100 && selectedObjectID < 200) && (placeableObjectID >= 200 && placeableObjectID < 400))
                {
                    return 4;
                }
                //si es un ingrediente o especia puede ponerse en un recipiente
                else if ((selectedObjectID >= 200 && selectedObjectID < 400) && (placeableObjectID >= 100 && placeableObjectID < 200))
                {
                    return 5;
                }
            }
            else if(mapObjectID >= 3)
            {
                //si lo pone en una estación que no sea piso, ni mesa basica, ni dispensador, ni basura
                return 5;
            }
        }
        else if (selectedObjectID >= 400)
        {
            //Si es una torreta, solo puede ponerse sobre la misma torreta, o en el piso o una mesa basica pero que esten vacios (no ocupados por otro objeto)
            if (placeableObjectID == selectedObjectID)
            {
                //aca podria agregarse que no este en una mesa, pero por ahora tiene permitido mejorar torretas aunque esten inactivas en la mesa
                return 5;
            }
            else if (placeableObjectID == -1 && (mapObjectID == -1 || mapObjectID == 0))
            {
                return 3;
            }
        }

        return 0;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        //Debug.Log(placementValidity);
        previewSystem.UpdateDefaultPreviewPosition(grid.CellToWorld(gridPosition), placementValidity);
        previewSystem.UpdateObjectPreviewPosition(grid.CellToWorld(gridPosition), placementValidity, mapObjectsData.GetObjectIDAt(gridPosition));
    }
}

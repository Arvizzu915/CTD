using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    //private int selectedObjectIndex = -1;
    //private float yOffSet = 0f;
    int selectedID;
    GameObject selectedGameObject;
    Grid grid;
    PreviewSystem previewSystem;
    InventorySystem inventorySystem;
    GridData towersObjectsData;
    GridData mapObjectsData;
    ObjectPlacer objectPlacer;

    public PlacementState(int ID, GameObject gameObject, Grid grid, PreviewSystem previewSystem, InventorySystem inventorySystem, GridData towersObjectsData, GridData mapObjectsData, ObjectPlacer objectPlacer)
    {
        selectedID = ID;
        selectedGameObject = gameObject;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.inventorySystem = inventorySystem;
        this.towersObjectsData = towersObjectsData;
        this.mapObjectsData = mapObjectsData;
        this.objectPlacer = objectPlacer;

        //Aca queremos que solo muestre preview si tiene un plato con una torre dentro
        //selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        //selectedObjectIndex = selectedID;
        //if (selectedObjectIndex > -1)
        //{
        //    previewSystem.StartShowingDefaultPreview(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Size);
        //    if(index == -1)
        //    {
        //        previewSystem.StartShowingObjectPreview(placeableObjectsDatabase.objectsPlacementData[selectedObjectIndex].Prefab);
        //    }
        //    else if(objectPlacer.GetGameObjectWithIndex(index) != null)
        //    {
                
        //    }
        //    else
        //    {
        //        Debug.Log("de donde sacaste ese objeto?");
        //    }
        //}
        //else
        //{
        //    throw new System.Exception($"No object with ID {iD}");
        //}
    }

    public void EndState()
    {
        previewSystem.StopShowingDefaultPreview();
        previewSystem.StopShowingObjectPreview();
    }

    private void RefreshObjectPreview(PreviewSystem previewSystem, GameObject newPrefab, Vector3Int gridPosition, int placementValidity)
    {
        previewSystem.StopShowingObjectPreview();
        previewSystem.StartShowingObjectPreview(newPrefab);
        previewSystem.UpdateObjectPreviewPosition(grid.CellToWorld(gridPosition), placementValidity, mapObjectsData.GetObjectIDAt(gridPosition));
    }

    public void OnAction1(Vector3Int gridPosition)
    {
        //Como ya quiero acabar, hare este if a lo "facil" asi que probablemente no sea muy optimo
        int mapObjectID = mapObjectsData.GetObjectIDAt(gridPosition);
        GameObject mapObjectGameObject = mapObjectsData.GetGameObjectAt(gridPosition);

        // Si no hay nada en el mapa o no es interactuable, pues nada xd (aunque aqui quiza podria venir despues lo de que se vaya al mas cercano)
        if (mapObjectID == -1)
            return;
        if (mapObjectID == 0)
        {
            //Aca solo para torre
            if(selectedID >= 100 && selectedID < 105)
            {
                //solo si tiene un plato principal (los unicos que pueden tener torres)
                BaseContainerScript selectedContainerScript = selectedGameObject.GetComponent<BaseContainerScript>();
                int selectedTowerID = selectedContainerScript.GetContainedItemID();
                if(selectedTowerID >= 400 && selectedTowerID < 500)
                {
                    //solo si ese plato tiene dentro una torre
                    int towerObjectID = towersObjectsData.GetObjectIDAt(gridPosition);
                    GameObject towerObjectGameObject = towersObjectsData.GetGameObjectAt(gridPosition);
                    if (towerObjectID == -1)
                    {
                        //si no hay ninguna torre ya en ese lugar, entonces pone la que tiene
                        selectedContainerScript.EmptyContainer(true);
                        GameObject newTower = objectPlacer.CreateNewObject(selectedTowerID);
                        Vector3 adjustedPosition = grid.CellToWorld(gridPosition);
                        adjustedPosition = new Vector3(adjustedPosition.x + 0.5f, adjustedPosition.y, adjustedPosition.z + 0.5f);
                        newTower.transform.position = adjustedPosition;
                        towersObjectsData.AddObjectAt(gridPosition, selectedTowerID, newTower);
                    }
                    else if(towerObjectID == selectedTowerID)
                    {
                        //si ya hay una, y es la misma a la que tenemos, entonces ve si la puede mejorar
                        if (towerObjectGameObject.GetComponent<BaseTurretScript>().CanUpgradeTurret())
                        {
                            //si pudo mejorarla, entonces vacia nuestro plato con la torre
                            selectedContainerScript.EmptyContainer(true);
                        }
                    }
                }
            }
            else if(selectedID == 0)
            {
                //si tiene una pala, ve si hay una torre para borrar
                int towerObjectID = towersObjectsData.GetObjectIDAt(gridPosition);
                GameObject towerObjectGameObject = towersObjectsData.GetGameObjectAt(gridPosition);
                if(towerObjectID >= 400 && towerObjectID < 500)
                {
                    //si hay una torre, la borra y la quita del diccionario
                    objectPlacer.DeleteObject(towerObjectGameObject);
                    towersObjectsData.RemoveObjectAt(gridPosition);
                }
            }
        }
        else
        {
            BaseStationScript mapObjectStationScript = mapObjectGameObject.GetComponent<BaseStationScript>();
            //Primero vemos si el objeto se puede meter en la estacion
            switch (mapObjectStationScript.CanEnterStation(selectedID, selectedGameObject))
            {
                case 0:
                    //Significa que no pudo entrar
                    if (selectedID >= 100 && selectedID < 200)
                    {
                        //Solo si es contenedor, ve si lo que esta en la estacion podria entrar
                        int toEnterID = mapObjectStationScript.GetContainedItemID();
                        GameObject toEnterGameObject = mapObjectStationScript.GetContainedItemGameObject();

                        switch (selectedGameObject.GetComponent<BaseContainerScript>().CanEnterContainer(toEnterID, toEnterGameObject))
                        {
                            case 0:
                                //pos nada
                                break;
                            case 1:
                                mapObjectStationScript.EmptyStation();
                                break;
                            case 2:
                                //Nada, el propio script ya vacio al otro contenedor
                                break;
                        }
                    }
                    break;
                case 1:
                    //significa que si pudo entrar
                    inventorySystem.RemoveObject();
                    break;
                case 2:
                    //significa que su contenido si pudo entrar
                    break;
            }
        }
    }

    public void OnAction2(Vector3Int gridPosition)
    {
        //Por ahora, si tiene un objeto, su accion secundaria no hace nada, pero pues aqui esta el hueco disponible por si se quiere hacer que lanze el objeto, o haga algo con el, como rotar o algo asi
    }

    //Esta funcion solia retornar bool, pero para reutilizar codigo, ahora retorna un int que significa:
    // 0 - No es valido (false)
    // 1 - Conserva objeto en mano, y quita objeto en mapa
    // 2 - Pierde objeto en mano, y no afecta objeto en mapa
    // 3 - Mueve objeto de mano a mapa

    // 4 - Pierde objeto en mano y sustituye objeto en mapa (quita el que está y pone otro)
    // 5 - Pierde objeto en mano, y modifica objeto en mapa
    private int CheckPlacementValidity(Vector3Int gridPosition)
    {
        //aca esto sera cambiado para que solo sirva para el preview de las torres
        return 0;
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        int placementValidity = CheckPlacementValidity(gridPosition);
        //Debug.Log(placementValidity);
        previewSystem.UpdateDefaultPreviewPosition(grid.CellToWorld(gridPosition), placementValidity);
        previewSystem.UpdateObjectPreviewPosition(grid.CellToWorld(gridPosition), placementValidity, mapObjectsData.GetObjectIDAt(gridPosition));
    }
}

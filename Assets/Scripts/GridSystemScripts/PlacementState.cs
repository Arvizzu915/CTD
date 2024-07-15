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
    PlaceableObjectsDatabaseSO placeableObjectsDatabase;
    GridData placeableObjectsData;
    GridData mapObjectsData;
    ObjectPlacer objectPlacer;

    public PlacementState(int ID, GameObject gameObject, Grid grid, PreviewSystem previewSystem, InventorySystem inventorySystem, PlaceableObjectsDatabaseSO placeableObjectsDatabase, GridData placeableObjectsData, GridData mapObjectsData, ObjectPlacer objectPlacer)
    {
        selectedID = ID;
        selectedGameObject = gameObject;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.inventorySystem = inventorySystem;
        this.placeableObjectsDatabase = placeableObjectsDatabase;
        this.placeableObjectsData = placeableObjectsData;
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
        int placeableObjectID = placeableObjectsData.GetObjectIDAt(gridPosition);
        GameObject placeableObjectGameObject = placeableObjectsData.GetGameObjectAt(gridPosition);
        int mapObjectID = mapObjectsData.GetObjectIDAt(gridPosition);
        //aca quiero que defina el cellposiion, por ahora se usaria 3 veces

        // Si no hay nada en el mapa o no es interactuable, pues nada xd (aunque aqui quiza podria venir despues lo de que se vaya al mas cercano)
        if (mapObjectID == -1)
            return;
        //switch para objetos de mapa especiales
        switch (mapObjectID)
        {
            case 0:
                //piso solo para torreta
                if(selectedID == 100 || selectedID == 101)
                {
                    //Si es un plato o vaso (ahorita ignora el vaso)
                    int containedItemID = selectedGameObject.GetComponent<BasePlateState>().GetContainedItemID();
                    if (containedItemID >= 400)
                    {
                        //si el plato tiene torreta
                        if(placeableObjectID == containedItemID)
                        {
                            //Ve si puede mejorar la torre
                            if (placeableObjectGameObject.GetComponent<BaseTurretScript>().CanUpgradeTurret() == true)
                            {
                                //ya mejoro la torre, entonces solo vacia el plato
                                selectedGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                            }
                        }
                        else
                        {
                            //Coloca la torreta en piso
                            Vector3 cellPosition = grid.CellToWorld(gridPosition);
                            GameObject newTower = objectPlacer.CreateNewObject(containedItemID);
                            objectPlacer.MoveObject(newTower, cellPosition);
                            selectedGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                            placeableObjectsData.AddObjectAt(gridPosition, containedItemID, newTower);
                        }
                    }
                }
                Debug.Log("tilin");
                return;
            case 1:
                //mesa basica
                if(placeableObjectID == -1)
                {
                    //si esta vacia la mesa, simplemente coloca el objeto arriba de la mesa (por ahora usa un offset de 1, pero aqui le podemos cambiar)
                    Vector3 cellPosition = grid.CellToWorld(gridPosition);
                    objectPlacer.MoveObject(selectedGameObject, new Vector3(cellPosition.x, cellPosition.y + 1f, cellPosition.z));
                    placeableObjectsData.AddObjectAt(gridPosition, selectedID, selectedGameObject);
                    inventorySystem.RemoveObject();
                }
                else
                {
                    //Si tiene algo la mesa, entonces
                    if (selectedID == 100 || selectedID == 101)
                    {
                        //Si es un plato o vaso (ahorita ignora el vaso)
                        int containedItemID = selectedGameObject.GetComponent<BasePlateState>().GetContainedItemID();
                        GameObject containedItemGameObject = selectedGameObject.GetComponent<BasePlateState>().GetContainedItemGameObject();
                        int itemToEnterID;
                        GameObject itemToEnterGameObject;

                        //ve cada posible interaccion del plato con:
                        if (placeableObjectID == 100)
                        {
                            //hay un plato colocado
                            itemToEnterID = placeableObjectGameObject.GetComponent<BasePlateState>().GetContainedItemID();
                            itemToEnterGameObject = placeableObjectGameObject.GetComponent<BasePlateState>().GetContainedItemGameObject();
                            if (selectedGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el contenido del plato colocado pudo entrar al plato nuestro, entonces vacia el plato colocado
                                placeableObjectGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                            }
                            else
                            {
                                //si el contenido del plato colocado no pudo entrar al plato nuestro, entonces ve si el contenido del plato nuestro puede entrar al plato colocado
                                if (placeableObjectGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(containedItemID, containedItemGameObject))
                                {
                                    //si el contenido del plato nuestro pudo entrar al plato colocado, entonces vacia el plato nuestro
                                    selectedGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                                }
                            }
                        }
                        else if (placeableObjectID >= 105 && placeableObjectID < 200)
                        {
                            //hay un recipiente (que no es plato) colocado
                            itemToEnterID = placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemID();
                            itemToEnterGameObject = placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemGameObject();
                            if (selectedGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el contenido del recipiente colocado pudo entrar al plato nuestro, entonces vacia el recipiente colocado
                                placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().EmptyContainer();
                            }
                            else
                            {
                                //si el contenido del recipiente colocado no pudo entrar al plato nuestro, entonces ve si el contenido del plato nuestro puede entrar al recipiente colocado
                                if (placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(containedItemID, containedItemGameObject))
                                {
                                    //si el contenido del plato nuestro pudo entrar al recipiente colocado, entonces vacia el plato nuestro
                                    selectedGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                                }
                            }
                        }
                        else if (placeableObjectID >= 200 && placeableObjectID < 300)
                        {
                            //hay un ingrediente colocado
                            itemToEnterID = placeableObjectID;
                            itemToEnterGameObject = placeableObjectGameObject;
                            if (selectedGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el ingrediente colocado pudo entrar al plato nuestro, quita al ingrediente del diccionario grid (no hace falta moverlo ya que el plato lo hace)
                                placeableObjectsData.RemoveObjectAt(gridPosition);
                            }
                            else
                            {
                                //si el ingrediente colocado no pudo entrar al plato nuestro, entonces no hace falta ver si el contenido del plato nuestro puede afectar al ingrediente, porque es pro
                            }
                        }

                    }
                    else if (selectedID >= 105 && selectedID < 200)
                    {
                        //Esto es casi lo mismo que el de arriba solo que cambiando todo BasePlateState a IndividualState3ChangerContainerState
                        //Si es un recipiente (que no es plato pricipal)
                        int containedItemID = selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemID();
                        GameObject containedItemGameObject = selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemGameObject();
                        int itemToEnterID;
                        GameObject itemToEnterGameObject;

                        //ve cada posible interaccion del recipiente con:
                        if (placeableObjectID == 100)
                        {
                            //hay un plato colocado
                            itemToEnterID = placeableObjectGameObject.GetComponent<BasePlateState>().GetContainedItemID();
                            itemToEnterGameObject = placeableObjectGameObject.GetComponent<BasePlateState>().GetContainedItemGameObject();
                            if (selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el contenido del plato colocado pudo entrar al recipiente nuestro, entonces vacia el plato colocado
                                placeableObjectGameObject.GetComponent<BasePlateState>().EmptyContainer(false);
                            }
                            else
                            {
                                //si el contenido del plato colocado no pudo entrar al recipiente nuestro, entonces ve si el contenido del recipiente nuestro puede entrar al plato colocado
                                if (placeableObjectGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(containedItemID, containedItemGameObject))
                                {
                                    //si el contenido del recipiente nuestro pudo entrar al plato colocado, entonces vacia el recipiente nuestro
                                    selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().EmptyContainer();
                                }
                            }
                        }
                        else if (placeableObjectID >= 105 && placeableObjectID < 200)
                        {
                            //hay un recipiente (que no es plato) colocado
                            itemToEnterID = placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemID();
                            itemToEnterGameObject = placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemGameObject();
                            if (selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el contenido del recipiente colocado pudo entrar al recipiente nuestro, entonces vacia el recipiente colocado
                                placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().EmptyContainer();
                            }
                            else
                            {
                                //si el contenido del recipiente colocado no pudo entrar al recipiente nuestro, entonces ve si el contenido del recipiente nuestro puede entrar al recipiente colocado
                                if (placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(containedItemID, containedItemGameObject))
                                {
                                    //si el contenido del recipiente nuestro pudo entrar al recipiente colocado, entonces vacia el recipiente nuestro
                                    selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().EmptyContainer();
                                }
                            }
                        }
                        else if (placeableObjectID >= 200 && placeableObjectID < 300)
                        {
                            //hay un ingrediente colocado
                            itemToEnterID = placeableObjectID;
                            itemToEnterGameObject = placeableObjectGameObject;
                            if (selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(itemToEnterID, itemToEnterGameObject))
                            {
                                //si el ingrediente colocado pudo entrar al recipiente nuestro, quita al ingrediente del diccionario grid (no hace falta moverlo ya que el recipiente lo hace)
                                placeableObjectsData.RemoveObjectAt(gridPosition);
                            }
                            else
                            {
                                //si el ingrediente colocado no pudo entrar al recipiente nuestro, entonces no hace falta ver si el contenido del recipiente nuestro puede afectar al ingrediente, porque es noob
                                //mas que nada es porque estos recipientes (hasta donde yo se) no pueden tener nada que afecte a un ingrediente directamente (como el pan cortado)
                            }
                        }
                    }
                    else if (selectedID >= 200 && selectedID < 300)
                    {
                        //Si es un ingrediente
                        //ve cada posible interaccion del ingrediente con:
                        if (placeableObjectID == 100)
                        {
                            //hay un plato colocado
                            if (placeableObjectGameObject.GetComponent<BasePlateState>().CanPlaceObjectInContainer(selectedID, selectedGameObject))
                            {
                                //si el ingrediente pudo entrar al plato colocado, entonces quita el ingrediente del inventario
                                inventorySystem.RemoveObject();
                            }
                        }
                        else if (placeableObjectID >= 105 && placeableObjectID < 200)
                        {
                            //hay un recipiente (que no es plato) colocado
                            if (placeableObjectGameObject.GetComponent<IndividualState3ChangerContainerState>().CanPlaceObjectInContainer(selectedID, selectedGameObject))
                            {
                                //si el ingrediente pudo entrar al recipiente colocado, entonces quita el ingrediente del inventario
                                inventorySystem.RemoveObject();
                            }
                        }
                        else if (placeableObjectID >= 200 && placeableObjectID < 300)
                        {
                            //aca en teoria estaria todo para poder empanizar cosas fuera del plato
                            int placeableObjecChangeState2ID = placeableObjectGameObject.GetComponent<BaseIngredientScript>().ShowChangeState2ID();
                            int selectedObjecChangeState2ID = selectedGameObject.GetComponent<BaseIngredientScript>().ShowState2();

                            //Si tienen mismo id de cambio de estado, entonces dale, sino pal lobby
                            if (selectedObjecChangeState2ID == placeableObjecChangeState2ID && selectedObjecChangeState2ID != 0)
                            {
                                //Aca checa cual es el que es el changer (como el pan) para ver a cual se le cambia el estado, si ninguno es el changer, entonces fuga
                                if (selectedGameObject.GetComponent<BaseIngredientScript>().ShowIfIsState2Changer())
                                {
                                    placeableObjectGameObject.GetComponent<BaseIngredientScript>().ChangeState2(selectedObjecChangeState2ID, true);
                                    inventorySystem.RemoveObject();
                                }
                                else if (placeableObjectGameObject.GetComponent<BaseIngredientScript>().ShowIfIsState2Changer())
                                {
                                    //primero le aplica el nuevo estado al ingrediente nuestro
                                    selectedGameObject.GetComponent<BaseIngredientScript>().ChangeState2(selectedObjecChangeState2ID, true);
                                    //luego quita el ingrediente que esta colocado
                                    placeableObjectsData.RemoveObjectAt(gridPosition);
                                    objectPlacer.DeleteObject(placeableObjectGameObject);
                                    //y al final coloca nuestro ingrediente
                                    Vector3 cellPosition = grid.CellToWorld(gridPosition);
                                    objectPlacer.MoveObject(selectedGameObject, new Vector3(cellPosition.x, cellPosition.y + 1f, cellPosition.z));
                                    placeableObjectsData.AddObjectAt(gridPosition, selectedID, selectedGameObject);
                                    inventorySystem.RemoveObject();
                                }
                            }
                        }
                    }
                    //aca quiza podria entrrar un if extra para las especias si es necesario
                }
                Debug.Log("tilin");
                return;
            case 2:
                //basura
                if (selectedID >= 100 && selectedID < 400)
                {
                    //si es tirable
                    if (selectedID == 100 || selectedID == 101)
                    {
                        //si es plato principal (estoy ignorando al vaso otra vez xd), vacia permanentemente al plato
                        selectedGameObject.GetComponent<BasePlateState>().EmptyContainer(true);
                    }
                    else if(selectedID >= 105 && selectedID < 200)
                    {
                        //si es un recipiente, elimina el objeto que tiene dentro y luego lo vacia
                        objectPlacer.DeleteObject(selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().GetContainedItemGameObject());
                        selectedGameObject.GetComponent<IndividualState3ChangerContainerState>().EmptyContainer();
                    }
                    else
                    {
                        //si es otra cosa (ingrediente o especia) borra el objeto
                        objectPlacer.DeleteObject(selectedGameObject);
                        inventorySystem.RemoveObject();
                    }
                }
                Debug.Log("tilin");
                return;
            case 3:
                //dispensador (aun no esta listo)
                //if (selectedID == 100 || selectedID == 101)
                //{
                //    //si es plato principal (estoy ignorando al vaso otra vez xd), intenta poner el nuevo objeto dentro del plato
                //}
                //else if (selectedID >= 105 && selectedID < 200)
                //{
                //    //si es un recipiente, intenta poner el nuevo ingrediente dentro del recipiente
                //}
                Debug.Log("tilin");
                return;
        }
        Debug.Log("Si antes de esto se imprimió un tilin, algo salio mal");
        //el resto de estaciones
        if(mapObjectID >= 4)
        {
            if(mapObjectsData.GetGameObjectAt(gridPosition).GetComponent<BaseStationScript>().OnAccessWithID(selectedID, selectedGameObject))
            {
                //si pudo meter el objeto en mano a la estacion, entonces fuga
                inventorySystem.RemoveObject();
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

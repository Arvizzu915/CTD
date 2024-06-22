using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerActions playerActions;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private PlaceableObjectsDatabaseSO placeableObjectsDatabase;

    public GridData placeableObjectsData, mapObjectsData;

    [SerializeField]
    private PreviewSystem previewSystem;
    [SerializeField]
    private InventorySystem inventorySystem;
    [SerializeField]
    private MapSystem mapSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopSystem();
        placeableObjectsData = new();
        mapObjectsData = new();
        mapSystem.SetGridData(placeableObjectsData, mapObjectsData);
    }

    public void StartDefault()
    {
        StopSystem();
        buildingState = new DefaultState(grid, previewSystem, inventorySystem, placeableObjectsData, mapObjectsData, objectPlacer);
        playerActions.OnPressed += SystemAction;
    }

    public void StartPlacement(int ID, int index)
    {
        StopSystem();
        buildingState = new PlacementState(ID, index, grid, previewSystem, inventorySystem, placeableObjectsDatabase, placeableObjectsData, mapObjectsData, objectPlacer);
        playerActions.OnPressed += SystemAction;
        //aca podria entrar un evento de OnRotated o algo asi, para rotar la torre quiza eso tilin
    }

    private void SystemAction()
    {
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);
        buildingState.OnAction(gridPosition);
    }

    private void StopSystem()
    {
        if (buildingState == null)
            return;
        buildingState.EndState();
        playerActions.OnPressed -= SystemAction;
        playerActions.OnExit -= StopSystem;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);

        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}

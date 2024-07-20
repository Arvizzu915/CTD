using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerActions playerActions;
    [SerializeField]
    private Grid grid;

    public GridData towersObjectsData, mapObjectsData;

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
        towersObjectsData = new();
        mapObjectsData = new();
        mapSystem.SetGridData(towersObjectsData, mapObjectsData);
    }

    public void StartDefault()
    {
        StopSystem();
        buildingState = new DefaultState(grid, previewSystem, inventorySystem, towersObjectsData, mapObjectsData, objectPlacer);
        playerActions.OnPressed1 += SystemAction1;
        playerActions.OnPressed2 += SystemAction2;
    }

    public void StartPlacement(int ID, GameObject gameObject)
    {
        StopSystem();
        buildingState = new PlacementState(ID, gameObject, grid, previewSystem, inventorySystem, towersObjectsData, mapObjectsData, objectPlacer);
        playerActions.OnPressed1 += SystemAction1;
        playerActions.OnPressed2 += SystemAction2;
    }

    private void SystemAction1()
    {
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);
        buildingState.OnAction1(gridPosition);
    }

    private void SystemAction2()
    {
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);
        buildingState.OnAction2(gridPosition);
    }

    private void StopSystem()
    {
        if (buildingState == null)
            return;
        buildingState.EndState();
        playerActions.OnPressed1 -= SystemAction1;
        playerActions.OnPressed2 -= SystemAction2;
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

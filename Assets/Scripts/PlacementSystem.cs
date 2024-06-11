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
    private TowersDatabaseSO towersDatabase;

    private GridData towersData;

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        towersData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        buildingState = new PlacementState(ID, grid, previewSystem, towersDatabase, towersData, objectPlacer);
        playerActions.OnPressed += PlaceStructure;
        playerActions.OnExit += StopPlacement;
        //aca podria entrar un evento de OnRotated o algo asi, para rotar la torre quiza eso tilin
    }

    public void StartRemoving()
    {
        StopPlacement();
        buildingState = new RemovingState(grid, previewSystem, towersData, objectPlacer);
        playerActions.OnPressed += PlaceStructure;
        playerActions.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);
        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    //como por ahora solo esta el gridData de las torres, aca solo igualamos
    //    GridData selectedData = towersData;

    //    return selectedData.CanPlaceObjectAt(gridPosition, towersDatabase.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        buildingState.EndState();
        playerActions.OnPressed -= PlaceStructure;
        playerActions.OnExit -= StopPlacement;
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

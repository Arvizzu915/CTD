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
    private int selectedObjectIndex = -1;

    private GridData towersData;

    private List<GameObject> placedGameObjects = new();

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        towersData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = towersDatabase.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        previewSystem.StartShowingPlacementPreview(towersDatabase.objectsData[selectedObjectIndex].Prefab, towersDatabase.objectsData[selectedObjectIndex].Size);
        playerActions.OnPressed += PlaceStructure;
        //playerActions.OnExit += StopPlacement; esto lo movi a PlaceStructure para que solo pueda salir del "modo placement" despues de colocar un objeto
        //aca podria entrar un evento de OnRotated o algo asi, para rotar la torre quiza eso tilin
    }

    private void PlaceStructure()
    {
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        GameObject newOject = Instantiate(towersDatabase.objectsData[selectedObjectIndex].Prefab);
        newOject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newOject);

        GridData selectedData = towersData;
        selectedData.AddObjectAt(gridPosition, towersDatabase.objectsData[selectedObjectIndex].Size, towersDatabase.objectsData[selectedObjectIndex].ID, placedGameObjects.Count - 1);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);

        //Esto lo agrege aqui para que al final de poner la torre, ahora si pueda salirse del modo placement (esto lo hace automaticamente despues de soltar el espacio en el playerActions script)
        playerActions.OnExit += StopPlacement;
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //como por ahora solo esta el gridData de las torres, aca solo igualamos
        GridData selectedData = towersData;

        return selectedData.CanPlaceObjectAt(gridPosition, towersDatabase.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        previewSystem.StopShowingPlacementPreview();
        playerActions.OnPressed -= PlaceStructure;
        playerActions.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        Vector3 playerPointerPosition = playerActions.GetPointingPosition();
        Vector3Int gridPosition = grid.WorldToCell(playerPointerPosition);

        if(lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }
    }
}

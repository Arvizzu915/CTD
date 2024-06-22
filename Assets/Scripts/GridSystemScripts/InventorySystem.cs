using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private int selectedObjectIndex = -1;
    private int specificObjectIndex;

    [SerializeField]
    PlaceableObjectsDatabaseSO placeableObjectsDatabase;

    [SerializeField]
    PlacementSystem placementSystem;

    private void Start()
    {
        RemoveObject();
    }

    public void GetObject(int ID, int index)
    {
        if (selectedObjectIndex > -1)
            return;
        selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        specificObjectIndex = index;
        if (selectedObjectIndex > -1)
        {
            placementSystem.StartPlacement(selectedObjectIndex, specificObjectIndex);
        }
        else
        {
            throw new System.Exception($"No object with ID {ID}");
        }
    }

    public void RemoveObject()
    {
        selectedObjectIndex = -1;
        specificObjectIndex = -1;
        placementSystem.StartDefault();
    }
}

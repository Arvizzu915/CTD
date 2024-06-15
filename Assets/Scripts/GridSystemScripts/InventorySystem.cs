using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private int selectedObjectIndex = -1;

    [SerializeField]
    PlaceableObjectsDatabaseSO placeableObjectsDatabase;

    public void GetObject(int ID)
    {
        if (selectedObjectIndex > -1)
            return;
        selectedObjectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            //previewSystem.StartShowingPlacementPreview(towersDatabase.objectsPlacementData[selectedObjectIndex].Prefab, towersDatabase.objectsPlacementData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {ID}");
        }
    }

    public void RemoveObject()
    {
        selectedObjectIndex = -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private int selectedObjectID = -1;
    private GameObject selectedObjectGameObject = null;

    [SerializeField]
    PlacementSystem placementSystem;
    [SerializeField]
    ObjectPlacer objectPlacer;
    [SerializeField]
    PlayerActions playerActions;

    private void Start()
    {
        RemoveObject();
    }

    public bool GetObject(int ID, GameObject gameObject)
    {
        //Si ya tiene algo en la mano, o le llega un ID no aceptado, da false
        if (selectedObjectID > -1 || ID <= -1 || ID > 400)
            return false;
        //Iguala y coloca el objeto en mano
        selectedObjectID = ID;
        //Si el gameObject es null, significa que es un nuevo objeto, por lo que manda a crearlo para luego colocarlo
        if(gameObject == null)
        {
            selectedObjectGameObject = objectPlacer.CreateNewObject(ID);
        }
        else
        {
            selectedObjectGameObject = gameObject;
        }
        objectPlacer.MoveObject(selectedObjectGameObject, playerActions.GetGrabHitboxPosition());
        placementSystem.StartPlacement(selectedObjectID, selectedObjectGameObject);
        return true;
    }

    public void RemoveObject()
    {
        selectedObjectID = -1;
        selectedObjectGameObject = null;
        placementSystem.StartDefault();
    }
}

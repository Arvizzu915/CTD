using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStationScript : MonoBehaviour
{
    [SerializeField]
    public int stationID = 501;//por ahora la hize publica, pero podria ser privada y solo tener una funcion extra que devuelva su valor

    [SerializeField]
    private int[] acceptedIDs;

    //Aca la verdad no se si esto sea la mejor opcion, pero fue lo unico que se me ocurrio para poder pasarle al dispenserState un ID del objeto que dispensa, lo malo es que ningun otro state va a usar esta variable
    [SerializeField]
    private int itemID;

    IStationState stationState;

    void Start()
    {
        switch (stationID)
        {
            case 501:
                stationState = new BasicTableState(this.transform);
                break;
            case 502:
                stationState = new TrashcanState();
                break;
            case 503:
                stationState = new DispenserState(itemID);
                break;
            case 504:
                stationState = new StoveState(this.transform, acceptedIDs);
                break;
            case 505:
                stationState = new CuttingTableState(stationID, this.transform);
                break;
            case 506:
                stationState = new DeepFryerState(this.transform, acceptedIDs);
                break;
        }
    }

    public int GetContainedItemID()
    {
        return stationState.GetContainedItemID();
    }

    public GameObject GetContainedItemGameObject()
    {
        return stationState.GetContainedItemGameObject();
    }

    public void EmptyStation()
    {
        stationState.EmptyStation();
    }

    public int CanEnterStation(int ID, GameObject gameObject)
    {
        return stationState.CanEnterStation(ID, gameObject);
    }

    public void OnAccess2()
    {
        stationState.OnAccess2();
    }

    void Update()
    {
        stationState.UpdateState();
    }
}

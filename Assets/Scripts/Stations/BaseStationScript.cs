using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStationScript : MonoBehaviour
{
    [SerializeField]
    private int stationID = 501;

    [SerializeField]
    private int[] acceptedIDs;

    //Aca la verdad no se si esto sea la mejor opcion, pero fue lo unico que se me ocurrio para poder pasarle al dispenserState un ID del objeto que dispensa, lo malo es que ningun otro state va a usar esta variable
    [SerializeField]
    public int itemID; //la hize public de mientras nomas en lo que esta el mapsystem definitivo (ahorita necesita esto)

    IStationState stationState;

    void Start()
    {
        switch (stationID)
        {
            case 501:
                stationState = new BasicTableState(this.transform.position);
                break;
            case 502:
                stationState = new TrashcanState();
                break;
            case 503:
                stationState = new DispenserState(itemID);
                break;
            case 504:
                stationState = new StoveState(acceptedIDs);
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

    void Update()
    {
        stationState.UpdateState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStationScript : MonoBehaviour
{
    private enum StationTypes { Dispenser, Furnace }
    [SerializeField]
    private StationTypes stationType = StationTypes.Dispenser;

    //Aca la verdad no se si esto sea la mejor opcion, pero fue lo unico que se me ocurrio para poder pasarle al dispenserState un ID del objeto que dispensa, lo malo es que ningun otro state va a usar esta variable
    [SerializeField]
    private int itemID;

    IStationState stationState;

    void Start()
    {
        switch (stationType)
        {
            case StationTypes.Dispenser:
                stationState = new DispenserState(itemID);
                break;
            case StationTypes.Furnace:
                stationState = new FurnaceState(-1);
                break;
        }
    }

    public int OnAccessEmpty()
    {
        return stationState.OnAccessEmpty();
    }

    public bool OnAccessWithID(int ID, GameObject gameObject)
    {
        return stationState.OnAccessWithID(ID, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        stationState.UpdateState();
    }
}

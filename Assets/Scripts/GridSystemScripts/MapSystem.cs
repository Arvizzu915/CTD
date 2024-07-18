using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    //Aca minimo es izquierda inferior, y maximo es derecha superior
    [SerializeField]
    private GameObject minMapPosObject, maxMapPosObject, minKitchenPosObject, maxKitchenPosObject;
    private Vector3Int minMapPos, maxMapPos, minKitchenPos, maxKitchenPos;
    [SerializeField]
    private GameObject[] stationObjects, containerObjects, specialObjects;
    
    [SerializeField]
    ObjectPlacer objectPlacer;

    [SerializeField]
    private Grid grid;

    private GridData placeableObjectsData, mapObjectsData;


    void Start()
    {
        
    }

    public void SetGridData(GridData placeableObjectsData, GridData mapObjectsData)
    {
        this.placeableObjectsData = placeableObjectsData;
        this.mapObjectsData = mapObjectsData;
        minMapPos = grid.WorldToCell(minMapPosObject.transform.position);
        maxMapPos = grid.WorldToCell(maxMapPosObject.transform.position);
        minKitchenPos = grid.WorldToCell(minKitchenPosObject.transform.position);
        maxKitchenPos = grid.WorldToCell(maxKitchenPosObject.transform.position);
        SetStations();
        SetSpecialObjects();
        SetContainers();
        SetMap();
        SetKitchen();
    }

    private void SetStations()
    {
        //primero ponemos las estaciones en el diccionario (mapa)
        for (int i = 0; i < stationObjects.Length; i++)
        {
            //estas 2 lineas que parece que no hacen nada, lo que hacen es mover el objeto a justo la casilla correcta
            Vector3Int gridPosition = grid.WorldToCell(stationObjects[i].transform.position);
            objectPlacer.MoveObject(stationObjects[i], grid.CellToWorld(gridPosition));
            mapObjectsData.AddObjectAt(gridPosition, stationObjects[i].GetComponent<BaseStationScript>().stationID, stationObjects[i]);
        }
    }

    private void SetSpecialObjects()
    {
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        for (int i = 0; i < specialObjects.Length; i++)
        {
            Vector3Int gridPosition = grid.WorldToCell(specialObjects[i].transform.position);
            GameObject station =  mapObjectsData.GetGameObjectAt(gridPosition);
            //aca es 0 porque por ahora solo hay un objeto especial, pero quiza despues haya que ponerles un script que nos de su ID, o hacer un arreglo aqui que tenga todos los IDs
            station.GetComponent<BaseStationScript>().CanEnterStation(0, specialObjects[i]);
        }
    }

    private void SetContainers()
    {
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        for (int i = 0; i < containerObjects.Length; i++)
        {
            Vector3Int gridPosition = grid.WorldToCell(containerObjects[i].transform.position);
            GameObject station = mapObjectsData.GetGameObjectAt(gridPosition);
            //aca es 0 porque por ahora solo hay un objeto especial, pero quiza despues haya que ponerles un script que nos de su ID, o hacer un arreglo aqui que tenga todos los IDs
            station.GetComponent<BaseStationScript>().CanEnterStation(containerObjects[i].GetComponent<BaseContainerScript>().containerID, containerObjects[i]);
        }
    }

    private void SetMap()
    {
        for (int z = minMapPos.z; z <= maxMapPos.z; z++)
        {
            for (int x = minMapPos.x; x <= maxMapPos.x; x++)
            {
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                if (mapObjectsData.GetObjectIDAt(gridPosition) == -1)
                {
                    mapObjectsData.AddObjectAt(gridPosition, 0, null);
                }
            }
        }
    }

    private void SetKitchen()
    {
        for (int z = minKitchenPos.z; z <= maxKitchenPos.z; z++)
        {
            for (int x = minKitchenPos.x; x <= maxKitchenPos.x; x++)
            {
                Vector3Int gridPosition = new Vector3Int(x, 0, z);
                if (mapObjectsData.GetObjectIDAt(gridPosition) == 0)
                {
                    mapObjectsData.RemoveObjectAt(gridPosition);
                }
            }
        }
    }
}

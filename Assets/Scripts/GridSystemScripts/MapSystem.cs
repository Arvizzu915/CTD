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

    private GridData towersObjectsData, mapObjectsData;


    void Start()
    {
        
    }

    public void SetGridData(GridData placeableObjectsData, GridData mapObjectsData)
    {
        this.towersObjectsData = placeableObjectsData;
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
        foreach (GameObject station in stationObjects)
        {
            //estas 3 lineas que parece que no hacen nada, lo que hacen es mover el objeto a justo la casilla correcta
            Vector3Int gridPosition = grid.WorldToCell(station.transform.position);
            Vector3 adjustedPosition = grid.CellToWorld(gridPosition);
            adjustedPosition = new Vector3(adjustedPosition.x + 0.5f, adjustedPosition.y, adjustedPosition.z + 0.5f);
            station.transform.position = adjustedPosition;
            mapObjectsData.AddObjectAt(gridPosition, station.GetComponent<BaseStationScript>().stationID, station);
        }
    }

    private void SetSpecialObjects()
    {
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        foreach (GameObject specialObject in specialObjects)
        {
            Vector3Int gridPosition = grid.WorldToCell(specialObject.transform.position);
            //esta linea de abajo se hace porque asumimos que el objeto esta en una celda arriba de la estacion
            Vector3Int stationGridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z);
            GameObject station = mapObjectsData.GetGameObjectAt(stationGridPosition);
            //aca es 0 porque por ahora solo hay un objeto especial, pero quiza despues haya que ponerles un script que nos de su ID, o hacer un arreglo aqui que tenga todos los IDs
            station.GetComponent<BaseStationScript>().CanEnterStation(0, specialObject);
        }
    }

    private void SetContainers()
    {
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        foreach (GameObject container in containerObjects)
        {
            Vector3Int gridPosition = grid.WorldToCell(container.transform.position);
            Vector3Int stationGridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z);
            GameObject station = mapObjectsData.GetGameObjectAt(stationGridPosition);
            station.GetComponent<BaseStationScript>().CanEnterStation(container.GetComponent<BaseContainerScript>().containerID, container);
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

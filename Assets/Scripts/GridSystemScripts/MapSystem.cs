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
    private GameObject stationsParent, containersParent, specialObjectsParent;
    
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
        if (stationsParent == null)
            return;
        //primero ponemos las estaciones en el diccionario (mapa)
        foreach (Transform stationChild in stationsParent.transform)
        {
            //estas 3 lineas que parece que no hacen nada, lo que hacen es mover el objeto a justo la casilla correcta
            Vector3Int gridPosition = grid.WorldToCell(stationChild.position);
            Vector3 adjustedPosition = grid.CellToWorld(gridPosition);
            adjustedPosition = new Vector3(adjustedPosition.x + 0.5f, adjustedPosition.y, adjustedPosition.z + 0.5f);
            stationChild.position = adjustedPosition;
            mapObjectsData.AddObjectAt(gridPosition, stationChild.gameObject.GetComponent<BaseStationScript>().stationID, stationChild.gameObject);
        }
    }

    private void SetSpecialObjects()
    {
        if (specialObjectsParent == null)
            return;
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        int specialObjects = specialObjectsParent.transform.childCount;
        for (int i = 0; i < specialObjects; i++)
        {
            Transform specialChild = specialObjectsParent.transform.GetChild(0);
            Vector3Int gridPosition = grid.WorldToCell(specialChild.position);
            //esta linea de abajo se hace porque asumimos que el objeto esta en una celda arriba de la estacion
            Vector3Int stationGridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z);
            GameObject station = mapObjectsData.GetGameObjectAt(stationGridPosition);
            //aca es 0 porque por ahora solo hay un objeto especial, pero quiza despues haya que ponerles un script que nos de su ID, o hacer un arreglo aqui que tenga todos los IDs
            station.GetComponent<BaseStationScript>().CanEnterStation(0, specialChild.gameObject);
        }
    }

    private void SetContainers()
    {
        if (containersParent == null)
            return;
        //Aca es para poner los objetos dentro de sus respetivas estaciones, no checamos si pueden entrar porque en teoria nosotros hacemos el mapa, y pondremos todo donde puede estar
        int containers = containersParent.transform.childCount;
        for (int i = 0; i < containers; i++)
        {
            //esto se hace con for y no foreach porque sus hijos dejan de ser sus hijos al meterlos a las estaciones, entonces hay que hacerlo de esta manera, para que 
            //siempre use al primer hijo, y cuando desaparesca seguir con el primer hijo (ya que el original dejo de ser hijo, asi que el segundo hijo es ahora el primero)
            Transform containerChild = containersParent.transform.GetChild(0);
            Vector3Int gridPosition = grid.WorldToCell(containerChild.position);
            Vector3Int stationGridPosition = new Vector3Int(gridPosition.x, gridPosition.y - 1, gridPosition.z);
            GameObject station = mapObjectsData.GetGameObjectAt(stationGridPosition);
            station.GetComponent<BaseStationScript>().CanEnterStation(containerChild.gameObject.GetComponent<BaseContainerScript>().containerID, containerChild.gameObject);
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

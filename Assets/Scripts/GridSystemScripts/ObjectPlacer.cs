using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    //Ahora este script es el encargado de crear los objetos nuevos, moverlos y destruirlos, es el unico que puede crear nuevos objetos, mas no el unico en poder destruirlos o moverlos
    [SerializeField]
    private List<GameObject> placedStations = new();

    [SerializeField]
    PlaceableObjectsDatabaseSO placeableObjectsDatabase;

    public GameObject CreateNewObject(int ID)
    {
        int objectIndex = placeableObjectsDatabase.objectsPlacementData.FindIndex(data => data.ID == ID);
        GameObject newOject = Instantiate(placeableObjectsDatabase.objectsPlacementData[objectIndex].Prefab);
        return newOject;
    }

    public GameObject CreateNewStation(int ID, StationsDatabaseSO stationsDatabase)
    {
        int objectIndex = stationsDatabase.stationsData.FindIndex(data => data.ID == ID);
        GameObject newOject = Instantiate(stationsDatabase.stationsData[objectIndex].Prefab);
        return newOject;
    }

    public void MoveObject(GameObject gameObject, Vector3 position)
    {
        if(gameObject == null)
        {
            print("que rayos paso aqui, porque me mandas un objeto vacio??");
        }
        gameObject.transform.position = position;
    }

    public void DeleteObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public int PlaceStation(GameObject prefab, Vector3 position, int index)
    {
        //Por ahora esto no se usa, asi que quiza sea eliminado junto con la lista de estaciones si es que no son necesarias
        if (index == -1)
        {
            GameObject newOject = Instantiate(prefab);
            newOject.transform.position = position;
            for (int i = 0; i < placedStations.Count; i++)
            {
                if (placedStations[i] == null)
                {
                    //Aca basicamente si no tiene en index nada (es un objeto nuevesito, entonces busca entre los objetos colocados haber si hay uno vacio, si hay lo pone ahi y ese sera su nuevo index.
                    placedStations[i] = newOject;
                    return i;
                }
            }
            //Si estan todos llenos, entonces agrega un nuevo elemento a la lista y pasa su index.
            placedStations.Add(newOject);
            return placedStations.Count - 1;
        }
        else
        {
            if (index < placedStations.Count)
            {
                //Si ya tiene index, significa que es un objeto que si existe ya, entonces nomas hay que activarlo
                placedStations[index].SetActive(true);
                placedStations[index].transform.position = position;
                return index;
            }
            else
            {
                //si por alguna razon el objeto ya trae index, pero no esta en la lista, entonces algo salio muy mal
                throw new System.Exception($"No index found in list {index}");
            }
        }
    }

    public GameObject GetStationWithIndex(int index)
    {
        if (placedStations.Count <= index || index == -1)
            return null;
        return placedStations[index].gameObject;
    }
}

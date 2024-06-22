using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    [SerializeField]
    private List<GameObject> placedStations = new();

    public int PlaceObject(GameObject prefab, Vector3 position, float yOffSet, int index)
    {
        if(index == -1)
        {
            GameObject newOject = Instantiate(prefab);
            newOject.transform.position = new Vector3(position.x, position.y + yOffSet, position.z);
            for (int i = 0; i < placedGameObjects.Count; i++)
            {
                if(placedGameObjects[i] == null)
                {
                    //Aca basicamente si no tiene en index nada (es un objeto nuevesito, entonces busca entre los objetos colocados haber si hay uno vacio, si hay lo pone ahi y ese sera su nuevo index.
                    placedGameObjects[i] = newOject;
                    return i;
                }
            }
            //Si estan todos llenos, entonces agrega un nuevo elemento a la lista y pasa su index.
            placedGameObjects.Add(newOject);
            return placedGameObjects.Count - 1;
        }
        else
        {
            if(index < placedGameObjects.Count)
            {
                //Si ya tiene index, significa que es un objeto que si existe ya, entonces nomas hay que activarlo
                placedGameObjects[index].SetActive(true);
                placedGameObjects[index].transform.position = new Vector3(position.x, position.y + yOffSet, position.z);
                return index;
            }
            else
            {
                //si por alguna razon el objeto ya trae index, pero no esta en la lista, entonces algo salio muy mal
                throw new System.Exception($"No index found in list {index}");
            }
        }
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        //por ahora solo oculta el objeto, pero quiza sea mejor despues moverlo a las manos del jugador
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;
        placedGameObjects[gameObjectIndex].SetActive(false);
    }

    public void PermaRemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }

    public int PlaceStation(GameObject prefab, Vector3 position, int index)
    {
        if (index == -1)
        {
            GameObject newOject = Instantiate(prefab);
            newOject.transform.position = position;
            for (int i = 0; i < placedGameObjects.Count; i++)
            {
                if (placedGameObjects[i] == null)
                {
                    //Aca basicamente si no tiene en index nada (es un objeto nuevesito, entonces busca entre los objetos colocados haber si hay uno vacio, si hay lo pone ahi y ese sera su nuevo index.
                    placedGameObjects[i] = newOject;
                    return i;
                }
            }
            //Si estan todos llenos, entonces agrega un nuevo elemento a la lista y pasa su index.
            placedGameObjects.Add(newOject);
            return placedGameObjects.Count - 1;
        }
        else
        {
            if (index < placedGameObjects.Count)
            {
                //Si ya tiene index, significa que es un objeto que si existe ya, entonces nomas hay que activarlo
                placedGameObjects[index].SetActive(true);
                placedGameObjects[index].transform.position = position;
                return index;
            }
            else
            {
                //si por alguna razon el objeto ya trae index, pero no esta en la lista, entonces algo salio muy mal
                throw new System.Exception($"No index found in list {index}");
            }
        }
    }
}

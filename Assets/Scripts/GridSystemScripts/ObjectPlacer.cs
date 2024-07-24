using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    //Ahora este script es el encargado de crear los objetos nuevos, moverlos y destruirlos, es el unico que puede crear nuevos objetos, mas no el unico en poder destruirlos o moverlos
    [SerializeField]
    ObjectsDatabaseSO specialItemsDatabase, containersDatabase, ingredientsDatabase, spicesDatabase, towersDatabase, stationsDatabase;

    public GameObject CreateNewObject(int ID)
    {
        ObjectsDatabaseSO objectsDatabase = null;
        if (ID >= 0 && ID < 100)
        {
            objectsDatabase = specialItemsDatabase;
        }
        else if (ID >= 100 && ID < 200)
        {
            objectsDatabase = containersDatabase;
        }
        else if (ID >= 200 && ID < 300)
        {
            objectsDatabase = ingredientsDatabase;
        }
        else if (ID >= 300 && ID < 400)
        {
            objectsDatabase = spicesDatabase;
        }
        else if (ID >= 400 && ID < 500)
        {
            objectsDatabase = towersDatabase;
        }
        else if (ID >= 500 && ID < 600)
        {
            objectsDatabase = stationsDatabase;
        }

        if(objectsDatabase != null)
        {
            int objectIndex = objectsDatabase.objectsData.FindIndex(data => data.ID == ID);
            GameObject newOject = Instantiate(objectsDatabase.objectsData[objectIndex].Prefab);
            return newOject;
        }
        else
        {
            Debug.Log("Invalid ID");
            return null;
        }
        
    }

    public void DeleteObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}

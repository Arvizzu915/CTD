using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stations Objects Database SO", menuName = "Stations Objects Database SO")]
public class StationsDatabaseSO : ScriptableObject
{
    public List<ObjectPlacementData> objectsPlacementData;
    public List<StationData> stationsData;
}

[Serializable]
public class StationData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public CookingObjectData.Type[] AcceptedCookingObjects { get; private set; }
    [field: SerializeField]
    public bool givesObject { get; private set; }
    [field: SerializeField]
    public bool removesObject { get; private set; }
}
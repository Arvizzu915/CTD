using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stations Objects Database SO", menuName = "Stations Objects Database SO")]
public class StationsDatabaseSO : ScriptableObject
{
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
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
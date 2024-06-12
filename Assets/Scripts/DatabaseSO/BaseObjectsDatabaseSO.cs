using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Objects Database SO", menuName = "Base Objects Database SO")]
public class BaseObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectPlacementData> objectsPlacementData;
}

[Serializable]
public class ObjectPlacementData
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
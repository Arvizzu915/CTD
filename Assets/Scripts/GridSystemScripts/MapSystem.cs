using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    [SerializeField]
    private Vector3Int minMapPos;
    [SerializeField]
    private Vector3Int maxMapPos;
    [SerializeField]
    private int[] mapObjectsID;
    [SerializeField]
    private int[] mapObjectsAmmount;
    [SerializeField]
    private Vector3Int[] mapObjectsPositions;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private StationsDatabaseSO stationsDatabaseSO;
    [SerializeField]
    GridData mapObjectsData;
    [SerializeField]
    ObjectPlacer objectPlacer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public interface IStationState
{
    int GetContainedItemID();
    GameObject GetContainedItemGameObject();
    void EmptyStation();
    int CanEnterStation(int ID, GameObject gameObject);
    void OnAccess2();
    void UpdateState();
}

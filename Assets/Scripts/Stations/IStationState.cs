using UnityEngine;

public interface IStationState
{
    int GetContainedItemID();
    GameObject GetContainedItemGameObject();
    void EmptyStation();
    bool OnAccessWithID(int ID, GameObject gameObject);
    void OnAccess2();
    void UpdateState();
}

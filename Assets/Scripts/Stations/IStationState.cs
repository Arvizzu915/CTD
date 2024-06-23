using UnityEngine;

public interface IStationState
{
    int OnAccessEmpty();
    bool OnAccessWithID(int ID, GameObject gameObject);
    void UpdateState();
}

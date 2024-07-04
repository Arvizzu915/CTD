using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction1(Vector3Int gridPosition);
    void OnAction2(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}
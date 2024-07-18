using UnityEngine;

public interface IContainerState
{
    int GetContainedItemID();
    GameObject GetContainedItemGameObject();
    void EmptyContainer(bool deleteContainedItemGameObject);
    int CanEnterContainer(int ID, GameObject gameObject);
    void UpdateModel();
}

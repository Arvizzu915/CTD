using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICToICContainerState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] startingModelsForIDs;
    [SerializeField]
    private GameObject[] finishingModelsForIDs;
    [SerializeField]
    private List<int> acceptedObjectsIDs;
    [SerializeField]
    private int[] startingStatesPerID;
    [SerializeField]
    private int finishingState;

    private int containedItemID = -1;
    private GameObject containedItemGameObject = null;
    private int currentContainedObjectIndex = -1;
    private bool isFinished = false;


    public bool CanPlaceObjectInContainer(int objectID, int objectState)
    {
        if (containedItemID != -1)
            return false;
        int objectIndex = acceptedObjectsIDs.FindIndex(ID => ID == objectID);
        if (objectIndex == -1)
            return false;
        if (objectState != startingStatesPerID[objectIndex])
            return false;
        containedItemID = objectID;
        currentContainedObjectIndex = objectIndex;
        isFinished = false;
        ShowModelForID();
        return true;
    }

    public bool CanChangeContainedItemState3(int newState)
    {
        containedItemGameObject.GetComponent<BaseIngredientScript>().ChangeState2(newState, true);
        return true;
    }

    public void EmptyContainer()
    {
        if(containedItemID != -1)
        {
            HideModelForID();
            isFinished = false;
            containedItemID = -1;
            currentContainedObjectIndex = -1;
        }
    }

    public void ChangeContainedObjectsToFinished()
    {
        if(containedItemID != -1 && !isFinished)
        {
            HideModelForID();
            isFinished = true;
            ShowModelForID();
        }
    }

    public int GetContainedObjectID()
    {
        return containedItemID;
    }

    public int GetContainedObjectState()
    {
        if (containedItemID == -1)
            return -1;
        if (isFinished)
        {
            return finishingState;
        }
        else
        {
            return startingStatesPerID[currentContainedObjectIndex];
        }
    }

    private void ShowModelForID()
    {
        if(isFinished)
        {
            finishingModelsForIDs[currentContainedObjectIndex].SetActive(true);
        }
        else if (!isFinished)
        {
            startingModelsForIDs[currentContainedObjectIndex].SetActive(true);
        }
    }

    private void HideModelForID()
    {
        if (isFinished)
        {
            finishingModelsForIDs[currentContainedObjectIndex].SetActive(false);
        }
        else if (!isFinished)
        {
            startingModelsForIDs[currentContainedObjectIndex].SetActive(false);
        }
    }
}

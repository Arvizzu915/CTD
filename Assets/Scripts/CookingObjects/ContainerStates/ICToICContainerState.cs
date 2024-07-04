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

    private int currentContainedObjectID = -1;
    private int currentContainedObjectIndex = -1;
    private bool isFinished = false;


    public bool CanPlaceObjectInContainer(int objectID, int objectState)
    {
        if (currentContainedObjectID != -1)
            return false;
        int objectIndex = acceptedObjectsIDs.FindIndex(ID => ID == objectID);
        if (objectIndex == -1)
            return false;
        if (objectState != startingStatesPerID[objectIndex])
            return false;
        currentContainedObjectID = objectID;
        currentContainedObjectIndex = objectIndex;
        isFinished = false;
        ShowModelForID();
        return true;
    }

    public void EmptyContainer()
    {
        if(currentContainedObjectID != -1)
        {
            HideModelForID();
            isFinished = false;
            currentContainedObjectID = -1;
            currentContainedObjectIndex = -1;
        }
    }

    public void ChangeContainedObjectsToFinished()
    {
        if(currentContainedObjectID != -1 && !isFinished)
        {
            HideModelForID();
            isFinished = true;
            ShowModelForID();
        }
    }

    public int GetContainedObjectID()
    {
        return currentContainedObjectID;
    }

    public int GetContainedObjectState()
    {
        if (currentContainedObjectID == -1)
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

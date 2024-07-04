using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCToICContainerState : MonoBehaviour
{
    [SerializeField]
    private GameObject startingModel;
    [SerializeField]
    private GameObject[] finishingModels;
    [SerializeField]
    private GameObject failedFinishingdModel;
    [SerializeField]
    private List<int> acceptedObjectsIDs;
    [SerializeField]
    private List<int> finishingObjectsIDs;
    [SerializeField]
    private int[] startingStatesPerID;
    [SerializeField]
    private int maxContainedObjects;

    private List<int> currentContainedObjectsIDs;
    private int finishingContainedObjectIndex = -1;
    private bool isFinished = false;


    public bool CanPlaceObjectsInContainer(List<int> objectsIDs, int[] objectsStates)
    {
        if (isFinished)
            return false;
        if (currentContainedObjectsIDs.Count + objectsIDs.Count >= maxContainedObjects)
            return false;
        foreach (int objectID in objectsIDs)
        {
            int objectIndex = acceptedObjectsIDs.FindIndex(ID => ID == objectID);
            if (objectIndex == -1)
                return false;
            if (objectsStates[objectIndex] != startingStatesPerID[objectIndex])
                return false;
            //Aqui vendria algo de checar que no este ya en la lista, pero por ahora esta permitido meterle mas de 1 del mismo objeto
        }

        foreach (int objectID in objectsIDs)
        {
            currentContainedObjectsIDs.Add(objectID);
        }
        isFinished = false;
        ShowModelForID();
        return true;
    }

    public void EmptyContainer()
    {
        if (currentContainedObjectsIDs.Count > 0)
        {
            HideModelForID();
            if (isFinished)
            {
                finishingContainedObjectIndex = -1;
            }
            else
            {
                foreach (int containedObjectID in currentContainedObjectsIDs)
                {
                    currentContainedObjectsIDs.Remove(containedObjectID);
                }
            }
            isFinished = false;
        }
    }

    public void ChangeToFinishedObject(int finishedObjectID)
    {
        if(currentContainedObjectsIDs.Count > 0 && !isFinished)
        {
            HideModelForID();
            isFinished = true;
            finishingContainedObjectIndex = finishingObjectsIDs.FindIndex(ID => ID == finishedObjectID);
            ShowModelForID();
        }
    }

    public  List<int> GetContainedObjectsIDs()
    {
        return currentContainedObjectsIDs;
    }

    public int GetContainedObjectState()
    {
        //nose, quiza ni se vaya a usar
        return -1;
    }

    private void ShowModelForID()
    {
        if (isFinished)
        {
            if (finishingContainedObjectIndex == -1)
            {
                failedFinishingdModel.SetActive(true);
            }
            else
            {
                finishingModels[finishingContainedObjectIndex].SetActive(true);
            }
        }
        else if (!isFinished)
        {
            startingModel.SetActive(true);
        }
    }

    private void HideModelForID()
    {
        if (isFinished)
        {
            if (finishingContainedObjectIndex == -1)
            {
                failedFinishingdModel.SetActive(false);
            }
            else
            {
                finishingModels[finishingContainedObjectIndex].SetActive(false);
            }
        }
        else if (!isFinished)
        {
            startingModel.SetActive(false);
        }
    }
}

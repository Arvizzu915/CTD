using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffSet = 0.05f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        previewMaterialInstance.color = Color.white;
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingDefaultPreview(Vector2Int size)
    {
        PrepareCellIndicator(size);
        cellIndicator.SetActive(true);
    }

    public void StartShowingObjectPreview(GameObject prefab)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
    }

    private void PrepareCellIndicator(Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            //Aca si haces la y mas alta, puedes hacer que sea como una caja que rodea la torre en vez de solo una plataforma en el suelo, podrias poner size.y pa que se adapte al tamaño de la torre
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingDefaultPreview()
    {
        cellIndicator.SetActive(false);
    }

    public void StopShowingObjectPreview()
    {
        if (previewObject == null)
            return;
        Destroy(previewObject);
        previewObject = null;
    }

    public void UpdateDefaultPreviewPosition(Vector3 position, int validity)
    {
        MoveCellIndicator(position);
        ApplyFeedbackToCellIndicator(validity);
    }

    public void UpdateObjectPreviewPosition(Vector3 position, int validity, int mapObjectID)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToObjectPreview(validity, mapObjectID);
        }
    }

    private void ApplyFeedbackToCellIndicator(int validity)
    {
        Color c;
        if (validity == 0)
        {
            c = Color.red;
        }
        else
        {
            c = Color.green;
        }
        c.a = 0.392f;
        cellIndicatorRenderer.material.color = c;
    }

    private void ApplyFeedbackToObjectPreview(int validity, int mapObjectID)
    {
        if(validity == 0)
        {
            previewObject.SetActive(false);
        }
        else
        {
            previewObject.SetActive(true);
            if(mapObjectID == -1)
            {
                previewYOffSet = 0.05f;
            }
            else if(mapObjectID == 0)
            {
                previewYOffSet = 1.05f;
            }
        }
        
    }

    private void MoveCellIndicator(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffSet, position.z);
    }
}

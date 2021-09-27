using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    #region Variables
    [Header("Sorting Configuration")]
    [Tooltip("All objects to be sorted as one")]
    [SerializeField]
    private List<Renderer> sortingObjects;

    [Tooltip("Sorting offset position")]
    [SerializeField]
    private float sortingOffset;


    [Tooltip("Sort just once, still not static")]
    [SerializeField]
    private bool staticSort;
    #endregion 

    private void Start()
    {
        if(staticSort)
            sortingObjects.AutoSortLayers(transform.position.y - sortingOffset);
    }

    void LateUpdate()
    {
        if(!staticSort)
            sortingObjects.AutoSortLayers(transform.position.y - sortingOffset);
    }
}

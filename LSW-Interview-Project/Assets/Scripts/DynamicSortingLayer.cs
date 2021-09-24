using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    [Header("Sorting Configuration")]
    [Tooltip("All objects to be sorted as one")]
    [SerializeField]
    private List<Renderer> sortingObjects;

    [Tooltip("Collider 2D for get botton collider world position")]
    [SerializeField]
    private Collider2D sortingCollider;

    void Awake()
    {
    }

    void LateUpdate()
    {
        sortingObjects.AutoSortLayers(sortingCollider);
    }
}

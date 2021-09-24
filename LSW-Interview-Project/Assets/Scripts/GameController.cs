using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //The base sorting value for all sorting objects
    public static int baseSortingValue { get { return 5000; } }

    void Start()
    {
        //// Locks at 60 fps, to avoid bugs with different performances, i used only for this project beside mobile ones
        //Application.targetFrameRate = 60;
        SortStaticSprites();
    }

    /// <summary>
    /// Gets the botton y of static objects
    /// </summary>
    private void SortStaticSprites()
    {
        Renderer[] sprites = FindObjectsOfType<Renderer>();
        sprites.AutoSortLayers(true);
    }

}

// Extensions used to aply methods in the base of object type
public static class Extensions
{
    /// <summary>
    /// Sort all SpriteRenderers based on y position
    /// </summary>
    /// <param name="sRenderer">Sprites do sort</param>
    /// <param name="statics">Sort only static, or no static</param>
    public static void AutoSortLayers(this Renderer[] sRenderer, bool statics)
    {
        foreach (Renderer s in sRenderer)
        {
            if (s.gameObject.isStatic == statics)
            {
                Bounds objectBounds = s.GetComponent<Collider2D>().bounds;
                if (objectBounds != null)
                {
                    float objectBottom = objectBounds.center.y - (objectBounds.size.y /2);
                    s.sortingOrder = (int)(GameController.baseSortingValue - objectBottom);
                }
                else s.sortingOrder = (int)(GameController.baseSortingValue - s.transform.position.y);
            }
        }
    }

    /// <summary>
    /// Sort all SpriteRenderers based on y position
    /// </summary>
    /// <param name="sRenderer">Sprites do sort</param>
    /// <param name="statics">Sort only static, or no static</param>
    public static void AutoSortLayers(this List<Renderer> sRenderer, bool statics)
    {
        foreach (Renderer s in sRenderer)
        {
            if (s.gameObject.isStatic == statics)
            {
                Bounds objectBounds = s.GetComponent<Collider2D>().bounds;
                if (objectBounds != null)
                {
                    float objectBottom = objectBounds.center.y - (objectBounds.size.y / 2);
                    s.sortingOrder = (int)(GameController.baseSortingValue - objectBottom);
                }
                else s.sortingOrder = (int)(GameController.baseSortingValue - s.transform.position.y);
            }
        }
    }

    /// <summary>
    /// Sort all SpriteRenderers based on y position
    /// </summary>
    /// <param name="sRenderer">Sprites do sort</param>
    /// <param name="statics">Sort bse on collider</param>
    public static void AutoSortLayers(this List<Renderer> sRenderer, Collider2D baseCollider)
    {
        foreach (Renderer s in sRenderer)
        {
            Bounds objectBounds = baseCollider.GetComponent<Collider2D>().bounds;
            float objectBottom = objectBounds.center.y - (objectBounds.size.y / 2);
            s.sortingOrder = (int)(GameController.baseSortingValue - objectBottom);
        }
    }

    /// <summary>
    /// Sort all SpriteRenderers based on y position
    /// </summary>
    /// <param name="sRenderer">Sprites do sort</param>
    /// <param name="statics">Sort bse on collider</param>
    public static void AutoSortLayers(this Renderer[] sRenderer, Collider2D baseCollider)
    {
        foreach (Renderer s in sRenderer)
        {
            Bounds objectBounds = baseCollider.GetComponent<Collider2D>().bounds;
            float objectBottom = objectBounds.center.y - (objectBounds.size.y / 2);
            s.sortingOrder = (int)(GameController.baseSortingValue - objectBottom);
        }
    }

}

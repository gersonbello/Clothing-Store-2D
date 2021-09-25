using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Cursor Options")]
    [Tooltip("Base Cursor Texture")]
    public Texture2D baseCursorTexture;


    //The base sorting value for all sorting objects
    public static int baseSortingValue { get { return 100000; } }

    // Make a GameController singleton
    private static GameController _gcInstance;
    public static GameController gcInstance
    {
        get
        {
            if (_gcInstance == null)
            {
                _gcInstance = FindObjectOfType<GameController>();

                if (_gcInstance == null) 
                {
                    GameController gc = Instantiate(new GameObject()).AddComponent<GameController>();
                    _gcInstance = gc;
                }
            }
            return _gcInstance;
        }
    }

    // Public reference to money controller
    public MoneyController moneyController { get; private set; }
    public GridAndNodes worldGrid { get; private set; }
    public PlayerBehaviour playerBehaviour { get; private set; }

    private void Awake()
    {
        if (_gcInstance == null) _gcInstance = this; else if (_gcInstance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        moneyController = FindObjectOfType<MoneyController>();
        worldGrid = FindObjectOfType<GridAndNodes>();
        foreach(PlayerBehaviour pb in FindObjectsOfType<PlayerBehaviour>())
        {
            if (pb.CompareTag("Player")) playerBehaviour = pb;
        }

        baseCursorTexture = Resources.Load<Texture2D>("Sprites/Cursors/BaseCursor");
    }

    void Start()
    {
        Cursor.SetCursor(baseCursorTexture, Vector2.zero, CursorMode.Auto);

        SortStaticSprites();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && worldGrid != null && playerBehaviour != null)
        {
            Vector3 playerPos = playerBehaviour.transform.position;
            playerBehaviour.SetPath();
        }
    }

    /// <summary>
    /// Gets the botton y of static objects
    /// </summary>
    private void SortStaticSprites()
    {
        Renderer[] sprites = FindObjectsOfType<Renderer>();
        sprites.AutoSortLayers(true);
    }

    /// <summary>
    /// Restore the cursor after object on click action
    /// </summary>
    /// <param name="timeBeforeRestore">Time to wait before changing the cursor</param>
    /// <param name="interactedObject">Last object interacted</param>
    /// <param name="mouseOverObjectTexture">Last object interacted cursor</param>
    /// <param name="overHotspot">Last object interacted cursor hotspot</param>
    /// <returns></returns>
    public IEnumerator RestoreCursorAfter(float timeBeforeRestore, GameObject interactedObject, Texture2D mouseOverObjectTexture, Vector2 overHotspot)
    {
        yield return new WaitForSeconds(timeBeforeRestore);
        if(interactedObject == null || !interactedObject.gameObject.activeInHierarchy)
            Cursor.SetCursor(baseCursorTexture, Vector2.zero, CursorMode.Auto);
        else Cursor.SetCursor(mouseOverObjectTexture, overHotspot, CursorMode.Auto);
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
                if (s.GetComponent<Collider2D>() != null)
                {
                    Bounds objectBounds = s.GetComponent<Collider2D>().bounds;
                    float objectBottom = objectBounds.center.y - (objectBounds.size.y /2);
                    s.sortingOrder = (int)((GameController.baseSortingValue - objectBottom) /.1f);
                }
                else s.sortingOrder = (int)((GameController.baseSortingValue - s.transform.position.y) / .1f);
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
                if (s.GetComponent<Collider2D>() != null)
                {
                    Bounds objectBounds = s.GetComponent<Collider2D>().bounds;
                    float objectBottom = objectBounds.center.y - (objectBounds.size.y / 2);
                    s.sortingOrder = (int)((GameController.baseSortingValue - objectBottom) / .1f);
                }
                else s.sortingOrder = (int)((GameController.baseSortingValue - s.transform.position.y) / .1f);
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
            s.sortingOrder = (int)((GameController.baseSortingValue - objectBottom) / .1f);
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
            s.sortingOrder = (int)((GameController.baseSortingValue - objectBottom)/ .1f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables
    [Header("Cursor Options")]
    [Tooltip("Base Cursor Texture")]
    public Texture2D baseCursorTexture;
    [Tooltip("Base Cursor Over Button Texture")]
    public Texture2D baseCursorOverButtonTexture;
    // Is generic button cursor active
    bool cursorOverButton;

    [Header("Sorting Options")]
    [Tooltip("Layers to auto sort")]
    [SerializeField]
    private LayerMask sortedLayers;


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
    [HideInInspector]
    public DialogueSystem dialogueSystem { get; set; }
    #endregion

    #region Unity Methods
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
        baseCursorOverButtonTexture = Resources.Load<Texture2D>("Sprites/Cursors/HandPointingCursor");
    }
    void Start()
    {
        Cursor.SetCursor(baseCursorTexture, Vector2.zero, CursorMode.Auto);

        SortStaticSprites();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !IsPointerOverObject(Input.mousePosition) && worldGrid != null && playerBehaviour != null)
        {
            playerBehaviour.SetPath();
            Cursor.SetCursor(baseCursorOverButtonTexture, new Vector2(0, 10), CursorMode.Auto);
            StopCoroutine("RestoreCursorAfter");
            StartCoroutine(RestoreCursorAfter(.2f));
        }
        if(IsPointerOverObject(Input.mousePosition) || cursorOverButton)
        {
            VerifyButtonCursorInteraction(Input.mousePosition);
        }
    }
    #endregion

    #region Sorting
    /// <summary>
    /// Gets the botton y of static objects
    /// </summary>
    private void SortStaticSprites()
    {
        Renderer[] sprites = FindObjectsOfType<Renderer>();
        sprites.AutoSortLayers(true, sortedLayers);
    }
    #endregion

    #region Cursor
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
    /// <summary>
    /// Restore the cursor after object on click action
    /// </summary>
    /// <param name="timeBeforeRestore">Time to wait before changing the cursor</param>
    /// <param name="interactedObject">Last object interacted</param>
    /// <param name="mouseOverObjectTexture">Last object interacted cursor</param>
    /// <param name="overHotspot">Last object interacted cursor hotspot</param>
    /// <returns></returns>
    public IEnumerator RestoreCursorAfter(float timeBeforeRestore)
    {
        yield return new WaitForSeconds(timeBeforeRestore);
        Cursor.SetCursor(baseCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Veify any raycast target at position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsPointerOverObject(Vector2 pos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Veify any raycast target at position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public void VerifyButtonCursorInteraction(Vector2 pos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach(RaycastResult r in results)
        {
            Button interactableObject = r.gameObject.GetComponent<Button>();
            if (interactableObject != null)
            {
                cursorOverButton = true;
                Cursor.SetCursor(baseCursorOverButtonTexture, new Vector2(0, 10), CursorMode.Auto);
                return;
            }
        }
        cursorOverButton = false;
        Cursor.SetCursor(baseCursorTexture, Vector2.zero, CursorMode.Auto);
    }
    #endregion
}

/// <summary>
/// xtensions used to aply methods in the base of object type
/// </summary>
public static class Extensions
{
    // A way to sort whitout using pivots and selec what's sorting and what isn't
    #region Sorting
    /// <summary>
    /// Sort all SpriteRenderers based on y position
    /// </summary>
    /// <param name="sRenderer">Sprites do sort</param>
    /// <param name="statics">Sort only static, or no static</param>
    public static void AutoSortLayers(this Renderer[] sRenderer, bool statics, LayerMask sortedLayers)
    {
        foreach (Renderer s in sRenderer)
        {
            if (s.gameObject.isStatic == statics && (1 << s.gameObject.layer & sortedLayers) != 0)
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
    #endregion

}

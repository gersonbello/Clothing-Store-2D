using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    #region Variables
    [Header("Mouse Interaction Configuration")]
    [Tooltip("Mouse cursor over this object")]
    public Texture2D cursorTextureOverObject;
    [Tooltip("Position of the cursor when over boject")]
    public Vector2 cursorOverHotspot = new Vector2(15, 15);
    [Tooltip("Mouse cursor when mouse down on this object")]
    public Texture2D cursorTextureMouseDownObject;
    [Tooltip("Position of the cursor when select object")]
    public Vector2 cursorOnMouseDownHotspot = new Vector2(15, 15);


    [Header("Object Events Configuration")]
    [Tooltip("Events when selected")]
    [SerializeField]
    protected UnityEvent OnClicked;
    [Tooltip("Offset to find path to object")]
    [SerializeField]
    protected Vector2 setPathOffset;
    #endregion

    private void Start()
    {
        if (cursorTextureOverObject == null) cursorTextureOverObject = Resources.Load<Texture2D>("Sprites/Cursors/HandCursor");
        if (cursorTextureMouseDownObject == null) cursorTextureMouseDownObject = Resources.Load<Texture2D>("Sprites/Cursors/HandClosedCursor");
    }

    #region Mouse Interactions
    private void OnMouseExit()
    {
        Cursor.SetCursor(GameController.gcInstance.baseCursorTexture, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTextureOverObject, cursorOverHotspot, CursorMode.Auto);
    }
    private void OnMouseDown()
    {
        if (GameController.gcInstance.IsPointerOverObject(Input.mousePosition)) return;
        Cursor.SetCursor(cursorTextureMouseDownObject, cursorOnMouseDownHotspot, CursorMode.Auto);
        GameController.gcInstance.StartCoroutine(GameController.gcInstance.RestoreCursorAfter(.1f, gameObject, cursorTextureOverObject, cursorOverHotspot));
        if (Vector2.Distance(GameController.gcInstance.playerBehaviour.transform.position, transform.position) > 2f)
        {
            GameController.gcInstance.playerBehaviour.SetPath(gameObject, (Vector2)transform.position + setPathOffset,OnClicked);
        }
        else OnClicked.Invoke();
    }
    private void OnMouseUp()
    {
        GameController.gcInstance.StopCoroutine("RestoreCursorAfter");
        Cursor.SetCursor(cursorTextureOverObject, cursorOverHotspot, CursorMode.Auto);
    }
    #endregion
}

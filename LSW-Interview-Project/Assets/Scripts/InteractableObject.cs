using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Mouse Interaction Configuration")]
    [Tooltip("Mouse cursor over this object")]
    [SerializeField]
    protected Texture2D cursorTextureOverObject;
    [Tooltip("Position of the cursor when over boject")]
    [SerializeField]
    protected Vector2 cursorOverHotspot = new Vector2(15, 15);
    [Tooltip("Mouse cursor when mouse down on this object")]
    [SerializeField]
    protected Texture2D cursorTextureMouseDownObject;
    [Tooltip("Position of the cursor when select object")]
    [SerializeField]
    protected Vector2 cursorOnMouseDownHotspot = new Vector2(15, 15);


    [Header("Object Events Configuration")]
    [Tooltip("Events when selected")]
    [SerializeField]
    protected UnityEvent OnClicked;

    private void Start()
    {
        if (cursorTextureOverObject == null) cursorTextureOverObject = Resources.Load<Texture2D>("Sprites/Cursors/HandCursor");
        if (cursorTextureMouseDownObject == null) cursorTextureMouseDownObject = Resources.Load<Texture2D>("Sprites/Cursors/HandClosedCursor");
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(GameController.gcInstance.baseCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTextureOverObject, cursorOverHotspot, CursorMode.ForceSoftware);
    }

    private void OnMouseDown()
    {
        Cursor.SetCursor(cursorTextureMouseDownObject, cursorOnMouseDownHotspot, CursorMode.ForceSoftware);
        GameController.gcInstance.StartCoroutine(GameController.gcInstance.RestoreCursorAfter(.1f, gameObject, cursorTextureOverObject, cursorOverHotspot));
        OnClicked.Invoke();
    }
}

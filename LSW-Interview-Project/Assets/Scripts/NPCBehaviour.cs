using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MoveableObjects
{
    #region Variables
    [Header("Dialogues")]
    [Tooltip("Dialogues to show")]
    [SerializeField]
    private List<Dialogue> dialogues;
    [Tooltip("Generic Dialogues to show")]
    [SerializeField]
    private List<Dialogue> genericDialogues;

    [Header("Movement")]
    [Tooltip("Max Movement Distance")]
    [SerializeField]
    private Vector2 maxAutoMoveDistance = new Vector2(10,10);
    #endregion

    #region Dialogue
    public void ShowDialogue()
    {
        GetObjectDirection(GameController.gcInstance.playerBehaviour.transform.position - transform.position);
        HandleAnimation(new Vector2());
        ChangeSkin();
        Dialogue dialogueToSent = new Dialogue();
        if (dialogues != null && dialogues.Count > 0)
        {
            dialogueToSent = dialogues[0];
            dialogues.Remove(dialogueToSent);
        }
        if ((dialogueToSent == null || dialogueToSent.dialogueSentences == null) && genericDialogues != null && genericDialogues.Count > 0)
            dialogueToSent = genericDialogues[Random.Range(0, genericDialogues.Count)];

        if (dialogueToSent.dialogueSentences == null) return;
        GameController.gcInstance.playerBehaviour.TurnOffInput();
        GameController.gcInstance.dialogueSystem.settedDialogue = dialogueToSent;
        GameController.gcInstance.dialogueSystem.gameObject.SetActive(true);
    }
    #endregion

    #region Change Player Status
    /// <summary>
    /// Turn off the player input
    /// </summary>
    public void TurnPlayerInputOff()
    {
        GameController.gcInstance.playerBehaviour.TurnOffInput();
    }
    /// <summary>
    /// Turn on the player input
    /// </summary>
    public void TurnPlayerInputOn()
    {
        GameController.gcInstance.playerBehaviour.TurnOnInput();
    }
    #endregion

    #region Movement
    [Header("Movement Options")]
    [Tooltip("Active auto random movement")]
    [SerializeField]
    private bool autoMove;

    private void Start()
    {
        StartCoroutine(MoveToRandomPosition(Random.Range(10, 30)));
    }

    private void Update()
    {
        if (autoMove &&
            targetWalkNode != null &&
            lastWalkNode != null &&
            (GameController.gcInstance.dialogueSystem == null ||
            GameController.gcInstance.dialogueSystem != null &&
            !GameController.gcInstance.dialogueSystem.gameObject.activeInHierarchy))
            AutoMove();
        else HandleAnimation(new Vector2());
    }
    public IEnumerator MoveToRandomPosition(float time)
    {
        yield return new WaitForSeconds(time);
        if (!autoMove)
        {
            StopAllCoroutines();
            yield break;
        }
        float x = Random.Range(-maxAutoMoveDistance.x, maxAutoMoveDistance.x);
        float y = Random.Range(-maxAutoMoveDistance.y, maxAutoMoveDistance.y);
        Vector2 newPosition = (Vector2)transform.position + new Vector2(x, y);
        if (ComparePositionToWorldBounds(newPosition))
            SetPath(newPosition, null);

        StartCoroutine(MoveToRandomPosition(Random.Range(10, 30)));
    }
    #endregion
}

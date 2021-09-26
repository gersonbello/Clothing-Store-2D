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
    #endregion

    #region Dialogue
    public void ShowDialogue()
    {
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
}

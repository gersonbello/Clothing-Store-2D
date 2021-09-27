using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

#region Dialogue Type Class
/// <summary>
/// Base Dialogue Class
/// </summary>
[Serializable]
public class Dialogue
{
    [Tooltip("The texts to be displayed in the dialogue")]
    public DialogueSentence[] dialogueSentences;
    [Tooltip("The events to invoke after dialogue")]
    public UnityEvent dialogueEvents;
    [Serializable]
    public class DialogueSentence
    {
        [Tooltip("The texts to be displayed in the dialogue")]
        [TextArea(2,5)]
        public string sentence;
        [Tooltip("The hat for the sentence")]
        public Sprite dialogueCharacterHat;
    }
}
#endregion

public class DialogueSystem : MonoBehaviour
{
    #region Variables
    [Header("Dialogue Box Configuration")]
    [Tooltip("Dialogue Text")]
    [SerializeField]
    private TextMeshProUGUI dialogueBoxText;
    [Tooltip("Dialogue Text")]
    [SerializeField]
    private Image charRepresentationHatRenderer;
    [Tooltip("Dialogue Text")]
    [SerializeField]
    private GameObject nextSentenceIndicator;

    [Header("Dialogue status")]
    [Tooltip("Current dialogue")]
    public Dialogue settedDialogue;
    [Tooltip("The current dialogue sentence")]
    [SerializeField]
    private int sentencesIndex;
    // the target sentence to show in the dialogue box
    private string targetSentence;
    #endregion

    #region Dialogue
    private void OnEnable()
    {
        nextSentenceIndicator.SetActive(false);
        sentencesIndex = 0;
        GameController.gcInstance.dialogueSystem = this;
        if (GameController.gcInstance.playerBehaviour != null) GameController.gcInstance.playerBehaviour.ResetMovement();
        GetComponent<CanvasGroup>().alpha = 1;
        StopAllCoroutines();
        if (settedDialogue == null) GetNextDialogue();
        else
        {
            if (settedDialogue.dialogueSentences != null && sentencesIndex < settedDialogue.dialogueSentences.Length)
                StartCoroutine(AnimateText());
            else gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Get the next dialogue sentence
    /// </summary>
    public void GetNextDialogue()
    {
        nextSentenceIndicator.SetActive(false);
        StopCoroutine(AnimateText());
        sentencesIndex++;
        if(settedDialogue == null || sentencesIndex >= settedDialogue.dialogueSentences.Length)
        {
            sentencesIndex = 0;
            if (settedDialogue != null ) settedDialogue.dialogueEvents.Invoke();
            settedDialogue = null;
            StartCoroutine(FadeDisable());
        }
        else
        {
            if(settedDialogue != null)
                StartCoroutine(AnimateText());
        }
    }

    /// <summary>
    /// Called when the mouse click on this UI object
    /// </summary>
    public void OnClick()
    {
        // Try to affect the dialogue system, in cases where the player quickly advance the text before the coroutine ends 
        // and the action ends with any errors, the action will be ignored an the system will be closed.
        try
        {
            if (settedDialogue.dialogueSentences == null) return;
            if (dialogueBoxText.text != settedDialogue.dialogueSentences[sentencesIndex].sentence)
            {
                StopCoroutine("AnimateText");
                dialogueBoxText.text = settedDialogue.dialogueSentences[sentencesIndex].sentence;
                nextSentenceIndicator.SetActive(true);
            }
            else GetNextDialogue();
        }
        catch
        {
            StopAllCoroutines();
            Debug.LogWarning("Something went wrong with the dialogue");
            if (settedDialogue != null) settedDialogue.dialogueEvents.Invoke();
            settedDialogue = null;
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Animation
    /// <summary>
    /// Animate the text letters
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateText()
    {
        targetSentence = settedDialogue.dialogueSentences[sentencesIndex].sentence;
        char[] charArray = targetSentence.ToCharArray();

        if (settedDialogue.dialogueSentences[sentencesIndex].dialogueCharacterHat != null)
            charRepresentationHatRenderer.sprite = settedDialogue.dialogueSentences[sentencesIndex].dialogueCharacterHat;
        dialogueBoxText.text = "";
        int charIndex = 0;

        while (settedDialogue != null &&
            targetSentence != null &&
            dialogueBoxText.text != targetSentence &&
            charIndex < charArray.Length)
        {
            if (charIndex >= targetSentence.Length ||
                charIndex > charArray.Length) break;
            dialogueBoxText.text += charArray[charIndex];
            charIndex++;
            yield return new WaitForSeconds(1.25f * Time.deltaTime);
        }

        dialogueBoxText.text = targetSentence;
        nextSentenceIndicator.SetActive(true);

    }

    /// <summary>
    /// Fade animation to canvas before desable
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeDisable()
    {
        StopCoroutine(AnimateText());
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0.001f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, 10f * Time.fixedDeltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    #endregion
}

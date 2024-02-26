using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue {
    public int speakerIndex;
    public int colorIndex;
    [TextArea(3, 10)]
    public string dialogueText;
    public UnityEvent dialogueEvent;
}
public class InteractableNPC : MonoBehaviour
{
    public Dialogue[] dialogues;
    private int dialogueIndex = 0;
    public bool isSpeaking = false;

    public string[] CharacterNames;
    public Color[] textColors;

    public Transform player;
    bool p_isInRange = false;
    bool isInRange = false;
    public float distance = 2f;

    void Start()
    {
        player = Player.Instance?.transform;
    }

    void Update()
    {
        if(player == null)return;

        if(isSpeaking)
        {
            if (PlayerInput.Instance.interactKey) {
                ContinueDialogue();
            }
            return;
        }
        isInRange = (player.position-transform.position).sqrMagnitude < distance * distance;

        if (isInRange && !p_isInRange) {
            SubtitleManager.Instance?.DisplaySubtitle("Press E to talk");
        } else if (!isInRange && p_isInRange) {
            SubtitleManager.Instance?.ClearSubtitle();
        }
        p_isInRange = isInRange;

        if (isInRange && PlayerInput.Instance.interactKey)
            StartDialogue();
    }

    public void StartDialogue() {
        if (dialogues.Length > 0) {
            // Display the first dialogue
            isSpeaking = true;
            PlayerInput.Instance.SetBlockedState(true);
            DisplayDialogue(dialogues[dialogueIndex]);
        }
    }

    public void ContinueDialogue() {
        dialogueIndex++;
        if (dialogueIndex < dialogues.Length) {
            // Display the next dialogue
            DisplayDialogue(dialogues[dialogueIndex]);
        } else {
            // End dialogue
            EndDialogue();
        }
    }

    private void DisplayDialogue(Dialogue dialogue)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(textColors[dialogue.colorIndex]);
        SubtitleManager.Instance?.DisplaySubtitle("<color=#" + colorHex + ">" + CharacterNames[dialogue.speakerIndex] + ":</color> " + dialogue.dialogueText);
        dialogue?.dialogueEvent?.Invoke();
    }

    private void EndDialogue() {
        Debug.Log("End of dialogue");
        SubtitleManager.Instance?.ClearSubtitle();
        dialogueIndex = 0;
        CancelInvoke("ResetSpeaking");
        Invoke("ResetSpeaking", 0.2f);
        PlayerInput.Instance.SetBlockedState(false);
    }
    void ResetSpeaking()
    {
        isSpeaking = false;
    }
}

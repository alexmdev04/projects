using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class uiMessage : MonoBehaviour
{
    public static uiMessage instance { get; private set; }
    TextMeshProUGUI textBox;
    public List<string> messages = new();
    public List<float> messageDurations = new();
    StringBuilder displayText;
    RectTransform rectTransform;
    float targetYPos;
    float startYPos;
    int messageCount;
    [SerializeField] float yPerMsg = 42.5f;
    [SerializeField] float animSpeed = 5f;
    void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        textBox = GetComponent<TextMeshProUGUI>();
        startYPos = rectTransform.position.y;
    }
    void Update()
    {
        displayText = new StringBuilder();
        messageCount = 0;
        foreach (string msg in messages)
        {
            // if a message has been nullified it is skipped 
            if (msg == string.Empty) { continue; } 
            messageCount++;
            displayText.Append(msg).Append("\n");
        }
        textBox.text = displayText.ToString();
        // if all messages are nullified, the lists are cleared
        if (textBox.text == string.Empty) { Clear(); } 

        // depending on the current number of messages,
        targetYPos = startYPos + (messageCount * yPerMsg);
        // the text box lerps up to make it look like the message is sliding up onto the screen
        rectTransform.position = new(rectTransform.position.x,
                                     Mathf.Lerp(rectTransform.position.y, targetYPos, animSpeed * Time.deltaTime),
                                     rectTransform.position.z);

    }
    /// <summary>
    /// Displays a new temporary uiMessage
    /// </summary>
    /// <param name="text"></param>
    /// <param name="duration"></param>
    public void New(string text, float duration = 3f)
    {
        messages.Add(text);
        messageDurations.Add(duration);
        StartCoroutine(RemoveAfter(messages.IndexOf(text), duration));
        //if (uiDebug.instance.debugMode) { Debug.Log(text); }
    }
    /// <summary>
    /// Waits until removing a uiMessage at a given index
    /// </summary>
    /// <param name="msgIndex"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator RemoveAfter(int msgIndex, float duration)
    {
        yield return new WaitForSeconds(duration);
        Remove(msgIndex);
    }
    /// <summary>
    /// Removes all uiMessages / Clears the list of messages
    /// </summary>
    public void Clear()
    {
        messages.Clear();
        messageDurations.Clear();
    }
    /// <summary>
    /// <para>Instantly removes a uiMessage at a given index</para>
    /// <para>This only nullifies the message, it does not delete it from the list of messages</para>
    /// </summary>
    /// <param name="msgIndex"></param>
    public void Remove(int msgIndex)
    {
        messages[msgIndex] = string.Empty;
        messageDurations[msgIndex] = 0;
    }
}
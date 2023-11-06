using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class uiDebugConsole : MonoBehaviour
{
    public static uiDebugConsole instance { get; private set; }
    TMP_InputField inputField;
    [SerializeField] bool outputCommandInputs;
    int previousInputsCurrentIndex = 0;
    string previousInputTemporary;
    List<string> previousInputs = new() { string.Empty };
    string command;
    string data1;
    struct commandInputs
    {
        public string command;
        public string data1;
        //public string data2;
        //public string data3;
    }
    List<string> levels = new()
    {
        "Level0"
    };
    void Awake()
    {
        instance = this;
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((string playerInput) => { CheckCommand(playerInput); });
        gameObject.SetActive(false);
    }
    void Update()
    {
        inputField.ActivateInputField();
        if (previousInputsCurrentIndex == 0)
        {
            previousInputTemporary = inputField.text;
        }
        if (previousInputs.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && previousInputsCurrentIndex < previousInputs.Count - 1)
            {
                previousInputsCurrentIndex++;
                inputField.text = previousInputs[previousInputsCurrentIndex];
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && previousInputsCurrentIndex > 0)
            {
                previousInputsCurrentIndex--;
                inputField.text = (previousInputsCurrentIndex == 0) ? inputField.text = previousInputTemporary : previousInputTemporary;
            }
        }
    }
    void OnDisable()
    {
        if (inputField.text == "`") { inputField.text = string.Empty; }
    }
    void CheckCommand(string playerInput)
    {
        previousInputs.Add(playerInput);
        commandInputs parsedInput = ParseInput(playerInput);
        command = parsedInput.command;
        data1 = parsedInput.data1;
        if (outputCommandInputs)
        {
            Debug.Log("command = \"" + parsedInput.command + "\", " +
                      "input = \"" + parsedInput.data1 + "\", ");
                      //"input2 = \"" + commandInputs.input2 + "\", " +
                      //"input3 = \"" + commandInputs.input3 + "\"");
        }

        switch (parsedInput.command)
        {
            case "level":
                {
                    foreach (string level in levels)
                    {
                        if (parsedInput.data1 == level)
                        {
                            LevelLoader.instance.ChangeLevel(parsedInput.data1);
                            break;
                        }
                        else
                        {
                            uiMessage.instance.New("Unknown level: \"" + parsedInput.data1 + "\"");
                        }
                    }
                    break;
                }
            case "grapple":
                {
                    switch (parsedInput.data1)
                    {
                        case "enable":
                            {
                                Grapple.instance.Enable();
                                break;
                            }
                        case "disable":
                            {
                                Grapple.instance.Disable();
                                break;
                            }
                        default:
                            {
                                InvalidInput();
                                break;
                            }
                    }
                    break;
                }
            case "god":
                {
                    uiDebug.instance.ToggleGod();
                    break;
                }
            case "godmode":
                {
                    uiDebug.instance.ToggleGod();
                    break;
                }
            case "ufo":
                {
                    uiDebug.instance.ToggleNoclip();
                    break;
                }
            case "noclip":
                {
                    uiDebug.instance.ToggleNoclip();
                    break;
                }
            case "":
                {
                    Debug.LogError("No command entered");
                    break;
                }
            default:
                {
                    InvalidCommand();
                    break;
                }
        }
        inputField.text = "";
        gameObject.SetActive(false);
    }
    commandInputs ParseInput(string input)
    {
        commandInputs returnInputs = new commandInputs();
        char[] chars = input.ToCharArray();
        List<int> spaceIndexes = new();

        // getting indexes of the spaces in the input text
        for (int i = 0; i < chars.Length; i++) { if (chars[i] == ' ') { spaceIndexes.Add(i); } }
        // if there are no spaces then return the input in full
        if (spaceIndexes.Count == 0)
        {
            if (outputCommandInputs) { Debug.Log("only command no inputs"); }
            returnInputs.command = input;
            return returnInputs;
        }
        // if the first letter is a space and there is more than 1 letter then remove the space (if there is only a space then it will just be an unrecognised command)
        if (spaceIndexes[0] == 0 && chars.Length >= 1) { chars = chars[1..]; }
        // if the first space index is the final character then ignore it and output the command
        if (chars[^1] == spaceIndexes[0])
        {
            if (outputCommandInputs) { Debug.Log("only command no inputs, space at the end ignored"); }
            returnInputs.command = chars[..^1].ArrayToString();
            return returnInputs;
        }
        // add all characters of the input to the command up until the first space
        returnInputs.command = chars[..spaceIndexes[0]].ArrayToString();
        returnInputs.data1 = chars[(spaceIndexes[0] + 1)..].ArrayToString();

        //if (chars[^1] == ' ') 
        //{ 
        //    chars = chars[..^1];
        //}

        //if (spaceIndexes.Count == 2)
        //{
        //    // if the second space index is the final character then ignore it and output the command
        //    if (chars[^1] == spaceIndexes[1])
        //    {
        //        returnInputs.input1 = chars[(spaceIndexes[0] + 1)..spaceIndexes[1]].ArrayToString();
        //        return returnInputs;
        //    }
        //    for (int i = spaceIndexes[0]; i < spaceIndexes[1]; i++)
        //    {
        //        returnInputs.input1 += chars[i];
        //    }
        //}
        //else
        //{
            
        //}

        return returnInputs;
    }
    void InvalidCommand()
    {
        uiMessage.instance.New("Invalid Command: " + command);
    }
    void InvalidInput()
    {
        uiMessage.instance.New("Invalid input \"" + data1 + "\" for \"" + command + "\" command");
    }
}
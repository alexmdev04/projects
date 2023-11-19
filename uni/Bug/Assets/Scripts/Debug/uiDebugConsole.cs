using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using System;

public class uiDebugConsole : MonoBehaviour
{
    public static uiDebugConsole instance { get; private set; }
    TMP_InputField inputField;
    [SerializeField] bool outputCommandInputs;
    public int previousInputsCurrentIndex = 0;
    public List<string> previousInputs = new() { string.Empty };
    string command;
    string data1;
    struct commandInputs
    {
        public string command;
        public string data1;
        //public string data2;
        //public string data3;
    }
    string[] levels = new string[2]
    {
        "Level0",
        "Level1"
    };
    void Awake()
    {
        Vector3 newVector = new Vector3(x: 1, y: 2, z: 3);
        instance = this;
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((string playerInput) => CheckCommand(playerInput) );
        gameObject.SetActive(false);
    }
    void Update()
    {
        inputField.ActivateInputField();
        PreviousInput();
    }
    void OnDisable()
    {
        previousInputs[0] = string.Empty;
        inputField.text = string.Empty;
    }
    void CheckCommand(string playerInput)
    {
        string outputMsg = string.Empty;
        previousInputs.Insert(1, playerInput);
        commandInputs parsedInput = ParseInput(playerInput);
        command = parsedInput.command.ToLower();
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
                            outputMsg = "Unknown level: \"" + parsedInput.data1 + "\"";
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
                        case "testcube":
                            {
                                Grapple.instance.grappleDestinationMarker.SetActive(Grapple.instance.grappleDestinationMarker.activeSelf);
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
            case "menu":
                {
                    LevelLoader.instance.UnloadLevelCurrent();
                    break;
                }
            case "player":
                {
                    switch (parsedInput.data1)
                    {
                        case "position":
                            {
                                //Player.instance.debugSetPosition(parsedInput.data2);
                                outputMsg = "Not Implemented";
                                break;
                            }
                        case "position reset":
                            {
                                outputMsg = "Player position has been reset";
                                break;
                            }
                    }
                    break;
                }
            case "tutorial":
                {
                    LevelLoader.instance.UnloadLevelCurrent();
                    LevelLoader.instance.menuLevel.GetComponent<uiMenuLevel>().ForceRestartTutorial();
                    break;
                }
            case "":
                {
                    break;
                }
            default:
                {
                    InvalidCommand();
                    break;
                }
        }
        inputField.text = "";
        if (outputMsg != string.Empty)
        {
            uiMessage.instance.New(outputMsg);
            if (uiDebug.instance.debugMode) { Debug.Log(outputMsg); }
        }
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
    void PreviousInput()
    {
        if (previousInputsCurrentIndex == 0) { previousInputs[0] = inputField.text; }
        if (previousInputs.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && previousInputsCurrentIndex < previousInputs.Count - 1)
            {
                previousInputsCurrentIndex++;
                inputField.text = previousInputs[previousInputsCurrentIndex];
                char[] inputFieldChars = inputField.text.ToCharArray();
                inputField.stringPosition = inputFieldChars.ToList().IndexOf(inputFieldChars[^1]) + 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && previousInputsCurrentIndex > 0)
            {
                previousInputsCurrentIndex--;
                inputField.text = previousInputs[previousInputsCurrentIndex];
            }
        }
    }
    public void InternalCommandCall(string input)
    {
        CheckCommand(input);
    }
}
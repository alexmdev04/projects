using UnityEngine;
using TMPro;

public class buttonLogicExample : MonoBehaviour
{
    // A simple set of logic for doing different actions on tapping, holding and letting go of a unity button,

    // this contains an example use of this which slowly changes a text color from pink to red as the button is held, 
    // then changes to green when it has been held for the given time

    // add the Event Trigger Component to your button
    // add two EventTypes, PointerDown and PointerUp
    // reference an object that has this script attached to it
    // set the fucntion to e.g. buttonLogic/bool buttonDown on both EventTypes
    // under the function, check the box inside PointerDown
    // make sure thet PointerUp is unchecked

    public static bool buttonDown { get; set; }
    [SerializeField] private TextMeshProUGUI buttonText;
    private float waitFor = 2f; // this is how long the button must be held for in seconds
    private float waitedFor = 0f;
    private bool repeatLock;

    void Update()
    {
        buttons();
    }

    void buttons() 
    {// this must be running for as long as the button needs to function
        if (buttonDown)
        { // button held
            if (waitedFor >= waitFor && !repeatLock)
            { // after waiting
                buttonText.color = Color.green;
                repeatLock = true;
            }
            else if (waitedFor < waitFor)
            { // while waiting
                waitedFor += Time.deltaTime;
                buttonText.color = new(1, 0, 1 - waitedFor/waitFor);
            }
        }
        else
        { // let go of button
            if (waitedFor > 0 && waitedFor < 0.5f)
            { // held for less than 0.5s a.k.a. a tap
                waitedFor = 0;
                buttonText.color = new(1, 0, 1);
            }
            else
            { // idle / not held
              // repeats while the player is idle
                repeatLock = false;
                waitedFor = 0f;
                buttonText.color = new(1, 0, 1);
            }
        }
    }
}

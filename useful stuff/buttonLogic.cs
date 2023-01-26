using UnityEngine;

public class buttonLogic : MonoBehaviour
{
    // add Event Trigger Component to your button
    // add PointerDown and PointerUp, set their fucntions to bool buttonDown
    // PointerDown sets true, PointerUp sets to false

    public static bool buttonDown { get; set; }
    private float waitFor = 2f; // button hold time
    private float waitedFor = 0f;
    private bool repeatLock;

    void Update()
    {
        buttons();
    }

    void buttons() 
    { // must be in update
        if (buttonDown)
        { // button held
            if (waitedFor >= waitFor && !repeatLock)
            { // after waiting
                // CODE HERE
                repeatLock = true;
            }
            else if (waitedFor < waitFor)
            { // while waiting
                waitedFor += Time.deltaTime;
                // CODE HERE
            }
        }
        else
        { // let go of button
            if (waitedFor > 0 && waitedFor < 0.5f)
            { // held for less than 0.5s a.k.a. a tap
                waitedFor = 0;
                // CODE HERE
            }
            else
            { // idle / not held
                // repeats while the player is idle
                repeatLock = false;
                waitedFor = 0f;
                // CODE HERE
            }
        }
    }
}

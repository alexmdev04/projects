using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiMenuLevel : MonoBehaviour
{
    public static uiMenuLevel instance { get; private set; }
    [SerializeField] GameObject uiWM;
    public bool skipTutorial;
    int tutorialPlayerMoved;
    [SerializeField] float tutorialReminderClock = 5f;
    [SerializeField] float tutorialTextWaitTime = 3.25f;
    RaycastHit hit;
    [SerializeField] LayerMask layerMask;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End) && !skipTutorial) { SkipTutorials(); }
    }

    public IEnumerator TutorialStart()
    {
        while (!skipTutorial)
        {
            uiMessage.instance.New("Tutorial Start");
            uiWM.SetActive(false);
            Grapple.instance.SetMovementActive(false);

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("Press End to skip this tutorial");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("Welcome to Project Bug!");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("A game about grappling and sneaking towards a goal.");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            StartCoroutine(TutorialLook());

            break;
        }
    }
    IEnumerator TutorialLook()
    {
        while (!skipTutorial)
        {
            uiMessage.instance.New("First, let's learn how to look.");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("Move your mouse to look around");

            float mouseRotationThreshhold = 5000f;
            float reminderClockMouse = 0f;
            do
            {
                if (reminderClockMouse > tutorialReminderClock)
                {
                    uiMessage.instance.New("Move your mouse to look around");
                    reminderClockMouse = 0f;
                }
                float mouseRotationMean = Mathf.Abs((Player.instance.mouseRotation.x + Player.instance.mouseRotation.y) / 2);
                mouseRotationThreshhold -= mouseRotationMean;
                reminderClockMouse += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            while (mouseRotationThreshhold > 0);
            uiMessage.instance.New("Nice, now you know how to look!");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            StartCoroutine(TutorialGrappleDestination());

            break;
        }
    }
    IEnumerator TutorialGrappleDestination()
    {
        while (!skipTutorial)
        {
            uiMessage.instance.New("Next, lets learn how to grapple.");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("To choose your destination, look at a wall and hold left click.");
            float grappleHoldThreshhold = 0f;
            float reminderClockGrappleDestination = 0f;
            do
            {
                if (reminderClockGrappleDestination > tutorialReminderClock)
                {
                    uiMessage.instance.New("Look at a wall and hold left click.");
                    reminderClockGrappleDestination = 0f;
                }
                if (InputHandler.instance.input.Player.Grapple.IsPressed())
                {
                    grappleHoldThreshhold += Time.deltaTime;
                }
                reminderClockGrappleDestination += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            while (grappleHoldThreshhold < 2f);
            uiMessage.instance.New("See the sphere that appears?");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("That is where you will go when you release the grapple button.");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("It's colour indicates if the destination is valid.");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            StartCoroutine(TutorialGrappleMovement());

            break;
        }
    }

    IEnumerator TutorialGrappleMovement()
    {
        while (!skipTutorial)
        {
            uiMessage.instance.New("Now let's try and move around the room!");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("Use what you've learned to move 3 times.");

            Grapple.instance.SetMovementActive(true);
            Grapple.instance.grappleFinished.AddListener(TutorialPlayerMoved);

            float reminderClockMovement = 0f;
            do
            {
                if (reminderClockMovement > tutorialReminderClock)
                {
                    uiMessage.instance.New("Look at a wall, hold left click, then release to move.");
                    reminderClockMovement = 0f;
                }
                reminderClockMovement += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            while (tutorialPlayerMoved < 3);
            Grapple.instance.grappleFinished.RemoveListener(TutorialPlayerMoved);
            uiMessage.instance.New("Good job!");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            uiMessage.instance.New("Now you can freely move around the game!");

            yield return new WaitForSeconds(tutorialTextWaitTime);
            StartCoroutine(TutorialRadar());

            break;
        }
    }
    void TutorialPlayerMoved()
    {
        tutorialPlayerMoved++;
    }
    IEnumerator TutorialRadar()
    {
        while (!skipTutorial)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(TutorialFinished());

            break;
        }
    }
    IEnumerator TutorialFinished()
    {
        yield return new WaitForEndOfFrame();
        uiMessage.instance.New("end");
        Grapple.instance.SetMovementActive(true);
        uiWM.SetActive(true);
    }
    public void ForceRestartTutorial()
    {
        StartCoroutine(TutorialStart());
    }
    void SkipTutorials()
    {
        skipTutorial = true;
        StartCoroutine(TutorialFinished());
        uiMessage.instance.New("Tutorials Skipped!");
    }
}
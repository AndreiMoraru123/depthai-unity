using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class HandGestureData
{
    public int RecognizedNumber { get; set; }
}


public class Keypad : Interactable
{

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private int requiredNumber;

    private KeypadHandTracker handTracker;

    private bool doorOpen;

    // Start is called before the first frame update
    void Start()
    {
        if (handTracker == null) handTracker = GetComponent<KeypadHandTracker>();
        if (handTracker == null) Debug.LogError("missing KeypadHandTracker component.");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override bool ValidateInteraction(object interactionData)
    {
        // if (interactionData == null) return false;
        // HandGestureData gestureData = (HandGestureData)interactionData;
        // gestureData.RecognizedNumber = 5;
        var number = handTracker.HandleGesture();
        Debug.Log(number.ToString());
        HandGestureData gestureData = new HandGestureData { RecognizedNumber = number };
        return gestureData.RecognizedNumber == requiredNumber;
    }

    protected override void Interact(object interactionData = null)
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}

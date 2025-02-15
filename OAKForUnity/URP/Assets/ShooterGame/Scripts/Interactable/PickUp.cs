
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PickUp : Interactable
{
    [SerializeField]
    private string requiredGesture;

    [SerializeField]
    private float sequenceTimeout = 3.5f; // time allowed between gestures
    private HandTrackingManager handTracker;
    private string shownGesture;
    private float lastGestureTime;

    // Start is called before the first frame update
    void Start()
    {
        if (handTracker == null) handTracker = GetComponent<HandTrackingManager>();
        if (handTracker == null) Debug.LogError("missing HandTracker component.");
        UpdatePromptMessage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastGestureTime > sequenceTimeout)
        {
            ResetSequence();
        }

        if (handTracker.CurrentGesture != -1)
        {
            UpdatePromptMessage(true);
        }

        if (handTracker.TryGetStableGesture(out int gesture))
        {
            HandleNewGesture(gesture);
        }
    }

    private void UpdatePromptMessage(bool showProgress = false)
    {
        var message = $"Pick up {gameObject.name}: <color=red>{requiredGesture}</color>";
        if (showProgress && handTracker.GetGestureFromNumber(handTracker.CurrentGesture) == requiredGesture)
        {
            var progress = handTracker.StabilityProgress;
            var colorHex = ColorUtility.ToHtmlStringRGB(Color.Lerp(Color.yellow, Color.green, progress));
            message = $"Pick up {gameObject.name}: <color=#{colorHex}>{requiredGesture}</color>";
        }
        promptMessage = message;
    }

    public void HandleNewGesture(int gesture)
    {
        switch (gesture)
        {
            case 0:
                shownGesture = "FIST";
                lastGestureTime = Time.time;
                break;
            case 5:
                shownGesture = "PALM";
                lastGestureTime = Time.time;
                break;
            default:
                return;
        }

        var isValid = true;
        if (shownGesture != requiredGesture)
        {
            isValid = false;
        }

        if (!isValid)
        {
            ResetSequence();
            return;
        }

        UpdatePromptMessage();
        Interact();
        ResetSequence();
    }

    private void ResetSequence()
    {
        shownGesture = null;
        lastGestureTime = 0f;
        UpdatePromptMessage();
    }

    public override bool ValidateInteraction()
    {
        string gesture;
        if (handTracker != null)
        {
            var number = handTracker.CountFingersOnHand();
            gesture = handTracker.GetGestureFromNumber(number);
        }
        else
        {
            gesture = "INVALID";
        }
        return gesture == requiredGesture;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class SceneController : MonoBehaviour {
    public Camera FirstPersonCamera;

    public ARKeyboard keyboard;

    private bool quitting = false;

    public void Awake() {
        Application.targetFrameRate = 60;
    }

    private void Update() {
        UpdateLifeCycle();

        ProcessTouches();
    }

    private void UpdateLifeCycle() {
        CheckForBackPress();

        ForceScreenAwake();

        if (quitting) {
            return;
        }

        CheckForErrors();
    }

    private void CheckForBackPress() {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void ForceScreenAwake() {
        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking) {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    private void CheckForErrors() {
        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted) {
            ShowAndroidToastMessage("Camera permission is needed to run this application.");
            quitting = true;
            Invoke("Quit", 0.5f);
        }
        else if (Session.Status.IsError()) {
            ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            quitting = true;
            Invoke("Quit", 0.5f);
        }
    }

    private void Quit() {
        Application.Quit();
    }

    private void ShowAndroidToastMessage(string message) {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null) {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    void ProcessTouches() {
        Touch touch;
        if (Input.touchCount != 1 ||
            (touch = Input.GetTouch(0)).phase != TouchPhase.Began) {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit)) {
            SetSelectedPlane(hit.Trackable as DetectedPlane);
        }
    }

    void SetSelectedPlane(DetectedPlane selectedPlane) {
        Debug.Log("Selected plane centered at " + selectedPlane.CenterPose.position);

        keyboard.SetPlane(selectedPlane);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

using GoogleARCore.Examples.Common;

public class ARWorld : MonoBehaviour
{
    [SerializeField]
    private GameObject ARCoreDevice;
    [SerializeField]
    private Camera firstPersonCamera;
    public DetectedPlane detectedPlane;
    private bool planeSet = false;

    public float scale;
    // Update is called once per frame
    void Update()
    {
        if (Application.isMobilePlatform && planeSet == false)
        {
            //session must be tracking in order access the frame
            if (Session.Status != SessionStatus.Tracking)
            {
                int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
                return;
            }
            //if we are tracking check if screen is touched
            if (!planeSet)
            {
                ProcessTouches();
            }
            else
            {
                if (GameObject.FindGameObjectsWithTag("Plane").Length > 0)
                {
                    OnTogglePlanes(false);
                }
            }
        }
    }

        //This will detect if user touches the screen.
        //if so then cast ray from camera to the touched position and check if it hits a ARCore detected plane
        private void ProcessTouches()
        {
            Touch touch;
            if ((Input.touchCount != 1) || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                SetSelectedPlane(hit.Trackable as DetectedPlane);
            }
        }

        //Used to test if raycast is working as intended
        private void SetSelectedPlane(DetectedPlane selectedPlane)
        {
            if (selectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
            {
                Debug.Log("selected plane at " + selectedPlane.CenterPose.position);
                detectedPlane = selectedPlane;

                if (detectedPlane.ExtentX <= detectedPlane.ExtentZ)
                {
                    scale = detectedPlane.ExtentX;
                }
                else
                {
                    scale = detectedPlane.ExtentZ;
                }
                this.gameObject.transform.localScale *= scale;
                this.gameObject.transform.position = detectedPlane.CenterPose.position;

                firstPersonCamera.GetComponentInParent<ARCoreSession>().SessionConfig.PlaneFindingMode = GoogleARCore.DetectedPlaneFindingMode.Disabled;
                OnTogglePlanes(false);
                planeSet = true;
            }
            else
            {
                return;
            }
        }


        // Taken Directly from ARCore HELLOAR example (I understand what this does but not how it works !! need to learn)
        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        public void OnTogglePlanes(bool flag)
        {
            foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Plane"))
            {
                Renderer r = plane.GetComponent<Renderer>();
                DetectedPlaneVisualizer t = plane.GetComponent<DetectedPlaneVisualizer>();
                r.enabled = flag;
                t.enabled = flag;
            }
        }
}

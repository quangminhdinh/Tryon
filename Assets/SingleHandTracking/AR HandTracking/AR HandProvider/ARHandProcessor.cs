using MediapipeHandTracking;
using UnityEngine;
using System;

public class ARHandProcessor : MonoBehaviour {
    private GameObject Hand = default;
    private HandRect currentHandRect = default;
    private HandRect oldHandRect = default;
    private ARHand currentHand = default;
    private bool isHandRectChange = default;

    public static int[] ringPos = {14};

    void Start() {
        Hand = Manager.instance.HandOnSpace;
        currentHand = new ARHand();
        currentHandRect = new HandRect();
        oldHandRect = new HandRect();
        // foreach (int i in ringPos) {
        //     Hand.transform.GetChild(i).gameObject.SetActive(true);
        // }
    }

    // void OnGUI() {
    //     int w = Screen.width, h = Screen.height;
    //     GUIStyle style = new GUIStyle();
    //     Rect rect = new Rect(0, 0, w, h * 2 / 100);
    //     // Rect rect1 = new Rect(0, rect.height + 2, w, h * 2 / 100);
    //     // Rect rect2 = new Rect(0, rect1.yMax + 2, w, h * 2 / 100);
    //     style.alignment = TextAnchor.UpperLeft;
    //     style.fontSize = h * 2 / 100;
    //     style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
    //     // float msec = deltaTime * 1000.0f;
    //     // fps = 1.0f / deltaTime;
    //     string text = "Width: " + currentHandRect.Width + "\nHeight: " + currentHandRect.Height + "\nXCenter: " + currentHandRect.XCenter + "\nYCenter: " + currentHandRect.YCenter + "\nRotation: " + currentHandRect.Rotaion;
    //     GUI.Label(rect, text, style);
    //     // GUI.Label(rect1, "Hand: " + handProcessor.CurrentHand.GetLandmark(0), style);
    //     // GUI.Label(rect2, "Hit: " + hitest.LazeOnSpace.Tail, style);
    // }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate() {
        if (GetComponent<ARFrameProcessor>().HandProcessor == null) return;
        float[] handRectData = GetComponent<ARFrameProcessor>().HandProcessor.getHandRectData();
        float[] handLandmarksData = GetComponent<ARFrameProcessor>().HandProcessor.getHandLandmarksData();

        if (null != handRectData) {
            currentHandRect = HandRect.ParseFrom(handRectData);
            // Console.log("Width: " + currentHandRect.Width + "\nHeight: " + currentHandRect.Height + "\nXCenter: " + currentHandRect.XCenter + "\nYCenter: " + currentHandRect.YCenter + "\nRotation: " + currentHandRect.Rotation);
            if (!isHandStay()) {
                oldHandRect = currentHandRect;
                isHandRectChange = true;
            } else {
                isHandRectChange = false;
            }
        }

        if (null != handLandmarksData && !float.IsNegativeInfinity(GetComponent<ARFrameProcessor>().ImageRatio)) {
            currentHand.ParseFrom(handLandmarksData, GetComponent<ARFrameProcessor>().ImageRatio);
        }
        long time = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        Debug.Log("Rendered at: " + time.ToString());

        if (!Hand.activeInHierarchy) return;
        for (int i = 0; i < ringPos.Length; i ++) {
            Hand.transform.GetChild(i).transform.position = currentHand.GetLandmark(i);
            Hand.transform.GetChild(i).transform.LookAt(Camera.main.transform);
            Hand.transform.GetChild(i).transform.Rotate( 0, 180, 0 ) ;
            // if (Hand.transform.GetChild(i).childCount > 0) {
            //     Hand.transform.GetChild(i).GetComponent<TextMesh>().text = i.ToString();
            // }
        }
    }

    private bool isHandStay() {
        return currentHandRect.XCenter == oldHandRect.XCenter &&
            currentHandRect.YCenter == oldHandRect.YCenter &&
            currentHandRect.Width == oldHandRect.Width &&
            currentHandRect.Height == oldHandRect.Height &&
            currentHandRect.Rotaion == oldHandRect.Rotaion;
    }

    public ARHand CurrentHand { get => currentHand; }
    public bool IsHandRectChange { get => isHandRectChange; }
    public HandRect CurrentHandRect { get => currentHandRect; }
}
using MediapipeHandTracking;
using UnityEngine;

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

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate() {
        if (GetComponent<ARFrameProcessor>().HandProcessor == null) return;
        float[] handRectData = GetComponent<ARFrameProcessor>().HandProcessor.getHandRectData();
        float[] handLandmarksData = GetComponent<ARFrameProcessor>().HandProcessor.getHandLandmarksData();

        if (null != handRectData) {
            currentHandRect = HandRect.ParseFrom(handRectData);
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
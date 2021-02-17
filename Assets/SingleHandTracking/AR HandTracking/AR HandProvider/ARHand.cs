using UnityEngine;
using MediapipeHandTracking;

public class ARHand {
    private Vector3[] landmarks, landmarksCP = default;
    public float currentDepth = 0f;
    private Camera cam;

    private static int LANDMARKS_NUM = 1;

    public ARHand() {
        landmarks = new Vector3[LANDMARKS_NUM];
        cam = Camera.main;
    }

    private ARHand(Vector3[] landmarks) {
        this.landmarks = landmarks;
    }

    public void ParseFrom(float[] arr, float c) {
        if (null == arr || arr.Length < 63) return;

        float xScreen = Screen.width * ((arr[ARHandProcessor.UPPER_JOINT_POS * 3 + 1] - 0.5f * (1 - c)) / c);
        float yScreen = Screen.height * (arr[ARHandProcessor.UPPER_JOINT_POS * 3]);
        this.landmarks[0] = cam.ScreenToWorldPoint(new Vector3(xScreen, yScreen, arr[ARHandProcessor.UPPER_JOINT_POS * 3 + 2] / 80 + 0.4f));
        // this.landmarks[i] = cam.ScreenToWorldPoint(new Vector3(500, 500, 0.3f));

        if (landmarksCP == default) {
            landmarksCP = new Vector3[LANDMARKS_NUM];
            landmarksCP = (Vector3[])landmarks.Clone();
        } else {
            // nễu bị rung giữ nguyên landmark cũ
            if (isVibrate(0.02f)) {
                landmarks = (Vector3[])landmarksCP.Clone();
            } else { // lưu lại landmark khi không bị rung
                landmarksCP = (Vector3[])landmarks.Clone();
            }
        }
    }

    public bool isVibrate(float deltaVibrate) {
        if (Vector3.Distance(landmarksCP[0], landmarks[0]) > deltaVibrate) return false;
        return true;
    }

    public Vector3 GetLandmark(int index) => this.landmarks[index];
    public Vector3[] GetLandmarks() => this.landmarks;
    public ARHand Clone() {
        return new ARHand((Vector3[])landmarks.Clone());
    }

}
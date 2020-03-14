using UnityEngine;

using GoogleARCore;

public class ARKeyboard : MonoBehaviour {
    public Camera firstPersonCamera;
    private DetectedPlane detectedPlane;

    public GameObject keyboardPrefab;
    private GameObject keyboardInstance;

    public void Update() {
        if (Input.touchCount > 0) {
            RaycastHit hit;

            Touch t = Input.touches[0];

            if(t.phase != TouchPhase.Began) {
                return;
            }

            Ray ray = firstPersonCamera.ScreenPointToRay(t.position);

            if (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)) {
                if (hit.collider.CompareTag("key")) {
                    Key k = hit.collider.gameObject.GetComponent<Key>();
                    k.TouchKey();
                }
            }
        }
    }

    public void SetPlane(DetectedPlane plane) {
        detectedPlane = plane;

        SpawnKeyboard();
    }

    void SpawnKeyboard() {
        if (keyboardInstance != null) {
            return;
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        keyboardInstance = Instantiate(keyboardPrefab, pos, Quaternion.identity, transform);
    }
}
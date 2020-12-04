using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FaceCamera : MonoBehaviour
{

    private Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);
        transform.Rotate( 0, 180, 0 ) ;
        GetComponent<TextMeshPro>().text = Vector3.Distance(transform.position, camera.position).ToString("F2");
    }
}

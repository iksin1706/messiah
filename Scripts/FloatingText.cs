using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingText : MonoBehaviour
{
    public Camera mainCamera;
    public string text;
    TextMesh textMesh;
    // Start is called before the first frame update

    void Awake()
    {
        textMesh = transform.Find("Text").GetComponent<TextMesh>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    void OnLevelWasLoaded(int level)
    {
        textMesh = transform.Find("Text").GetComponent<TextMesh>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void SetMainCamera(Camera cam)
    {
        mainCamera = cam;
    }

    // Update is called once per frame
    public void Update()
    {
        textMesh.text = text;
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}

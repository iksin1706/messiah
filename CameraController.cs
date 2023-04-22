using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Range(1, 10)]
    public float CameraFollowSpeed = 1f;
    public Vector3 OffSet;
    public float MinZoom = 0.5f;
    public float MaxZoom = 1.5f;
    public GameObject Player;

    private float cameraZoom = 1;
    private List<GameObject> gameObjects;
    BoxCollider boxCollider;
    private float currentYaw;
    CinemachineVirtualCamera cm;
    CinemachineOrbitalTransposer cmt;


    private void OnLevelWasLoaded(int level)
    {
       // if (SceneManager.GetActiveScene().buildIndex == 2) Player = GameObject.Find("Jesus");

    }

    private void Awake()
    {
        OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
    }


    private void Start()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
        cmt = cm.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        boxCollider = GetComponent<BoxCollider>();
        gameObjects = new List<GameObject>();

        transform.position =Player.transform.position + OffSet * cameraZoom;

    }
    void Update()
    {
        if (Player != null)
        {
            
            Zoom();

            boxCollider.size = new Vector3(0.2f, 0.2f, Vector3.Distance(transform.position, Player.transform.position) * 1.8f);
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                Player = GameObject.Find("Jesus");
                transform.position = Player.transform.position + OffSet * cameraZoom;
            }

        }
    }
    private void OnDisable()
    {
        foreach (GameObject obj in gameObjects) {
            ShowVeilingObject(obj.gameObject);
        }
    }
    private void LateUpdate()
    {
        CameraFollow();
        //transform.RotateAround(Player.transform.position, Vector3.up, currentYaw);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enviroment"))
            HideVeilingObject(other.gameObject);

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enviroment"))
         ShowVeilingObject(other.gameObject);
    }
    void ShowVeilingObject(GameObject go)
    {
        if (go != null&&go.CompareTag("Enviroment"))
        {
          //  for (int i = 0; i < gameObjects.Count; i++)
            //{

               // go.layer = LayerMask.NameToLayer("Default");
                Renderer rend = go.transform.GetComponent<Renderer>();
                for (int j = 0; j < rend.materials.Length; j++)
                {
                    if (rend.materials[j].name == "TreeLeaf_1 (Instance)") rend.materials[j].shader = Shader.Find("Polygon Wind/Tree");
                    else
                        rend.materials[j].shader = Shader.Find("Standard");
                if (rend.materials[j]?.color != null)
                {
                    Color tempColor = rend.materials[j].color;
                    tempColor.a = 1f;
                    rend.materials[j].color = tempColor;
                }
                }

           // }

        }
        gameObjects = new List<GameObject>();
    }


    void HideVeilingObject(GameObject go)
    {
        if (go.CompareTag("Enviroment"))
        {
            Renderer rend = go.transform.GetComponent<Renderer>();
            go.layer = LayerMask.NameToLayer("Ignore Raycast");
            gameObjects.Add(go);
            if (rend)
            {
                for (int j = 0; j < rend.materials.Length; j++)
                {
                    if (rend.materials[j].shader == Shader.Find("Polygon Wind/Tree"))
                    {
                        rend.materials[j].shader = Shader.Find("Transparent/Diffuse");
                        Color tempColor = rend.materials[j].color;
                        tempColor = new Color32(118, 120, 80, 122);
                        rend.materials[j].color = tempColor;


                    }
                    else
                    {
                        rend.materials[j].shader = Shader.Find("Transparent/Diffuse");
                        Color tempColor = rend.materials[j].color;
                        tempColor.a = 0.5f;
                        rend.materials[j].color = tempColor;
                    }
                }
            }
        }
    }

    void HideVeilingObjectsWithRaycast()
    {
        //ShowVeilingObject();
        Vector3 direction = Player.transform.position - transform.position;
        RaycastHit[] hitsArray = Physics.RaycastAll(transform.position, direction, Vector3.Distance(Player.transform.position, transform.position) * 10f);

        for (int i = 0; i < hitsArray.Length; i++)
        {
            HideVeilingObject(hitsArray[i].collider.gameObject);          
        }
    }

    void CameraFollow()
    {

        cmt.m_FollowOffset = Vector3.Lerp(cmt.m_FollowOffset, OffSet * cameraZoom,Time.deltaTime*3);
        //transform.position = 
        //  Vector3.Lerp(transform.position, 
        //Player.transform.position + OffSet * cameraZoom, 
        //   Time.deltaTime*CameraFollowSpeed/2);
        //var targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,  Time.deltaTime*CameraFollowSpeed);
        // 
        //if(Input.GetKey(KeyCode.A)) transform.RotateAround(Player.transform.position, Vector3.up, 20 * Time.deltaTime);
    }


    void Zoom()
    {  
        if (cameraZoom - Input.mouseScrollDelta.y / 10 >= MinZoom && cameraZoom - Input.mouseScrollDelta.y / 10 <= MaxZoom)
            cameraZoom -= Input.mouseScrollDelta.y / 10;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DitzelGames.FastIK;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool canMove = true;
    public float rotationSpeed;
    public Camera Camera;
    public FastIKFabric LeftHandIK;
    public FastIKFabric RightHandIK;
    public GameObject ParticlePrefab;
    [Header("Items")]

    public GameObject Box;

    public bool isInteracting { get; set; }
    private bool isTalking;
    private bool isSitting = false;
    [HideInInspector]
    public bool invoked = false;
    private GameObject particleEffect;
    private GameObject lastObject;
    private GameObject destination;
    private NavMeshAgent agent;
    private ThirdPersonCharacter character;
    private Animator anim;
    private Interactable interactObj;
    public bool haveBoxInHands;
    public static Player instance;


    void OnLevelWasLoaded(int level)
    {
        /*
        var g = GameObject.Find(gameObject.name);

        if (level==2)
        {
            var jesuses = Resources.FindObjectsOfTypeAll<Player>();
            //Destroy(jesuses[0].gameObject);

            if (jesuses.Length > 2)
            {
                if (jesuses[1].transform.position.z > 210)
                {
                    jesuses[1].gameObject.SetActive(true);
                    Destroy(jesuses[2].transform.gameObject);
                    
                }
                else
                {
                    jesuses[0].gameObject.SetActive(true);
                    Destroy(jesuses[2].transform.gameObject);
                   
                }
            }
            if (gameObject.name == "Jesus")
            {
                Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                Camera.gameObject.GetComponent<CameraController>().Player = gameObject;
                Debug.Log("Camera" + Camera.gameObject.GetComponent<CameraController>().Player);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (level==1)
        {
            if (gameObject.name == "Jesusnight")
            {
                gameObject.SetActive(true);
                Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                Camera.gameObject.GetComponent<CameraController>().Player = gameObject;

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if (level==3)
        {
            Debug.Log("domek");
            Debug.Log(gameObject.name);
            if (gameObject.name == "Jesus 1")
            {
                Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                gameObject.SetActive(true);  
            }
            else
            {
                Debug.Log("dome" + gameObject.name);
                gameObject.SetActive(false);
            }
        }
     

        NPC[] npcs = FindObjectsOfType<NPC>();

        Debug.Log("ENPECETY");
            foreach (var npc in npcs)
            {
            Debug.Log(npc.name);
                if (level==2)
                    npc.player = GameObject.Find("Jesus");
                else npc.player = GameObject.Find("Jesusnight");

            Debug.Log("Night:" + npc.player);
        }
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        anim = GetComponent<Animator>();
        
        */
    }

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        anim = GetComponent<Animator>();

        // if(gameObject.name=="Jesus")DontDestroyOnLoad(transform.gameObject);
        OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
        /* Scene scene = SceneManager.GetActiveScene();
         agent = GetComponent<NavMeshAgent>();
         character = GetComponent<ThirdPersonCharacter>();
         anim = GetComponent<Animator>();

         if (instance == null)
             instance = this;
         else
         {
             var g = GameObject.Find(gameObject.name);
             if (gameObject.name == g.name && g != gameObject) Destroy(gameObject);
             return;
         }*/
    }
    void LateUpdate()
    {
        Movement();
        ShowHideObjectName();
        HandleAnimations();

        if (isInteracting)
        {
            FaceTo(interactObj.transform);
        }
        Debug.Log(canMove);
    }
    public void TakeBoxToHands()
    {
        Box.SetActive(true);
        LeftHandIK.enabled = true;
        RightHandIK.enabled = true;
        haveBoxInHands = true;
    }

    public bool GetHaveBoxInHands()
    {
        if (haveBoxInHands) return true;
        else return false;
    }


    public void DropBox()
    {
        Box.SetActive(false);
        LeftHandIK.enabled = false;
        RightHandIK.enabled = false;
        haveBoxInHands = false;
    }

    public void HandleAnimations()
    {
        anim.SetBool("Sitting", isSitting);
    }

    public void PlayAnimation(string animation)
    {
        anim.Play(animation);
    }



    void SpawnParticleEffect()
    {
        if (particleEffect != null) Destroy(particleEffect);
        particleEffect = Instantiate(ParticlePrefab, agent.destination + new Vector3(0, 0.1f, 0), new Quaternion(0, 0, 0, 0));
        particleEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        particleEffect.GetComponent<ParticleSystem>().time = 1;
        particleEffect.GetComponent<ParticleSystem>().Play();
    }

    public void ShowHideObjectName()
    {
        if (canMove)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (lastObject != null && lastObject != hit.collider.gameObject)
                    lastObject.GetComponent<Interactable>().HideName();
                if (hit.collider.gameObject.tag == "Interactable")
                {
                    lastObject = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<Interactable>().ShowName();
                }
            }
        }
        else if (lastObject != null) lastObject.GetComponent<Interactable>().HideName();
    }

    public void FaceTo(Transform target)
    {
        var rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }

    public void SetInteractObj(Interactable inter)
    {
        interactObj = inter;
    }


    public void StartInteracting()
    {
        Debug.Log("StartInteract");
        isInteracting = true;
        StopAllCoroutines();
        StartCoroutine(WaitForInteract(interactObj.waitTime));
        interactObj.onStartInteracting.Invoke();
        canMove = false;
    }

    public void Interact()
    {
        Debug.Log("Interaction");
        if (interactObj!=null) interactObj.onInteract.Invoke();
    }
    public void StopInteracting()
    {
        StopAllCoroutines();
        //anim.Play("Grounded");
        isInteracting = false;
        interactObj.onStopInteracting.Invoke();
        interactObj = null;
        canMove = true;
        agent.destination = transform.position;
        Debug.Log("STOP");
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.SetDestination(transform.position);
    }
    public void CanMove(bool can)
    {
        canMove = can;
    }


    IEnumerator WaitForInteract(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Interact();
        StopAllCoroutines();
        StopInteracting();
    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0) && canMove && Camera.gameObject.activeSelf)
        {
            agent.isStopped = false;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider.gameObject.tag == "Interactable"&&!(hit.collider.gameObject.name.Contains("Crate")&&haveBoxInHands))
                {
                    
                    invoked = false;

                    if (hit.collider.gameObject.name == "Guardian2") interactObj = GameObject.Find("Guardian").GetComponent<Interactable>();
                    else
                     interactObj = hit.collider.gameObject.GetComponent<Interactable>();

                        if (interactObj.GetComponent<Interactable>().canInteractFromAnySide)
                            agent.SetDestination(hit.point - Vector3.down);
                        else
                        {

                        if (interactObj.offSet == Vector3.zero)
                            agent.SetDestination(interactObj.transform.position + interactObj.transform.forward * interactObj.requiredDistance);
                        else
                            agent.SetDestination(interactObj.transform.position + interactObj.offSet);

                       
                        agent.stoppingDistance = 0.1f;
                            Debug.Log("asd");

                        }
                        SpawnParticleEffect();
                    }
                else
                {
                    interactObj = null;
                    agent.SetDestination(hit.point - Vector3.down);
                    SpawnParticleEffect();
                }
            }
        }

        if (agent.destination != null)
        {
            if (interactObj != null)
            {
                agent.isStopped = false;
                agent.stoppingDistance = interactObj.requiredDistance;
                if (!interactObj.canInteractFromAnySide) agent.stoppingDistance = 0.05f;
            }
            else agent.stoppingDistance = 0.5f;

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                agent.isStopped = false;
                character.Move(agent.desiredVelocity / 2, false, false);
            }
            else
            {              
                agent.isStopped = true;
                character.Move(Vector3.zero, false, false);
                if (interactObj != null)
                {
                    FaceTo(interactObj.transform);
                    var distance = Vector3.Distance(interactObj.transform.position, transform.position);
                    if (!invoked&&agent.isStopped &&agent.remainingDistance<interactObj.requiredDistance&&distance<=interactObj.requiredDistance+1)
                    {
                        Debug.Log("INTER");
                        invoked = true;
                            if (interactObj.instantInteract)
                                Interact();
                            else
                            {
                                StartInteracting();
                            }
                    }
                }
            }
        }
    }
}

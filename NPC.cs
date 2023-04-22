using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NPC : Interactable
{
    public string characterName;
    public string nothingToSay;
    public bool isSitting;  
    public GameObject exclamationMarkPrefab;
    public bool isInteracting;
    public float interactRotationSpeed = 5;   
    public bool standOnInteraction;
    public bool sitAfterInteraction;
    public UnityEvent OnNotSeeingPlayer;
    public UnityEvent OnSeeingPlayer;
    public Transform target;
    public Transform facingTarget;
    public FloatingText sentence;

    private NavMeshAgent agent;
    private ThirdPersonCharacter character;
    private bool invokedWithPlayer;
    public bool isTalking;
    private int talkingOption;
    private GameObject exclamationMark;
    private Animator anim;
    private LookAt lookAt;
    private bool isFacing;
    
    private NpcInteractable interactObj;
    private bool invoked;
    private Monologue monologue;
    public static GameObject instance;
    ConversationMeneger convMeneger;


    void OnLevelWasLoaded(int level)
    {
        if (level == 2) player = GameObject.Find("Jesus");
    }

    private void Awake()
    {
        OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);



    }
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        anim = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();
        lookAt = gameObject.GetComponent<LookAt>();
        if(floatingName==null)
        nameText.text = characterName;
        else nameText.text = floatingName;
        if (isSitting == false) standOnInteraction = false;
        HideName(); 
        // floatingText = GameObject.Find("Sentence").GetComponent<FloatingText>();
    }

    void Update()
    {
        HandleAnimations();
        HandleInteracting();
        if (!lookAt.IsAiming) OnNotSeeingPlayer.Invoke();
        else OnSeeingPlayer.Invoke();
        Movement();

        if (facingTarget != null) FaceTo(facingTarget);

    }

    public void HandleAnimations()
    {
        if(target==null)
        anim.SetBool("Sitting", isSitting);
        if ((!anim.GetCurrentAnimatorStateInfo(0).IsName("sittingIdle") && isInteracting)) lookAt.enabled = false;
        else lookAt.enabled = true;
    }

    public void goTo(Transform trans)
    {
        agent.isStopped = false;
        target=trans;
        facingTarget = null;
        interactObj = trans.gameObject.GetComponent<NpcInteractable>();
        if(interactObj!=null)
        agent.stoppingDistance = interactObj.stoppingDistance;
        invoked = false;
    }

    public void ResetTarget()
    {
        target = null;
        agent.isStopped=true;
        agent.ResetPath();
    }

    public void Movement()
    {
        if (target != null&&!isTalking)
        {
            if (isSitting) StandUp();


            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
                
                if (interactObj == null)
                {
                    agent.SetDestination(target.position);

                    if (agent.remainingDistance > agent.stoppingDistance)
                    {
                        character.Move(agent.desiredVelocity / 2, false, false);
                    }
                    else
                    {
                            character.Move(Vector3.zero, false, false);
                    }
                }

                else
                {
                    agent.SetDestination(interactObj.transform.position + interactObj.offSet);

                    if (agent.remainingDistance > agent.stoppingDistance)
                    {
                        character.Move(agent.desiredVelocity/2, false, false);
                    }
                    else
                    {
                        character.Move(Vector3.zero, false, false);
                        //FaceTo(target);
                        var distance = Vector3.Distance(agent.destination, transform.position);

                        Debug.Log("Invoked");
                        if (!invoked && distance <= interactObj.stoppingDistance+1)
                            {
                            Debug.Log("Dziala TLOS");
                            interactObj.onInteract.Invoke();
                            //invoked = true;
                            if(interactObj.afterWait!=null)
                            StartCoroutine(interactObj.StartWaiting());
                        }
                    }                   
                }
            }
        }
    }

    public void PlayAnimation(string name)
    {
        anim.Play(name);
    }

    public void TriggerTalking(string animation)
    {
        if (!isSitting)
            anim.Play(animation);
    }
    public void StandUp()
    {
        isSitting = false;
    }
    public void SitDown()
    {
        isSitting = true;
    }

    public void FaceTo(Transform target)
    {
        facingTarget = target;
        var rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * interactRotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }

    public void SetFloatingName(string name)
    {
        floatingName = name;
        nameText.text = name;
        
    }  

    public void SetStoppingDistance(float distance)
    {
        agent.stoppingDistance = distance;
    }
    public void HandleInteracting()
    {

        if ((!invokedWithPlayer && !isSitting) && (isTalking))
        {
            FaceTo(player.transform);
        }
        if (isInteracting)
        {

            character.Move(Vector3.zero, false, false);
            Ray ray = new Ray(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, (requiredDistance+1)))
            {
                if ((hit.collider.gameObject.CompareTag("Player")) && !invokedWithPlayer)
                {
                    player = hit.collider.gameObject;
                    Interact();

                    invokedWithPlayer = true;
                }
            }
        }
    }


    public void StartLeadingPlayer()
    {
        agent.isStopped = false;
    }
    public void StopLeadingPlayer()
    {
        agent.isStopped = true;
    }


    public void SaySomething(string text)
    {
        Debug.Log(text);
        
        sentence.gameObject.SetActive(true);
        sentence.text = text;
        Debug.Log(sentence.transform.parent);
        StartCoroutine(DiseactiveDialogue());
    }

    public void SayMonologueSentence()
    {
        Debug.Log(monologue.sentences[monologue.currentSentence].text);
        sentence.gameObject.SetActive(true);
        sentence.text = monologue.sentences[monologue.currentSentence].text ;
        StopAllCoroutines();
        StartCoroutine(NextSentence(monologue.sentences[monologue.currentSentence].duration));
       
    }

    public void StartMonologue(Monologue mon)
    {
        monologue = mon;
        Debug.Log("Start");
        SayMonologueSentence();
    }
    public IEnumerator NextSentence(float time)
    {

        yield return new WaitForSeconds(time);
        if (monologue.currentSentence < monologue.sentences.Length - 1)
        {
            monologue.currentSentence++;
            SayMonologueSentence();
        }
        else
        {
            monologue.onFinish.Invoke();
            StopMonologue();
        }

    }
    public void StopMonologue()
    {
        sentence.text = "";
        monologue = null;
    } 

    IEnumerator DiseactiveDialogue()
    {
        yield return new WaitForSeconds(4);
        sentence.gameObject.SetActive(false);
        StopInteracting();
    }

    void ChangeNothingToSay(string text)
    {
        nothingToSay = text;
    }

    public void StartFacingToPlayer()
    {
        isFacing = true;
    }

    public void StopFacingToPlayer()
    {
        isFacing = false;
        facingTarget = null;
    }

    public void StartInteracting()
    {
        isInteracting = true;

    }

    public override void Interact()
    {
        if (GetComponent<Conversation>().dialogues.Where(d => d.isEnabled).ToList().Count <= 0||!canInteract)
        {
            SaySomething(nothingToSay);
        }
        else
        {
            player.GetComponent<ConversationMeneger>().StartConversation(GetComponent<Conversation>());
            player.GetComponent<Player>().canMove = false;
            if (standOnInteraction) isSitting = false;
        }
    }

    public void StopInteracting()
    {
        isInteracting = false;
        invokedWithPlayer = false;
        player.GetComponent<Player>().CanMove(true);
        player.GetComponent<Player>().isInteracting = false;
        if (standOnInteraction&&sitAfterInteraction) isSitting = true;
    }

    private void OnDrawGizmos()
    {
        if (interactObj != null)
            Gizmos.DrawSphere(interactObj.offSet, 1);
    }
}

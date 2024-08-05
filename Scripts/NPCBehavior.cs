using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    //NPC states.
    public enum FSMStates
    {
        Idle,
        Follow,
        Talk
    }

    //Current NPC state.
    public FSMStates currentState;

    //Player information.
    public GameObject player;

    //Animator controller of NPC.
    Animator m_Animator;

    //Distance and travel variables.
    float distanceToPlayer;
    public float speakDistance;
    public float followDistance;
    public float NPCSpeed = 3f;
    private bool isFollowing;

    //Audio clips.
    public AudioClip speakClip;

    // Start is called before the first frame update
    void Start()
    {
        //Finds possibly null objects.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //Gets animator component.
        m_Animator = GetComponent<Animator>();
        //Animation int for transitions is called "AnimationInt"
        //AnimationInt = 0 is idol.
        //AnimationInt = 1 is running when the player moves.
        //AnimationInt = 2 is speaking.

        //Sets current NPC state.
        currentState = FSMStates.Idle;

        //Sets bools.
        isFollowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case FSMStates.Idle:
                UpdateIdolState();
                break;
            case FSMStates.Follow:
                UpdateFollowState();
                break;
            case FSMStates.Talk:
                UpdateTalkState();
                break;
        }
    }

    void UpdateIdolState()
    {
        m_Animator.SetInteger("AnimationInt", 0);
        /*
        if(distanceToPlayer < speakDistance && ##Space Key is pushed##){
            currentState = FSMStates.Talk;
        }
        */

    }

    void UpdateFollowState()
    {
        if(distanceToPlayer > followDistance && isFollowing){
            m_Animator.SetInteger("AnimationInt", 1);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, NPCSpeed * Time.deltaTime);
        }
    }

    void UpdateTalkState()
    {

    }
}
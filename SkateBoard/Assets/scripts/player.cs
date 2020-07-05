using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private charackterClass myPlayer; //character class 
    private Rigidbody rb; // rigidbody component
    private Animator anim;
    private bool isStartUnit=true;
    private string objectTag;
    private bool isGrounded;
    private bool isDead;
    private Vector3 pos;
    private bool deactiveTime;
    [SerializeField] private Vector3[] wayPos;
    

    [SerializeField] private float directionSpeed;
    [SerializeField] private float moveSpeed;
    [Header("Unit")] [SerializeField] private int unit=3; // left and rigt units
    void Start()
    {
        myPlayer=new charackterClass(); // new character class 
        rb = GetComponent<Rigidbody>(); // gep player component
        anim = GetComponent<Animator>();
        anim.SetBool("isStart",isStartUnit);
        StartCoroutine(isStartStop());
        StartCoroutine(isJump());
        StartCoroutine(isMove());
        myPlayer.WayObjects();
        myPlayer.wayPos=wayPos;

    }

    // Update is called once per frame
    void Update()
    {
        myPlayer.MouseLeftRight(this.gameObject.transform,rb,directionSpeed,anim); 
        myPlayer.JumpFunc(rb,anim,this.transform);
        rb.MovePosition(rb.position+Vector3.forward*(moveSpeed*Time.deltaTime));
        
        //Debug.DrawRay(new Vector3(transform.position.x,0.1f,transform.position.z+0.7f),transform.forward,Color.red,3f,true );
       //print(Physics.gravity.magnitude);

    }

    IEnumerator isStartStop()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("isStart",false);
        myPlayer.isStartUnit = false;
    }

    IEnumerator isJump()
    {
        yield return new WaitForEndOfFrame();
        objectTag = myPlayer.RayFunc(this.transform);
        print(objectTag);
        isGrounded = myPlayer.isGround;
        if (objectTag != null )
        {
            if (objectTag == "notSlide" && !isGrounded && !deactiveTime)
            {
                myPlayer.AnimationPlay(anim,0.1f,"y");
            }
            else if (objectTag == "slide" && !isGrounded)
            {
                myPlayer.AnimationPlay(anim,0.2f,"y");
            }
            else if (objectTag == "rampa" && !isGrounded && !deactiveTime)
            {
                myPlayer.AnimationPlay(anim,0.1f,"y");
            }
   
        }
        else if(objectTag==null && !isGrounded)
        {
            if (!deactiveTime)
            {
                myPlayer.AnimationPlay(anim,0.5f,"y");
            }
        }
        else
        {
            myPlayer.AnimationPlay(anim,0f,"y");
            
        }
        yield return isJump();
    }

    IEnumerator DeactiveTime()
    {
        if (deactiveTime)
        {
            yield return new WaitForSeconds(2f);
            deactiveTime = false;
        }
    }
    IEnumerator isDeadPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        if (isDead)
        {
            moveSpeed = 0;
        }
    }
    IEnumerator isMove()
    {
        yield return new WaitForSeconds(0.3f);
        if (moveSpeed < 20)
        {
            moveSpeed++;
            yield return isMove();
        }
        else
        {
            moveSpeed = 20f;
        }
    }

    IEnumerator StartGame()
    {
        isStartUnit = true;
        myPlayer.isStartUnit = true;
        moveSpeed = 0;
        yield return new WaitForSeconds(4f);
        isDead = false;
        anim.SetBool("isDead",isDead);
        StartCoroutine(isMove());
        transform.position = new Vector3(pos.x,pos.y,pos.z+5);
        StartCoroutine(PlayerStart());
        myPlayer.AnimationPlay(anim,0f,"y");
        myPlayer.AnimationPlay(anim,isStartUnit,"isStart");
        StartCoroutine(isStartStop());




    }

    IEnumerator PlayerStart()
    {
        yield return new WaitForSeconds(0.3f);
        if (moveSpeed <= 10f)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().enabled =
                !GetComponentInChildren<SkinnedMeshRenderer>().enabled;
            yield return PlayerStart();
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "slide")
        {
            myPlayer.AnimationPlay(anim,0.4f,"y");
            deactiveTime = true;
        }
        if(other.collider.tag=="notSlide")
        {
            pos = transform.position;
            isDead = true;
            anim.SetBool("isDead",isDead);
            StartCoroutine(StartGame());
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "slide")
        {
            myPlayer.AnimationPlay(anim,0.3f,"y");
            StartCoroutine(DeactiveTime());

        }
    }
}

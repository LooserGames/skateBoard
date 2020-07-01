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
    private bool isStartUnit;
    private string objectTag;
    private bool isGrounded;
    private bool isDead;
    private Vector3 pos;
    

    [SerializeField] private float directionSpeed;
    [SerializeField] private float moveSpeed;
    [Header("Unit")] [SerializeField] private int unit=3; // left and rigt units
    void Start()
    {
        myPlayer=new charackterClass(); // new character class 
        rb = GetComponent<Rigidbody>(); // gep player component
        anim = GetComponent<Animator>();
        anim.SetBool("isStart",true);
        StartCoroutine(isStartStop());
        StartCoroutine(isJump());
        StartCoroutine(isMove());

        
    }

    // Update is called once per frame
    void Update()
    {
        myPlayer.MouseLeftRight(this.gameObject.transform,rb,directionSpeed,anim); 
        myPlayer.JumpFunc(rb,anim,this.transform);
       // rb.MovePosition(rb.position+Vector3.forward*(moveSpeed*Time.deltaTime));

       

    }

    IEnumerator isStartStop()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("isStart",false);
    }

    IEnumerator isJump()
    {
        yield return new WaitForEndOfFrame();
        objectTag = myPlayer.RayFunc(this.transform);
        isGrounded = myPlayer.isGround;
        if (objectTag != null )
        {
            if (objectTag == "notSlide" && !isGrounded)
            {
                myPlayer.AnimationPlay(anim,0.1f,"y");
            }
            else if (objectTag == "slide" && !isGrounded)
            {
                myPlayer.AnimationPlay(anim,0.2f,"y");
            }
   
        }
        else if(objectTag==null && !isGrounded)
        {
            myPlayer.AnimationPlay(anim,0.5f,"y");
        }
        else
        {
            myPlayer.AnimationPlay(anim,0,"y");
            
        }
        yield return isJump();
    }

    IEnumerator isDeadPlayer()
    {
        yield return new WaitForEndOfFrame();
        if (isDead)
        {
            moveSpeed = 0;
        }
    }
    IEnumerator isMove()
    {
        yield return new WaitForSeconds(0.3f);
        if (moveSpeed < 50)
        {
            moveSpeed++;
            yield return isMove();
        }
        else
        {
            moveSpeed = 50f;
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(isMove());
        transform.position = new Vector3(pos.x,pos.y,pos.z+2);
        StartCoroutine(PlayerStart());
        myPlayer.AnimationPlay(anim,0f,"y");
        isDead = false;
    }

    IEnumerator PlayerStart()
    {
        yield return new WaitForSeconds(0.3f);
        if (moveSpeed <= 20f)
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
        }
        if(other.collider.tag=="notSlide")
        {
            pos = transform.position;
            isDead = true;
            anim.SetBool("isDead",true);
            StartCoroutine(isDeadPlayer());
            StartCoroutine(StartGame());
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "slide")
        {
            myPlayer.AnimationPlay(anim,0.3f,"y");
        }
    }
    
    
    
    /*
     public void MouseLeftRight(Transform player ,Rigidbody rb, float speed,Animator anim)
   {
       // burda mouse hareketlerine göre sağa ve sola gitme işlemlerini yapacak
       float x = MouseControll();
       bool isStartUnit = anim.GetBool("isStart");

       // mouse pozisyonunu aldık ve sağ sol değerleri true ve false yaptık
       if (x > 0.7f  && !movingRight && isGround && !isStartUnit)
       {
          
          
           movingRight = true;
           movingLeft = false;
       }
       if(x < 0.3f && !movingLeft && isGround && !isStartUnit)
       {
          
          
           movingRight = false;
           movingLeft = true;
       }

       //burda eğer değerler true ise işlemleri yaptırıyoruz
       if (movingRight && player.position.x<1)
       {
           rb.MovePosition(rb.position+speed*Time.deltaTime*Vector3.right);
           AnimationPlay(anim,1.0f,"x");
          
       }
       else
       {
           movingRight = false;
           if (player.position.x > 1)
           {
               player.position = new Vector3(1,player.position.y,player.position.z);
               AnimationPlay(anim,0.0f,"x");
           }
       }
       
       if (movingLeft && player.position.x>-1 && !isStartUnit)
       {
           rb.MovePosition(rb.position+speed*Time.deltaTime*Vector3.left);
           AnimationPlay(anim,-1.0f,"x");
       }
       else
       {
           movingLeft = false;
           if (player.position.x < -1)
           {
               player.position = new Vector3(-1,player.position.y,player.position.z);
               AnimationPlay(anim,0.0f,"x");
           }
       }

      
   }
     */
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class charackterClass : MonoBehaviour
{ 
    public bool isGround=true; //isground true and false
    public float treshold=9; //touch max pos
    public bool isStartUnit=true;
    public Vector3[] wayPos;
    
    private bool movingLeft, movingRight;
    
    int currentWay = 0,targetWay=0;
    /// <summary>
    /// General character controll class
    /// </summary>
    /// <returns></returns>
    
    
    //private
    //touch pos controll
    float TouchControll()
    {
        float x=0;

        Touch touch= new Touch();
        touch = Input.GetTouch(0);
        
        
        if (touch.phase == TouchPhase.Moved && touch.fingerId==0)
        {
            x += touch.deltaPosition.x;
        }

        return x;
    }
    
    //public 
    //player touch left rigt method
    public void LeftRight(Transform player, float unit)
    {
        float x = TouchControll();

        if (x > treshold)
        {
            player.position=new Vector3(-unit,0,0);
        }
        else if(x<-treshold)
        {
            player.position=new Vector3(unit,0,0);
        }
    }

 
    //Animation methods
    public void AnimationPlay(Animator anim,int unit, string animName)
    {
        anim.SetInteger(animName,unit);
    }
    
    public void AnimationPlay(Animator anim,bool boolUnit, string animName)
    {
        anim.SetBool(animName,boolUnit);
    }
    
    public void AnimationPlay(Animator anim,float unet, string animName)
    {
        anim.SetFloat(animName,unet);
    }

    ///////////////////////////////////////////////////////////////////////////////////////

    
    //jump method
    public void JumpFunc(Rigidbody rb,Animator anim,Transform player)
    {
        float y = MouseYControll();
        string obstacle = RayFunc(player);
        Vector3 tempVelocity;
        
        
        if (y > 0.45f && isGround==true && obstacle!=null && !isStartUnit)
        {
            tempVelocity = rb.velocity;
            tempVelocity.y = Mathf.Sqrt(2f * 2f * Physics.gravity.magnitude);
            rb.velocity = tempVelocity;
            isGround = false;
            AnimationPlay(anim,isGround,"isGround");
        }
        
        else if(rb.gameObject.transform.position.y<=0.20f)
        {
            isGround = true;
            AnimationPlay(anim,isGround,"isGround");
        }
        
        if (y > 0.45f && obstacle == null && isGround==true && !isStartUnit)
        {
            
            tempVelocity = rb.velocity;
            tempVelocity.y = Mathf.Sqrt(0.5f * 2f * Physics.gravity.magnitude);
            rb.velocity = tempVelocity;
            isGround = false;
            AnimationPlay(anim,isGround,"isGround");
        }
        
        else if(rb.gameObject.transform.position.y<=0.15f)
        {
            isGround = true;
            AnimationPlay(anim,isGround,"isGround");
        }
       
    }

   public string RayFunc(Transform player)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(player.position.x,0.1f,player.position.z+0.7f), player.forward, out hit, 7f))
        {
            return hit.transform.tag;
        }

        return null;
    }
    
   //test mouse function
   /*public void MouseLeftRight(Transform player ,Rigidbody rb, float speed,Animator anim)
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
  /* public void MouseLeftRight(Transform player, Rigidbody rb, float speed, Animator anim)
   {
       float x = MouseControll();
       bool isStartUnit = anim.GetBool("isStart");
       int way = 0;
      
       
       if (x > 0.7f  && !movingRight && isGround && !isStartUnit)
       {
           movingLeft = false;
           movingRight = true;
       }
      
       
       if (x < 0.3f  && !movingRight && isGround && !isStartUnit)
       {
           movingLeft = true;
           movingRight = false;
           
       }
       
       
       if (player.position.x >= 0.0f && player.position.x <= 4.0f)
       {
           if (player.position.x>1.9f && player.position.x<2.1f && way!=1)
           {
               AnimationPlay(anim, 0f, "x");
               rb.MovePosition(new Vector3(2f,player.position.y,player.position.z));
               way = 1;
           }
          
           
           if (player.position.x>=0 && movingLeft)
           {
               rb.MovePosition(rb.position + speed *Time.deltaTime*Vector3.left);
               if (player.position.x < 0)
               {
                   way = 0;
               }
               AnimationPlay(anim, -1.0f, "x");
           }
          

           if (player.position.x<=4 && movingRight)
           {
               rb.MovePosition(rb.position+speed*Time.deltaTime*Vector3.right);
               if (player.position.x > 4)
               {
                   way = 2;
               }
               AnimationPlay(anim, 1.0f, "x");
           }
           
       }
       else
       {
           movingLeft = false;
           movingRight = false;
           player.position = new Vector3(Mathf.Round(player.position.x), player.position.y, player.position.z);
           AnimationPlay(anim, 0.0f, "x");
       }
     
   }*/

  public void MouseLeftRight(Transform player, Rigidbody rb, float speed, Animator anim)
  {
      float x = MouseControll();


      if (!isStartUnit && isGround)
      {
          if (currentWay != 2)
          {
              if (x >= 0.7f && targetWay < 2f && Mathf.Abs(currentWay-targetWay)==0)
              {
                  targetWay++;
              }
          }

          if (currentWay != 0)
          {
              if (x <= 0.3f && targetWay > 0f && Mathf.Abs(currentWay-targetWay)==0)
              {
                  targetWay--;
              }
          }

          targetWay = Mathf.Clamp(targetWay, 0, 2);

          if (Mathf.Abs(player.position.x - wayPos[targetWay].x) > 0.1f)
          {
              if (player.position.x<wayPos[targetWay].x)
              {
                  rb.MovePosition(rb.position+speed*Time.deltaTime*Vector3.right);
                  AnimationPlay(anim, 1.0f, "x");
              }

              if (player.position.x > wayPos[targetWay].x)
              {
                  rb.MovePosition(rb.position+speed*Time.deltaTime*Vector3.left);
                  AnimationPlay(anim, -1.0f, "x");
              }
          }
          if (Mathf.Abs(player.position.x - wayPos[targetWay].x) <= 0.1f)
          {
              currentWay = targetWay;
              player.position=new Vector3(wayPos[targetWay].x,player.position.y,player.position.z);
              AnimationPlay(anim, 0.0f, "x");
          }
          
      }
  }

public void WayObjects()
  {
      //wayPos[0] = transform.Find("left").position;
     // wayPos[1] = transform.Find("midle").position;
      //wayPos[2] = transform.Find("right").position;
  }

//mouse x controll
   private float MouseControll()
   {
       float x=0.5f;
       if (Input.GetMouseButton(0))
       {
           x= Input.mousePosition.x/Screen.width;
           //print(x);
       }
       return x;
   }

   //mouse y control for jump
   public float MouseYControll()
   {
       
       float y=0.0f;
       if (Input.GetMouseButton(0))
       {
           y = Input.mousePosition.y / Screen.width;
       }
       return y;
   }
    
}




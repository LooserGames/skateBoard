using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charackterClass : MonoBehaviour
{
    
    /// <summary>
    /// General character controll class
    /// </summary>
    /// <returns></returns>
    
    
    //private
    float TouchControll()
    {
        float x=0;
        int finderID;
        Touch touch= new Touch();
        touch = Input.GetTouch(0);
        finderID = touch.fingerId;
        if (touch.phase == TouchPhase.Moved && touch.fingerId==finderID)
        {
            x = touch.deltaPosition.x;
        }

        return x;
    }
    
    //public 
    public void LeftRight(Transform player, float unit)
    {
        float x = TouchControll();

        if (x < 0)
        {
            player.position=new Vector3(-unit,0,0);
        }
        else
        {
            player.position=new Vector3(unit,0,0);
        }
    }

 

    public void AnimationPlay(Animator anim,int unit, string animName)
    {
        anim.SetInteger(animName,unit);
    }

   //test mouse function
   public void MouseLeftRight(Transform player,float unit)
   {
       float x = MouseControll();

       if (x < 0)
       {
           player.position = new Vector3(transform.position.x-unit,transform.position.y,transform.position.z);
       }
      
   }

   private float MouseControll()
   {
       float x=0;
       if (Input.GetMouseButton(0) && Input.mousePosition.sqrMagnitude!=0.0f)
       {
           x = Input.mousePosition.x;
           print("bastı");
           
       }
       return x;
   }
    
}

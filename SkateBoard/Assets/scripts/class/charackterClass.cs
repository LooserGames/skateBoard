using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charackterClass : MonoBehaviour
{

    public float treshold=9;
    
    
    
    /// <summary>
    /// General character controll class
    /// </summary>
    /// <returns></returns>
    
    
    //private
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

    int way=1;
   //test mouse function
   public void MouseLeftRight(Transform player,float unit)
   {
       float x = MouseControll();
       

       if (x > 0.5f && way==1)
       {
           player.position=new Vector3(unit,player.position.y,player.position.z);
           way = 2;
       }
       if(x < 0.5f && way==2)
       {
           player.position=new Vector3(-unit,player.position.y,player.position.z);
           way = 1;
       }

      
   }

   private float MouseControll()
   {
       float x=0.5f;
       if (Input.GetMouseButton(0))
       {
           x= Input.mousePosition.x/Screen.width;
           print(x);
       }
       return x;
   }
    
}

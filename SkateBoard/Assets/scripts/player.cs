using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private charackterClass myPlayer;

    [Header("Unit")] [SerializeField] private int unit=3;
    void Start()
    {
        myPlayer=new charackterClass();
    }

    // Update is called once per frame
    void Update()
    {
        myPlayer.MouseLeftRight(this.gameObject.transform,unit);
    }
}

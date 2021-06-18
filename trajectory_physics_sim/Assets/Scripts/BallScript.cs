using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestroy", 30.0f);
    }


    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

 
   
}

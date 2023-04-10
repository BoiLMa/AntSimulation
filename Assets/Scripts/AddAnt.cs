using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAnt : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<AntBehaviour>().totalFood > 9)
        {
            GetComponent<AntBehaviour>().antList.Add(new Ant(Instantiate(GetComponent<AntBehaviour>().ant)));
            GetComponent<AntBehaviour>().totalFood -= 10;
        }       
    }
}

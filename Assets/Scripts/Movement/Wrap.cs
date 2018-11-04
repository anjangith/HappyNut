using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour {

    /// <summary>
    /// X min
    /// </summary>
    public int leftBound;

    /// <summary>
    /// X max
    /// </summary>
    public int rightBound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (transform.localPosition.x > rightBound)
	    {
	        var pos = transform.position;
	        pos.x = leftBound;
	        transform.position = pos;
	    }
	}
}

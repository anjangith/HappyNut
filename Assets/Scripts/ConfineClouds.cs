using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfineClouds : MonoBehaviour
{
    /// <summary>
    /// X min and max
    /// </summary>
    public int leftBound;

    /// <summary>
    /// X min and max
    /// </summary>
    public int rightBound;
    
    private MoveToLocations2D[] childrenMoveToLocations2Ds;

    // Use this for initialization
    void Start ()
    {
        childrenMoveToLocations2Ds = GetComponentsInChildren<MoveToLocations2D>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    foreach (MoveToLocations2D moveToLocations2D in childrenMoveToLocations2Ds)
	    {
	        if (moveToLocations2D.transform.position.x > rightBound)
	        {
	            var pos = moveToLocations2D.transform.position;
	            pos.x = leftBound;
	            moveToLocations2D.SetLocation(pos);
	        }
	    }
	}
}

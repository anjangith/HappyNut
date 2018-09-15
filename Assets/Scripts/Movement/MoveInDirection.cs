using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    public Vector3 Direction;
    public int Speed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Direction * Speed * Time.deltaTime;
	}

}

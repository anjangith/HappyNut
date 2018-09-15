using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocations2D : MonoBehaviour
{

    #region public Variables

    public float Speed;

    /// <summary>
    /// This is like an animation to move.
    /// </summary>
    public Vector2[] Frames;

    /// <summary>
    /// This is the length of the frame.
    /// </summary>
    [Tooltip("This is the length of the frame.")]
    public float[] FrameDuration;

    /// <summary>
    /// Should we start moving on start.
    /// </summary>
    [Tooltip("Should we start moving on start.")]
    public bool PlayOnWake;

    /// <summary>
    /// Where to Reset and play animation from on loop.
    /// </summary>
    [Tooltip("Where to Reset and play animation from on loop.")]
    public Vector3 ResetOffset;

    public bool IsLooping;

    #endregion

    #region Private Variables

    private float timeElapsed;

    private int currentFrame = 0;

    private bool isPlaying;

    private Vector3 startingLocation;

    #endregion

    // Use this for initialization
    void Start ()
    {
        timeElapsed = Time.time;
        isPlaying = PlayOnWake;
        startingLocation = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {

	    if (transform.position.x > 16.5f)
	    {
	        var pos = transform.localPosition;
	        pos.x = -16.5f;
	        transform.localPosition = pos;
	        startingLocation = pos;
	    }


	    if (isPlaying && Time.time - timeElapsed < FrameDuration[currentFrame])
	    {
	        // The step size is equal to speed times frame time.
	        float step = Speed * (Time.time - timeElapsed) / FrameDuration[currentFrame];
	        transform.localPosition = startingLocation +
	                                  new Vector3(Frames[currentFrame].x, Frames[currentFrame].y, 0) * step;
	    }
        else if (currentFrame < FrameDuration.Length - 1)
	    {
	        currentFrame++;
	        timeElapsed = Time.time;
	    }
	    else
	    {
	        AnimationFinished();
	    }
    }

    public void SetLocation(Vector3 location)
    {
        transform.position = location;
        startingLocation = transform.localPosition - startingLocation + location;
    }

    private void AnimationFinished()
    {
        if (IsLooping)
        {
            timeElapsed = Time.time;
            currentFrame = 0;
        }
    }

}

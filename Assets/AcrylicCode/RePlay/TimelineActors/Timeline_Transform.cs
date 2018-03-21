using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class Timeline_Transform : MonoBehaviour 
{
	//PUBLIC
	[Header("Recording Settings")]
	public float keyframeInterval = 0.2f;
	public bool recordPosition = true;
	public bool recordRotation = true;
	public bool recordScale = false;
	[Header("Events")]
	public UnityEvent recordStartEvent = new UnityEvent();
	public UnityEvent recordEndEvent = new UnityEvent();

	//PRIVATE
	private bool recording = false;
	private float maxTime = 30f;
	private KeyframeVector3 positionKey = new KeyframeVector3();
	private KeyframeQuaternion rotationKey = new KeyframeQuaternion();
	private KeyframeVector3 scaleKey = new KeyframeVector3();

    private KeyframeFloat attackKey = new KeyframeFloat();

	void Awake()
	{
		//add to events
		RePlayTimeline.replayTimeEvent += SampleTime;
		RePlayTimeline.recordingEvent += ReceiveRecordingState;

		//check if currently already recording
		if( RePlayTimeline.SELF != null && RePlayTimeline.SELF.recording == true )
		{
			ReceiveRecordingState( true );
		}
	}

	void OnDestroy()
	{
		//remove from events when object is destroyed
		RePlayTimeline.replayTimeEvent -= SampleTime;
		RePlayTimeline.recordingEvent -= ReceiveRecordingState;
	}

	void SampleTime( float time )
	{
		//position
		if( recordPosition == true )
		{
			this.transform.position = positionKey.SampleCurves( time );
		}

		//rotation
		if( recordRotation == true )
		{
			this.transform.rotation = rotationKey.SampleCurves( time );
		}

		//scale
		if( recordScale == true )
		{
			this.transform.localScale = scaleKey.SampleCurves( time );
		}

	}

	void ReceiveRecordingState( bool receivedState )
	{
		//get the maxTime from the timeline
		maxTime = RePlayTimeline.SELF.maxTime;

		//set state of recording
		recording = receivedState;

		if( recording == true )
		{
			recordStartEvent.Invoke();

			StartCoroutine("WriteCurves");
		}
		if( recording == false )
		{
			recordEndEvent.Invoke();
		}
	}

	IEnumerator WriteCurves()
	{
		float time = 0;

		//reset variables
		positionKey = new KeyframeVector3();
		rotationKey = new KeyframeQuaternion();
		scaleKey = new KeyframeVector3();

		float removeTime = 0;
		while( recording == true )
		{
			time = RePlayTimeline.SELF.CurrentTime();

			//remove keyframes that are not within timeframe
			removeTime += keyframeInterval;
			if( removeTime > 1 )
			{
				CheckMaxTime(time);
				removeTime = 0;
			}

			//position
			if( recordPosition == true )
			{
				positionKey.AddKeyframe( transform.position, time );
			}

			//rotation
			if( recordRotation == true )
			{
				rotationKey.AddKeyframe( transform.rotation, time );
			}

			//scale
			if( recordScale == true )
			{
				scaleKey.AddKeyframe( transform.localScale, time );
			}

         

			yield return new WaitForSeconds( keyframeInterval );
		}

		//remove keyframes that are not within timeframe
		CheckMaxTime(RePlayTimeline.SELF.CurrentTime());

		//when recording stops, write to curves
		if( recordPosition == true )
		{
			positionKey.SetCurves();
		}
		if( recordRotation == true )
		{
			rotationKey.SetCurves();
		}
		if( recordScale == true )
		{
			scaleKey.SetCurves();
		}
        

		yield return null;
	}

	void CheckMaxTime(float time)
	{	
		for( int i = 0; i < positionKey.keysX.Count; i++ )
		{
			if( positionKey.keysX[i].time >= time - maxTime )
			{
				positionKey.keysX.RemoveRange(0,i);
				positionKey.keysY.RemoveRange(0,i);
				positionKey.keysZ.RemoveRange(0,i);
				break;
			}
		}
		for( int i = 0; i < rotationKey.keysX.Count; i++ )
		{
			if( rotationKey.keysX[i].time >= time - maxTime )
			{
				rotationKey.keysX.RemoveRange(0,i);
				rotationKey.keysY.RemoveRange(0,i);
				rotationKey.keysZ.RemoveRange(0,i);
				rotationKey.keysW.RemoveRange(0,i);
				break;
			}
		}
	}
}
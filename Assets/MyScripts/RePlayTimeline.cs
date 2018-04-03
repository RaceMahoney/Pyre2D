using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RePlayTimeline : MonoBehaviour 
{
	//EVENT DELEGATES
	public delegate void RePlayTime(float time);
	public delegate void RecordingDelegate(bool toggle);

	//EVENTS

	/// <summary>
	/// Announces the time to any timeline objects so that they can sample their stored values at that time
	/// </summary>
	public static RePlayTime replayTimeEvent;
	public void ReplayTimeAnnounce(float time){ if(replayTimeEvent!=null) replayTimeEvent(time); }

	/// <summary>
	/// Announces to actors whether to record or not
	/// </summary>
	public static RecordingDelegate recordingEvent;
	public void RecordingAnnounce(bool recording){ if(recordingEvent!=null) recordingEvent(recording); }


	//PUBLIC VARIABLES
	[HideInInspector]public static RePlayTimeline SELF; //instance of self

	public Slider timeScrubSlider;
	public Text timeText;
	public float maxTime = 30f;

	private float startTime = 0;
	private float timelineTime = 0;
	private float endTime = 0;

	[HideInInspector] public bool recording = false;

	void Awake()
	{
		//create instance
		if( SELF == null )
			SELF = this;
	}

	void Start()
	{
		StartRecording();
	}

	/// <summary>
	/// Returns the current time nornmalized by the start time of current time session.
	/// </summary>
	/// <returns>The time.</returns>
	public float CurrentTime()
	{
		return Time.time - startTime;
	}

	public void StartRecording()
	{
		if( recording == false )
		{
			recording = true;

			//set start time to current actual time
			startTime = Time.time;

			//timeScrubSlider.value = 1;

			RecordingAnnounce( true );
		}
	}

	public void StopRecording()
	{
		if( recording == true )
		{
			recording = false;

			//set start time to current actual time
			endTime = CurrentTime();

			//set slider to only show max time rangge
			timeScrubSlider.minValue = Mathf.Clamp01(1 - ( maxTime / (endTime-startTime) ));

			RecordingAnnounce( false );
		}
	}
		
	/////////////////
	/// functions for playing back the timeline

	public void SampleTimeline( Slider slider )
	{
		StopRecording();

		//update timeline time
		timelineTime = slider.value * endTime;

		//announce time to sample to all actors
		ReplayTimeAnnounce( timelineTime );

		//update time text
		string currentTime = (int)(timelineTime/60) + "." + (int)(timelineTime%60);
		string totalTime = (int)(endTime/60) + "." + (int)(endTime%60);
		timeText.text = currentTime + " / " + totalTime;
	}

	public void Play( float speed )
	{
		//alter start time based on input speed
		if( recording == true )
		{
			if( speed < 0 )
			{
				timeScrubSlider.value = 1;
				timelineTime = endTime;
			}
			if( speed > 0 )
			{
				timeScrubSlider.value = 0;
				timelineTime = 0;
			}

			//sample
			SampleTimeline( timeScrubSlider );
		}

		//make sure everything is stopped
		StopCoroutine( "PlayTimeline" );
		StopRecording();

		//finally play the timeline at the given speed
		StartCoroutine( "PlayTimeline", speed );
	}

	public void Pause()
	{
		StopRecording();

		StopCoroutine( "PlayTimeline" );
	}

	IEnumerator PlayTimeline( float speed )
	{
		while( timelineTime <= endTime && timelineTime >= 0 && recording == false )
		{
			//update time of timeline
			timelineTime += Time.deltaTime * speed;

			//set the slider value
			timeScrubSlider.value = timelineTime / endTime;

			//sample
			SampleTimeline( timeScrubSlider );

			yield return null;
		}

		yield return null;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTutorial : MonoBehaviour
{
	public static ZoneTutorial Instance;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	// Class for controlling the timer of the zone
	[System.Serializable]
	public class Timer
	{
		public float CurrentTime, EndTime;

		public Timer(float curTime, float endTime)
		{
			CurrentTime = curTime;
			EndTime = endTime;
		}
	}

	// Class for controlling events of the zone
	[System.Serializable]
	public class ZoneEvents
	{
		[System.Serializable]
		public class FloatUnityEvent : UnityEvent<Timer>
		{
		}

		public FloatUnityEvent AreaIsShrinking,
							   WaitingForShrinking,
							   AreaStartShrinking,
							   StartWaitingForShrinking,
							   EndOfLastStep;
	}

	// Class for controlling radius and position of circle/step instance
	[System.Serializable]
	public class ZoneCircleBase
	{
		public Vector3 Position;
		public float Radius;
	}

	// Class for controlling other info of circle/step instance
	[System.Serializable]
	public class ZoneCircleInfo
	{
		public ZoneCircleBase PositionAndRadius;
		public float StartDelay;         // Delay before start shrinking
		public float ShrinkingTime;      // Time of the shinking period
		[Range(0.5f, 2f)] public float MoveSpeedRange = 1; // Scale of shrinking speed (optional)
	}

	public int StepsToEnd = 6;    // Zone steps/circles count
	public bool ResetupZoneCirclesOnStart = true; // Do we need to re-setup steps on start play ?

	public bool StartupTheZoneOnStart = true; // Do we want to startup the zone on start play ? Default is YES,

	// if not you can simply just call method StartWaitingForNewSafeZone() later
	public ZoneCircleTutorial[]
		LastZoneCircle; // Pre setted  circles on scene , one of them will be randomly choose for last step/circle
	public ZoneCircleTutorial[]
		FirstZoneCircle; // Pre setted  circles on scene , one of them will be randomly choose for first step/circle

	public ZoneCircleBase
		CurrentSafeZone, NextSafeZone; // Current and next circle/step info

	public List<ZoneCircleInfo> ZoneCircles = new List<ZoneCircleInfo>(); // All setup circles/steps

	public AnimationCurve
		ZoneCirclesSizeCurve; // Curve of size of the steps/circles between first and last circle (where 0 its size of first circle and 1 its size of last circle)

	public ZoneEvents Events; // Events of the zone

	//This booleans show handles on scene (show all circles/steps, show current circle/step, show next circle/step)
	public bool ShowStepsCircles = true, ShowCurrentStepCircle, ShowNextStepCircle;

	private Timer _timer;
	private bool _isShrinking;

	public bool IsShrinking { get { return _isShrinking; } set { _isShrinking = value; } }

	public int CurStep { get; private set; } // By this property you can know what the current step index right now

	private void Start()
	{
		CreateEvents();
		// Creating events
		if (ResetupZoneCirclesOnStart)
		{
			int i = Random.Range(0, FirstZoneCircle.Length);
			int j = Random.Range(0, LastZoneCircle.Length);
			SetupCircles(i, j);
		}  // Setuping circles if needed
		if (StartupTheZoneOnStart)
		{
			WaitForNewSafeZoneCmd(ZoneCircles[0].StartDelay, CurStep);
		}  // Startup of the zone working on start play
	}

	// Method for creating events and timer instances
	private void CreateEvents()
	{
		if (Events.AreaIsShrinking == null) Events.AreaIsShrinking = new ZoneEvents.FloatUnityEvent();
		if (Events.WaitingForShrinking == null) Events.WaitingForShrinking = new ZoneEvents.FloatUnityEvent();
		if (Events.AreaStartShrinking == null) Events.AreaStartShrinking = new ZoneEvents.FloatUnityEvent();
		if (Events.StartWaitingForShrinking == null)
			Events.StartWaitingForShrinking = new ZoneEvents.FloatUnityEvent();
		if (Events.EndOfLastStep == null) Events.EndOfLastStep = new ZoneEvents.FloatUnityEvent();
		_timer = new Timer(0, 0);
	}

	// Method for do setup circles/steps
	[ContextMenu("Setup Circles")]
	public void SetupCircles(int firstCircleId, int lastCircleId)
	{
		ZoneCircles.Clear();
		ZoneCircleTutorial firstCircle = null;
		// Choose one of first circles randomly
		if (FirstZoneCircle.Length > 0)
			firstCircle = FirstZoneCircle[firstCircleId];
		// Then reseting his y level to zone y level
		var startPos = Vector3.zero;
		if (firstCircle) startPos = firstCircle.transform.position;
		startPos.y = transform.position.y;

		ZoneCircleTutorial lastCircle = null;
		// Choose one of first circles randomly
		if (LastZoneCircle.Length > 0) lastCircle = LastZoneCircle[lastCircleId];
		Vector3 tm = lastCircle.transform.position;
		tm.y = 5;
		//DoorGameTutorial.door.escape.transform.position = tm;
		// Then reseting his y level to zone y level
		var endPos = Vector3.zero;
		if (lastCircle) endPos = lastCircle.transform.position;
		endPos.y = transform.position.y;
		//////////////////////////////////////////////
		// Adding chosen first circle to steps/circles list
		if (firstCircle)
			ZoneCircles.Add(new ZoneCircleInfo
			{
				PositionAndRadius = new ZoneCircleBase
				{
					Position = firstCircle.transform.position,
					Radius = firstCircle.Radius
				}
			});
		// Then we do setup all other steps/circles between first and last
		var step = 1f / StepsToEnd;                 // Calculate step delta value
		for (var i = 1; i < StepsToEnd - 1; i++) // Then do cycle from second circle/step index to last index -1
		{
			var sizeCurve =
				ZoneCirclesSizeCurve
					.Evaluate(step * i); // Using size curve for calculating size of circles/steps
			var newPos =
				Vector3.Lerp(startPos, endPos,
							   sizeCurve);  // By this evaluated curve getting new circle position
			newPos.y = transform.position.y; // Also resetting y level to zone y level
			var newRadius =
				Mathf.Lerp(firstCircle.Radius, lastCircle.Radius,
							 sizeCurve); // By this evaluated curve getting new circle radius
										 // Then adding new circle/step to list
			var circleBase = new ZoneCircleBase { Position = newPos, Radius = newRadius };
			if (ZoneCircles != null) ZoneCircles.Add(new ZoneCircleInfo { PositionAndRadius = circleBase });
		}

		//////////////////////////////////////////////
		// Adding last circle/step to list
		if (ZoneCircles != null && lastCircle)
			ZoneCircles.Add(new ZoneCircleInfo
			{
				PositionAndRadius = new ZoneCircleBase
				{
					Position = lastCircle.transform.position,
					Radius = lastCircle.Radius
				}
			});
		//////////////////////////////////////////////
		// When all circles/steps will be setup , doing setup shrinking time and delay for all circles/steps
		if (ZoneCircles != null)
		{
			if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene3)
			{
				for (var i = 0; i < ZoneCircles.Count; i++)
				{
					if (i == 0)
					{
						ZoneCircles[i].ShrinkingTime = 10;
						ZoneCircles[i].StartDelay = 10;
					}
					else if (i == 1)
					{
						ZoneCircles[i].ShrinkingTime = 10;
						ZoneCircles[i].StartDelay = 10;
					}
					else if (i == 2)
					{
						ZoneCircles[i].ShrinkingTime = 10;
						ZoneCircles[i].StartDelay = 10;
					}
					else if (i == 3)
					{
						ZoneCircles[i].ShrinkingTime = 11;
						ZoneCircles[i].StartDelay = 12;
					}
					else if (i == 4)
					{
						ZoneCircles[i].ShrinkingTime = 11;
						ZoneCircles[i].StartDelay = 12;
					}
				}
			}
			else
			{
				for (var i = 0; i < ZoneCircles.Count; i++)
				{
					if (i == 0)
					{
						ZoneCircles[i].ShrinkingTime = 10;
						ZoneCircles[i].StartDelay = 10;
					}
					else if (i == 1)
					{
						ZoneCircles[i].ShrinkingTime = 14;
						ZoneCircles[i].StartDelay = 13;
					}
					else if (i == 2)
					{
						ZoneCircles[i].ShrinkingTime = 12;
						ZoneCircles[i].StartDelay = 11;
					}
				}
			}
		}

		// Then doing setup current and next safe zones
		if (ZoneCircles != null)
		{

			CurrentSafeZone.Position = ZoneCircles[0].PositionAndRadius.Position;
			CurrentSafeZone.Radius = ZoneCircles[0].PositionAndRadius.Radius;

			NextSafeZone.Position = ZoneCircles[1].PositionAndRadius.Position;
			NextSafeZone.Radius = ZoneCircles[1].PositionAndRadius.Radius;
		}

	}
	void ChangeRadiusCmd(Vector3 position, float radius, float timeToChange, int curStep)
	{
		CurStep = curStep;
		StartCoroutine(ChangeRadius(position, radius, timeToChange));
	}
	// Method for changing radius and position of current circle in shrinking period
	public IEnumerator ChangeRadius(Vector3 position, float radius, float timeToChange)
	{

		var currentPos = CurrentSafeZone.Position;
		var startRadius = 0f;
		var currentRadius = 0f;

		startRadius = CurrentSafeZone.Radius;

		// Calculating time for shrinking while
		_timer.CurrentTime = Time.time;
		_timer.EndTime = _timer.CurrentTime + timeToChange;

		// Invoke area start shrinking event
		if (Events.AreaStartShrinking != null) Events.AreaStartShrinking.Invoke(_timer);

		while (_timer.CurrentTime < _timer.EndTime)
		{
			_timer.CurrentTime = Time.time;
			var alpha = 1 - (_timer.EndTime - _timer.CurrentTime) /
						timeToChange; // Calculate alpha value for change radius and position

			currentRadius = Mathf.Lerp(startRadius, radius, alpha); // Calculate new radius by this alpha

			CurrentSafeZone.Position =
				Vector3.Lerp(currentPos, position, alpha); // Calculate new position by this alpha and set
			CurrentSafeZone.Radius = currentRadius;             // set new radius
																// Invoke area is shrinking event
			if (Events.AreaIsShrinking != null) Events.AreaIsShrinking.Invoke(_timer);

			yield return null;
		}

		CurStep++; // Move to next step index
		Debug.Log("CurStep : " + CurStep);
		// If it was not last step/circle go to next step/circle else invoke end of last step event
		if (CurStep != StepsToEnd - 1)
		{
			WaitForNewSafeZoneCmd(ZoneCircles[CurStep].StartDelay, CurStep);
			//StartCoroutine(WaitForNewSafeZone(ZoneCircles[CurStep].StartDelay));
		}
		else
		{
			if (Events.EndOfLastStep != null) Events.EndOfLastStep.Invoke(_timer);
		}

	}
	void WaitForNewSafeZoneCmd(float timeToMove, int curStep)
	{
		CurStep = curStep;
		StartCoroutine(WaitForNewSafeZone(timeToMove));
	}
	// Method for waiting between shrinking of steps/circles
	public IEnumerator WaitForNewSafeZone(float timeToMove)
	{
		if (ZoneCircles == null) yield break;

		// Setup new current safe zone
		if (CurStep == -1 || CurStep == 0)
		{
			CurrentSafeZone.Position = ZoneCircles[0].PositionAndRadius.Position;
			CurrentSafeZone.Radius = ZoneCircles[0].PositionAndRadius.Radius;
		}

		_isShrinking = false; // Set IsShrinking property to false because of we waiting
		var step = Mathf.Clamp(CurStep + 1, 0, StepsToEnd - 1);
		// Calculate time for waiting
		_timer.CurrentTime = Time.time;
		_timer.EndTime = _timer.CurrentTime + timeToMove;
		// Set new next safe circle
		NextSafeZone.Position = ZoneCircles[step].PositionAndRadius.Position;
		NextSafeZone.Radius = ZoneCircles[step].PositionAndRadius.Radius;

		//Invoke start waiting for shrinking event
		if (Events.StartWaitingForShrinking != null) Events.StartWaitingForShrinking.Invoke(_timer);

		// Waiting before do shrinking
		while (_timer.CurrentTime < _timer.EndTime)
		{
			_timer.CurrentTime = Time.time;
			//Invoke waiting for shrinking event
			if (Events.WaitingForShrinking != null) Events.WaitingForShrinking.Invoke(_timer);
			yield return null;
		}

		// On end of waiting start shrinking
		ChangeRadiusCmd( ZoneCircles[step].PositionAndRadius.Position,
						ZoneCircles[step].PositionAndRadius.Radius,
						ZoneCircles[CurStep].ShrinkingTime,
						CurStep);
		_isShrinking = true; // Setup IsShrinking property to true because of circle start shrinking
	}
}

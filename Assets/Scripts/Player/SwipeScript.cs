using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Week 2 - Mobile Input</para>
/// Example of Swiping to add Torque to a cube. Set the Angular drag on the Rigidbody to 1.
/// <author>Rocco Briganti</author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SwipeScript : MonoBehaviour
{
	private const float MIN_SWIPE_LENGTH = 300f;
	private const float MAX_SWIPE_TIME = 0.35f;

	private Vector3 _mouseStartPos;
	private float _elapsedTime;
	private bool _startTimer;

	public PlayerMove player;

	void Start()
	{
		_mouseStartPos = new Vector3(0f, 0f, 0f);
		_elapsedTime = 0f;
		player = GetComponent<PlayerMove>();
	}

	void Update()
	{
#if UNITY_EDITOR
		SimulateMouseSwipe();
#endif

#if UNITY_ANDROID || UNITY_IOS
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				_startTimer = true;
				_elapsedTime = 0f;
			}

			if (touch.phase == TouchPhase.Ended && _elapsedTime < MAX_SWIPE_TIME)
			{
                if (GameManager.Instance.GetEnableControls())
                    Swipe(touch.deltaPosition);
			}
		}
#endif

		if (_startTimer)
		{
			if (_elapsedTime < MAX_SWIPE_TIME)
			{
				_elapsedTime += Time.deltaTime;
			}

			if (_elapsedTime >= MAX_SWIPE_TIME)
			{
				_startTimer = false;
			}
		}
	}

	private void SimulateMouseSwipe()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_mouseStartPos = Input.mousePosition;
			_startTimer = true;
			_elapsedTime = 0f;
		}

		if (Input.GetMouseButtonUp(0) && _elapsedTime < MAX_SWIPE_TIME)
		{
			_startTimer = false;

			Vector3 mouseEndPos = Input.mousePosition;
			Vector3 direction = (mouseEndPos - _mouseStartPos);

			if (GameManager.Instance.GetEnableControls())
				Swipe(direction);
		}
	}

	private void Swipe(Vector3 pDirection)
	{
		if (pDirection.magnitude > MIN_SWIPE_LENGTH)
		{
			if (pDirection.y > 0)
				player.Jump();
			else if (pDirection.y < 0)
			{
                if (GameManager.Instance.state == GameManager.GameState.Game)
                    GameManager.Instance.SetGameState(GameManager.GameState.Pause);
                else if (GameManager.Instance.state == GameManager.GameState.Pause)
                    GameManager.Instance.ResumeGame();
            }
		}
	}
}

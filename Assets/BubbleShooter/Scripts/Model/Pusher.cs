using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour
{
    GameManager _gameManager;
    Counter _counter;
    float _timeToPushDown;
    bool _pauseTimer;
    float _originalHeight;

    // Use this for initialization
    void Start()
    {
        _counter = GetComponent<Counter>();
        _originalHeight = transform.localPosition.y;
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_pauseTimer)
        {
            updateTimer();
        }
    }

    public void InitPusher(GameManager manager)
    {
        _gameManager = manager;
        _timeToPushDown = _gameManager.GetLevelProfile().GetTimeToPushDown();
    }

    void updateTimer()
    {
        if (_counter.CurrentState == Counter.CounterState.STOP)
        {
            _counter.StartTimerUpdateSeconds(_timeToPushDown, () =>
                {
                    (float,float) HDownAndSpeed = _gameManager.OnPushDown();
                    transform.localPosition -= new Vector3(0, HDownAndSpeed.Item1 * Time.deltaTime * HDownAndSpeed.Item2, 0);
                }, (int seconds) =>
                {
                    if (seconds <= 3)
                    {
                        _gameManager.OnWarning();
                    }
                });
        }
    }

    public void OnPushUpFixed()
    {
        float heightUp = _gameManager.OnPushUp();
        transform.localPosition += new Vector3(0, heightUp, 0);
    } 

    public void OnPause()
    {
        _pauseTimer = true;
        _counter.StopTimer();
    }

    public void OnResume()
    {
        _pauseTimer = false;
    }

    public void Reset()
    {
        transform.localPosition = new Vector3(0, _originalHeight, 0);
    }
}

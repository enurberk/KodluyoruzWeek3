using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    //[Header ("Input Values")] //bu atttributes a başlık eklememizi sağlar
    [HideInInspector] //ınspectorda gizlememizi sağlar
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    private float _touchVerticalStartPos;
    private float _touchHorizontalStartPos;
    private float _touchShootingValue = 0.1f;
   
    private float _swipeCheckStartPosX;
    private float _swipeCheckStartPosY;

    private float _swipeDistance = (float)Screen.width/4;
    private float _swipeMaxTime = 0.5f;
    private float _swipeTime = 0f;
    private bool isSwipe;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public static InputManager Instance()
    {
        return _instance;
    }

    // Update is called once per frame
    void Update()
    {
//#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
//#else
        if(Input.touchCount > 0)
        {
            foreach(var touch in Input.touches)
            {
                var touchPos = touch.position;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchVerticalStartPos = touch.position.y;
                        _touchHorizontalStartPos = touch.position.x;
                        _swipeCheckStartPosX = touch.position.x;
                        _swipeCheckStartPosY = touch.position.y;
                        break;
                    case TouchPhase.Moved:
                        CalculateInputOutput(touch.position);
                        if (!isSwipe)
                        {
                            _swipeTime += Time.deltaTime;
                            CheckForSwipe(touch.position, _swipeTime);
                        }                        
                        break;
                    case TouchPhase.Stationary:
                        CalculateInputOutput(touch.position);
                        break;
                    case TouchPhase.Ended:
                        isSwipe = false;
                        _touchVerticalStartPos = 0;
                        _touchHorizontalStartPos = 0;
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;
                }
            }
        }
//#endif
    }
    private void CheckForSwipe(Vector2 touchposition, float swipeTime)
    {
        if (swipeTime > _swipeMaxTime) return;
        if(Mathf.Abs(touchposition.x - _swipeCheckStartPosX) > _swipeDistance && touchposition.x > _swipeCheckStartPosX) //sağa swipe
        {
            isSwipe = true;
            Debug.Log("We have Horizontal right swipe.");
            _swipeTime = 0;
            _swipeCheckStartPosX = 0;
        }
        else if (Mathf.Abs(touchposition.x - _swipeCheckStartPosX) > _swipeDistance && touchposition.x < _swipeCheckStartPosX) //sola swipe
        {
            isSwipe = true;
            Debug.Log("We have Horizontal left swipe.");
            _swipeTime = 0;
            _swipeCheckStartPosX = 0;
        }
        else if (Mathf.Abs(touchposition.y - _swipeCheckStartPosY) > _swipeDistance && touchposition.y > _swipeCheckStartPosY) // yukarı swipe
        {
            isSwipe = true;
            Debug.Log("We have Vertical up swipe.");
            _swipeTime = 0;
            _swipeCheckStartPosY = 0;
        }
        else if (Mathf.Abs(touchposition.y - _swipeCheckStartPosY) > _swipeDistance && touchposition.y < _swipeCheckStartPosY) // aşağı swipe
        {
            isSwipe = true;
            Debug.Log("We have Vertical down swipe.");
            _swipeTime = 0;
            _swipeCheckStartPosY = 0;
        }
    }

    private void CalculateInputOutput(Vector2 touchposition)
    {
        verticalInput = Mathf.Clamp((touchposition.y - _touchVerticalStartPos) * _touchShootingValue, -1f, 1f);
        horizontalInput = Mathf.Clamp((touchposition.x - _touchHorizontalStartPos) * _touchShootingValue, -1f, 1f);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] [Range(0.001f, 1.0f)] public float PlayerPriority = 1.0f;
    private PlayerController _player;
    [SerializeField] [Range(0.0f, 1.0f)] public float CursorPriority = 1.0f;
    private GameObject _cursor;
    [SerializeField] [Range(0.0f, 1.0f)] public float OtherTargetPriority = 1.0f;
    private GameObject _target;
    [SerializeField] [Range(0.001f, 10.0f)] public float SmoothTime = 0.5f;
    private Vector3 _currentVelocityFollow;

    [Header("Effects Settings")]
    [SerializeField] public bool UseScreenShake = true;
    [SerializeField] [Range(0.001f, 10.0f)] public float ScreenShakeTime = 0.5f;
    [SerializeField] [Range(1, 100)] public int ScreenShakeRevolutions = 8;
    [SerializeField] [Range(0.0f, 64.0f)] public float ScreenShakeIntensity = 8.0f; //in pixels
    private const int k_pixelsPerUnit = 16;
    private float _moveTimer;
    private Vector3 _currentVelocityMoveCamera;
    private Camera _camera;
    [SerializeField] [Range(0.0f, 10.0f)] public float TimeBeforeLookingAtMirror = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] public float TimeToMirror = 2.5f;
    [SerializeField] [Range(0.0f, 10.0f)] public float TimeLookingAtMirror = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] public float TimeFromMirror = 2.5f;
    [SerializeField] [Range(0.0f, 10.0f)] public float TimeAfterLookingAtMirror = 5.0f;

    private void Start()
    {
        //get references
        _player = DataManager.Instance.PlayerDataObject.Player;
        _cursor = DataManager.Instance.PlayerDataObject.CursorObject;
        _target = null;
        _camera = Camera.main;

        //init current velocity (for smooth damp)
        _currentVelocityFollow = Vector3.zero;

        //init camera move timer
        _moveTimer = 0.0f;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        //get target position
        Vector3 targetPosition;
        if (_target == null)
        {
            targetPosition = (new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z) * PlayerPriority + new Vector3(_cursor.transform.position.x, _cursor.transform.position.y, transform.position.z) * CursorPriority) / (PlayerPriority + CursorPriority);
        }
        else
        {
            targetPosition = (new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z) * PlayerPriority + new Vector3(_cursor.transform.position.x, _cursor.transform.position.y, transform.position.z) * CursorPriority + new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z) * OtherTargetPriority) / (PlayerPriority + CursorPriority + OtherTargetPriority);
        }

        //smooth damp to target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocityFollow, SmoothTime);
    }

    public void FollowTarget(GameObject target)
    {
        _target = target;
    }

    public void UnfollowTarget()
    {
        _target = null;
    }

    public void ScreenShake()
    {
        StartCoroutine(ShakeScreen());
    }

    private IEnumerator ShakeScreen()
    {
        for (int i = 0; i < ScreenShakeRevolutions; i++)
        {
            float moveTime = ScreenShakeTime / ScreenShakeRevolutions;

            Vector2 point = Random.insideUnitCircle.normalized * ScreenShakeIntensity / k_pixelsPerUnit * (ScreenShakeRevolutions - i) / ScreenShakeRevolutions;

            StartCoroutine(MoveCamera(new Vector3(point.x, point.y, _camera.transform.position.z), moveTime));

            yield return new WaitForSeconds(moveTime);
        }
    }

    private IEnumerator MoveCamera(Vector3 localPosition, float time)
    {
        _moveTimer = time;

        while (_moveTimer > 0.0f)
        {
            _moveTimer -= Time.deltaTime;

            _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, localPosition, ref _currentVelocityMoveCamera, time);

            yield return null;
        }
    }

    public void LookAtMirror(Vector3 mirrorPosition)
    {
        StartCoroutine(MoveCameraToFromMirror(mirrorPosition - transform.position));
    }

    private IEnumerator MoveCameraToFromMirror(Vector3 localPosition)
    {
        yield return new WaitForSecondsRealtime(TimeBeforeLookingAtMirror);

        _moveTimer = TimeToMirror;
        while (_moveTimer > 0.0f)
        {
            _moveTimer -= Time.unscaledDeltaTime;

            _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, localPosition, ref _currentVelocityMoveCamera, TimeToMirror, Mathf.Infinity, Time.unscaledDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(TimeLookingAtMirror);

        _moveTimer = TimeFromMirror;
        while (_moveTimer > 0.0f)
        {
            _moveTimer -= Time.unscaledDeltaTime;

            _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, Vector3.zero, ref _currentVelocityMoveCamera, TimeFromMirror, Mathf.Infinity, Time.unscaledDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(TimeAfterLookingAtMirror);
    }
}

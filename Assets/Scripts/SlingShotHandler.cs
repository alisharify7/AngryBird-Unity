using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SlingShotHandler : MonoBehaviour
{
    [Header("LineRenderer")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transform Start Position")]
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;
    private bool _clickWithInArea = false;

    private Vector2 _slingShotLinePosition;

    [Header("Sling Shot Stat")]
    [SerializeField] private float _maxDistance = 4.5f;
    [SerializeField] private float _shotForce =  15.0f;
    [SerializeField] private float _timeBetweenBirdReSpawnes =  2.0f;


    [Header("Bird")]
    [SerializeField] private AngieBird _angieBirdPrefab;
    private AngieBird _spawnedAngieBird;

    [SerializeField] private float _angieBirdPositionOffset = 0.275f;
    private bool _birdOnSlingShot = false; // flag 
    private Vector2 _direction;
    private Vector2 _directionNormalized;

 
    private void Awake()
    {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;
        SpawnAngieBird();

    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.IsWithInSlingShowArea())
        {
            _clickWithInArea = true;
        }
        if (Mouse.current.leftButton.isPressed && _clickWithInArea && _birdOnSlingShot)
        {
            DrawSlingShot();
            PositionAndRotateAngieBird();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame && _birdOnSlingShot)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                _clickWithInArea = false;
                _spawnedAngieBird.LunchBird(_direction, _shotForce);
                GameManager.instance.UseShot();
                _birdOnSlingShot = false;
                SetLines(_centerPosition.position);
                if (GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnAngieBirdAfterTime());
                }
            }
        }
    }
 
    #region SlingShow Method
    private void DrawSlingShot()
    {

        Vector3 touchPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _slingShotLinePosition = _centerPosition.position +  Vector3.ClampMagnitude(touchPoint - _centerPosition.position, _maxDistance);
        SetLines(_slingShotLinePosition);
        _direction = (Vector2) _centerPosition.position - _slingShotLinePosition;
        _directionNormalized = _direction.normalized;
  
    }

    private void SetLines(Vector2 position) 
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }
        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }
    #endregion

    #region Angie Bird 
    private void SpawnAngieBird()
    {
        SetLines(_idlePosition.position);
        Vector2 _dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + _dir * _angieBirdPositionOffset;
        _spawnedAngieBird = Instantiate(_angieBirdPrefab, spawnPosition, Quaternion.identity);
        //_spawnedAngieBird.transform.right = _dir;

        _birdOnSlingShot = true;
    }
    private void PositionAndRotateAngieBird()
    {
        _spawnedAngieBird.transform.position = _slingShotLinePosition + _directionNormalized * _angieBirdPositionOffset;
        //_spawnedAngieBird.transform.right = _direction;
    }

    private IEnumerator SpawnAngieBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenBirdReSpawnes);
        SpawnAngieBird();
    }

    #endregion


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingShotAreaMask;
    public bool IsWithInSlingShowArea()
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Physics2D.OverlapPoint(WorldPosition, _slingShotAreaMask)) // only check if collider with specific layermask is overlap return true
        {
            return true;
        }
        return false;
    }
}

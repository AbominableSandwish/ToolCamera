using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Door
{
    private Vector2 position;
    private Vector2 size;
    private Room roomParent;
    private Room roomChild;

    public enum stateDoor
    {
        Open,
        Lock,
        Event
    }

    private stateDoor state = stateDoor.Lock;

    private List<Event> events;

    public Door(Vector2 _position)
    {
        position = _position;
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public stateDoor GetStateDoor()
    {
        return state;
    }

    public void SetStateDoor(stateDoor _state)
    {
        state = _state;
    }

}
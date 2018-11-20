using UnityEngine;
using UnityEditor;

public class Event
{
    public enum stateEvent
    {
        Cinematik,
        Screenshake
    }

    private stateEvent typeEvent;

    private Vector2 position;
    private Vector2 size;

    public Event(Vector2 _position, Vector2 _size, stateEvent _type)
    {
        position = _position;
        size = _size;
    }

    public Vector2 GetPosition()
    {
        return this.position;
    }

    public Vector2 GetSize()
    {
        return this.size;
    }

}
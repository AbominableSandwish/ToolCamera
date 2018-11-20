using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Room
{
    private Vector2 position;
    private Vector2 size;
    private List<Door> doors;


    public Room(int _x, int _y, int _width, int _height)
    {
        this.position.x = _x;
        this.position.y = _y;
        this.size.x = _width;
        this.size.y = _height;
    }

    public Room(Vector2 _position, Vector2 _size)
    {
        this.position = _position;
        this.size = _size;
    }

    public void SetX(float _x)
    {
        this.position.x = _x;
    }

    public void SetPosition( Vector2 _position)
    {
        this.position = _position;
    }

    public void SetY(float _y)
    {
        this.position.y = _y;
    }

    public void SetHeight(float _height)
    {
        this.size.y = _height;
    }

    public void SetWidth(float _width)
    {
        this.size.x = _width;
    }

    public float GetX()
    {
        return this.position.x;
    }


    public Vector2 GetPosition()
    {
        return this.position;
    }

    public Vector2 GetSize()
    {
        return this.size;
    }

    public float GetY()
    {
        return this.position.y;
    }

    public float GetHeight()
    {
        return this.size.y;
    }

    public float GetWidth()
    {
        return this.size.x;
    }

    public void AddDoor(Door _door)
    {
        if (doors == null)
            doors = new List<Door>();

        doors.Add(_door);
        Debug.Log(_door.GetPosition());
    }

    public List<Door> GetDoors()
    {
        return doors;
    }

    public bool HaveDoor()
    {
        if (doors == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

 

    private void OnDrawGizmos()
    {
        Vector2 BottomLeft = new Vector2(this.position.x, this.position.y);
        Vector2 TopRight = BottomLeft + new Vector2(this.size.x, this.size.y);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(BottomLeft, TopRight);
    }

}
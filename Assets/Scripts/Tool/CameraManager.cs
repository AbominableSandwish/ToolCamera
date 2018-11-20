using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform target;
    private Transform transform;
    private List<Transform> monstersIsAttacking;
    private Vector3 Position_BottomLeft;

    private List<Room> rooms;
    private Vector2 sizeAera;
    private Vector2 sizeBox = new Vector2(6, 5);

    private float smoothSpeed = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.gameObject.transform;
        monstersIsAttacking = new List<Transform>();
    }

    public enum stateCamera
    {
        Find,
        MoveTo,
    }

    public stateCamera state = stateCamera.Find;

    Vector2 newPositionCamera = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        if (monstersIsAttacking.Count == 0)
        {
            Vector2 position = transform.position;
            
            Vector2 positionTarget= Vector2.zero;
            //Limit camera X

            if (target != null)
            {
                if ((position.x - sizeBox.x >= Position_BottomLeft.x || (target.position.x - position.x) > 0) &&
                    (position.x + sizeBox.x <= Position_BottomLeft.x + sizeAera.x ||
                     (target.position.x - position.x) < 0))
                {
                    positionTarget = new Vector2(target.position.x, 0);
                    position = new Vector2(positionTarget.x, position.y);
                    transform.position = (Vector3) position - Vector3.forward * 10;


                }

                //Limit camera Y
                if ((position.y - sizeBox.y >= Position_BottomLeft.y || (target.position.y - position.y) > 0) &&
                    (position.y + sizeBox.y <= Position_BottomLeft.y + sizeAera.y ||
                     (target.position.y - position.y) < 0))
                {
                    positionTarget = new Vector2(0, target.position.y);
                    transform.position = new Vector3(position.x, positionTarget.y, -10);
                }
            }

        }
        else
        {
            switch (state)
            {
                case stateCamera.Find:

                    newPositionCamera = Vector2.zero;
                    int i = 0;

                    foreach (var monsterIsAttacking in monstersIsAttacking)
                    {
                        newPositionCamera += Vector2.Lerp(target.position, monsterIsAttacking.position, 0.5f);
                        i++;
                    }

                    newPositionCamera /= i;
                    state = stateCamera.MoveTo;
                    break;

                case stateCamera.MoveTo:
                    Vector3 position = transform.position;
                    transform.position = Vector3.Lerp(position, (Vector3)newPositionCamera - (Vector3.forward * 10), smoothSpeed);
                    //Debug.Log((newPositionCamera - (Vector2)position).magnitude);
                    if ((newPositionCamera - (Vector2)position).magnitude <= smoothSpeed)
                    {
                        state = stateCamera.Find;
                    }
                    break;
            }
        }
    }

    public void AddMonsterIsAttacking(Transform monster)
    {
        monstersIsAttacking.Add(monster);
    }

    public void RemoveMonsterIsAttacking(Transform monster)
    {
        for (int i= 0; i < monstersIsAttacking.Count; i++)
        {
            if (monstersIsAttacking[i] == monster)
            {
                monstersIsAttacking.RemoveAt(i);
            }
        }
    }

    public void AddPlayer(Transform player)
    {
        target = player;
    }


    private void OnDrawGizmos()
    {
        Room roomSelecting = new Room(0,0,0,0);
        if (newPositionCamera != Vector2.zero)
        {
            Gizmos.DrawSphere(newPositionCamera, 0.2f);
        }

        if (target != null)
        {
            if (transform != null)
            {
                Vector3 position = transform.position;
                //Show Box area piu
                Gizmos.color = Color.red;

                Gizmos.DrawLine(new Vector3(position.x - sizeBox.x, position.y + sizeBox.y, position.z),
                    new Vector3(position.x - sizeBox.x, position.y - sizeBox.y, position.z));
                Gizmos.DrawLine(new Vector3(position.x + sizeBox.x, position.y - sizeBox.y, position.z),
                    new Vector3(position.x + sizeBox.x, position.y + sizeBox.y, position.z));
                if ((position.x - sizeBox.x >= Position_BottomLeft.x || (target.position.x - position.x) > 0) &&
                    (position.x + sizeBox.x <= Position_BottomLeft.x + sizeAera.x ||
                     (target.position.x - position.x) < 0))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }


                Gizmos.DrawLine(new Vector3(position.x, position.y - sizeBox.y / 2, position.z),
                    new Vector3(position.x, position.y + sizeBox.y / 2, position.z));

                if ((position.y - sizeBox.y >= Position_BottomLeft.y || (target.position.y - position.y) > 0) &&
                    (position.y + sizeBox.y <= Position_BottomLeft.y + sizeAera.y ||
                     (target.position.y - position.y) < 0))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawLine(new Vector3(position.x - sizeBox.x/2, position.y, position.z),
                    new Vector3(position.x + sizeBox.x / 2, position.y, position.z));
            }
        }

        //Size Box Aera Camera
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Position_BottomLeft, new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y, Position_BottomLeft.z));
        Gizmos.DrawLine(Position_BottomLeft, new Vector3(Position_BottomLeft.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z));
        Gizmos.DrawLine(new Vector3(Position_BottomLeft.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z), new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z));
        Gizmos.DrawLine(new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y + sizeAera.y, Position_BottomLeft.z), new Vector3(Position_BottomLeft.x + sizeAera.x, Position_BottomLeft.y, Position_BottomLeft.z));


        //Navigation 2D
        if (rooms != null)
        {
            Vector2 position;
            Vector2 size;
            int i = 0;
            foreach (var room in rooms)
            {
                Gizmos.color = Color.gray;
                Debug.Log(roomIsSelecting);
                if (i == roomIsSelecting)
                {
                    roomSelecting = room;
                    Gizmos.color = Color.white;
                }


                position = new Vector2(room.GetX(), room.GetY());
                size = new Vector2(room.GetWidth(), room.GetHeight());
                Vector2 bottomLeft1 = position;
                Gizmos.DrawLine(bottomLeft1, bottomLeft1 + size * Vector2.up);
                Gizmos.DrawLine(bottomLeft1, bottomLeft1 + size * Vector2.right);
                Gizmos.DrawLine(bottomLeft1 + size * Vector2.up, bottomLeft1 + size);
                Gizmos.DrawLine(bottomLeft1 + size * Vector2.right, bottomLeft1 + size);
                Gizmos.DrawCube(bottomLeft1 + size * 0.5f, new Vector3(size.x * 0.95f, size.y * 0.95f, 1));
                i++;


            }

            foreach (var room in rooms)
            {
                if (room.HaveDoor())
                {
                    foreach (var door in room.GetDoors())
                    {
                        switch (door.GetStateDoor())
                        {
                            case Door.stateDoor.Open:
                                Gizmos.color = Color.green;
                                break;

                            case Door.stateDoor.Lock:
                                Gizmos.color = Color.red;
                                break;

                            case Door.stateDoor.Event:
                                Gizmos.color = Color.red;
                                break;
                        }

                        Gizmos.DrawCube((Vector3) door.GetPosition(), Vector3.one * 20);
                    }
                }
            }

        }

        if (events != null)
        {
            Vector2 position;
            Vector2 size;

            foreach (var cevent in events)
            {
                Gizmos.color = Color.blue;
                position = cevent.GetPosition();
                size = cevent.GetSize();
                Vector2 bottomLeft = position;
                Gizmos.DrawCube(bottomLeft + size * 0.5f, new Vector3(size.x * 0.5f, size.y * 0.5f, 1));
            }
        }
    }

    private int roomIsSelecting = 0;
    private int DoorIsSelecting = 0;

    //Functions Events
    #region RegionEvent
    private List<Event> events;

    public void AddEvent(Event _event)
    {
        if (events == null)
            events = new List<Event>();

        events.Add(_event);
        Debug.Log(events.Count);
    }

    public void EraseEvent(Event _event)
    {
        events.Remove(_event);
    }


    #endregion


    //Functions Door
    #region RegionDoor



    #endregion


    //Functions Room
    #region RegionRoom
    //Erase Room
    public void EraseRoom()
    {
        rooms.Remove(rooms[roomIsSelecting]);
        if (rooms.Count == 0)
            rooms = null;
    }

    //action int :1 go_up ,-1 go_down
    public Room ChangeRoomIsSelected(int action)
    {
        if (action == 1)
        {
            roomIsSelecting++;
        }

        if (action == -1)
        {
            roomIsSelecting--;
        }

        Room roomSelected = new Room(0, 0, 0, 0);
        int i = 0;
        foreach (var area in rooms)
        {
            if (i == roomIsSelecting)
            {
                roomSelected = area;
            }

            i++;
        }
        return roomSelected;
    }

    public void SetRoom(int _id, Room new_area)
    {
        int i = 0;

        foreach (var room in rooms)
        {
            Debug.Log(_id);
            Debug.Log(_id == i);
            if (_id == i)
            {
                Debug.Log(new_area.GetPosition());
                room.SetPosition(new_area.GetPosition());
                room.SetWidth(new_area.GetWidth());
                room.SetHeight(new_area.GetHeight());
                return;
            }
            i++;
        }
        return;
    }

    public List<Room> GetRooms()
    {
        return rooms;
    }

    public int GetCountRooms()
    {
        if (rooms == null)
        {
            return 0;
        }
        else
        {
            return rooms.Count;
        }
    }


    public int GetRoomSelecting()
    {
        return roomIsSelecting;
    }

    public void AddRoom(Room _room)
    {
        if (rooms == null)
            rooms = new List<Room>();

        roomIsSelecting = rooms.Count;

        rooms.Add(_room);
    }

    #endregion


}


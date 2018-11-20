using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleWindow : EditorWindow
{
    public Grid grid;
    private Vector2 positionArea;
    private Vector2 sizeArea;



    private GameObject camObj;
    private Camera cam;
    string events = "Events";

    private Room roomSelected;

    private bool showEvents = false;

    [MenuItem("Tools/Tool Camera")]
    static void InitializeWindow()
    {
        var window = GetWindow<ExampleWindow>("Tool Camera", true);
        window.titleContent.tooltip = "ExampleTOOL";
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private Vector2 new_size;

    public void OnGUI()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam.gameObject.GetComponent<Grid>() == null)
        {
            cam.gameObject.AddComponent<Grid>();
        }

        //GUILayout.Label("SelectCamera");
        //GUILayout.BeginArea(new Rect(10, 20, 100, 100));
        GUILayout.Label(events);
        if (GUILayout.Button("ADD"))
        {
            Debug.Log("Button Save Pressed, BUT don't work!");
            showEvents = !showEvents;
        }

        if (showEvents)
        {
            GUILayout.Label(events);
        }

        //positionArea = EditorGUILayout.Vector2Field("Position Area:", positionArea);
        sizeArea = EditorGUILayout.Vector2Field("Size Area  :", sizeArea);

          

        if (roomSelected != null)
        {
            //Function change Room Selected
            using (var horizontalScope = new GUILayout.HorizontalScope("box"))
            {
                GUILayout.Label(("Room °" + (cam.gameObject.GetComponent<CameraManager>().GetRoomSelecting() + 1))
                                .ToString() +
                                " by " + cam.gameObject.GetComponent<CameraManager>().GetRooms().Count.ToString());

                if (cam.gameObject.GetComponent<CameraManager>().GetRoomSelecting() > 0)
                {
                    if (GUILayout.Button(" < "))
                    {
                        roomSelected = cam.gameObject.GetComponent<CameraManager>().ChangeRoomIsSelected(-1);
                        new_size = roomSelected.GetSize();

                        RefreshSceneView();
                    }
                }


                if (cam.gameObject.GetComponent<CameraManager>().GetRooms().Count > 1)
                {
                    if (cam.gameObject.GetComponent<CameraManager>().GetRoomSelecting() <
                        cam.gameObject.GetComponent<CameraManager>().GetRooms().Count - 1)
                    {

                        if (GUILayout.Button(" > "))
                        {
                            roomSelected = cam.gameObject.GetComponent<CameraManager>().ChangeRoomIsSelected(1);
                            new_size = roomSelected.GetSize();

                            RefreshSceneView();
                        }
                    }
                }
            }

            //Function to create other Room
            using (var horizontalScope = new GUILayout.HorizontalScope("box"))
            {
                if (GUILayout.Button("Left"))
                {
                    roomSelected.AddDoor(
                        new Door(roomSelected.GetPosition() + (roomSelected.GetSize() * Vector2.up / 2)));

                    Room room = new Room(
                        new Vector2(roomSelected.GetPosition().x - sizeArea.x, roomSelected.GetPosition().y), sizeArea);
                    cam.gameObject.GetComponent<CameraManager>().AddRoom(room);
                    new_size = room.GetSize();
                    Debug.Log(cam.gameObject.GetComponent<CameraManager>().GetRooms().Count);

                    roomSelected = room;
                    RefreshSceneView();
                }

                if (GUILayout.Button("Right"))
                {
                    roomSelected.AddDoor(new Door(roomSelected.GetPosition() + roomSelected.GetSize() * Vector2.right +
                                                  (roomSelected.GetSize() * Vector2.up / 2)));

                    Room room = new Room(
                        new Vector2(roomSelected.GetPosition().x + roomSelected.GetSize().x,
                            roomSelected.GetPosition().y), sizeArea);
                    cam.gameObject.GetComponent<CameraManager>().AddRoom(room);
                    new_size = room.GetSize();
                    Debug.Log(cam.gameObject.GetComponent<CameraManager>().GetRooms().Count);

                    roomSelected = room;
                    RefreshSceneView();
                }

                if (GUILayout.Button("Up"))
                {
                    roomSelected.AddDoor(new Door(roomSelected.GetPosition() + roomSelected.GetSize() * Vector2.up +
                                                  (roomSelected.GetSize() * Vector2.right / 2)));

                    Room room = new Room(
                        new Vector2(roomSelected.GetPosition().x,
                            roomSelected.GetPosition().y + roomSelected.GetSize().y), sizeArea);
                    cam.gameObject.GetComponent<CameraManager>().AddRoom(room);
                    new_size = room.GetSize();
                    Debug.Log(cam.gameObject.GetComponent<CameraManager>().GetRooms().Count);

                    roomSelected = room;
                    RefreshSceneView();
                }


                if (GUILayout.Button("Down"))
                {
                    roomSelected.AddDoor(new Door(roomSelected.GetPosition() +
                                                  (roomSelected.GetSize() * Vector2.right / 2)));

                    Room room = new Room(
                        new Vector2(roomSelected.GetPosition().x,
                            roomSelected.GetPosition().y - roomSelected.GetSize().y), sizeArea);
                    cam.gameObject.GetComponent<CameraManager>().AddRoom(room);
                    new_size = room.GetSize();
                    Debug.Log(cam.gameObject.GetComponent<CameraManager>().GetRooms().Count);

                    roomSelected = room;
                    RefreshSceneView();
                }
            }

            GUILayout.Label("Room: ");
            new_size = EditorGUILayout.Vector2Field("Size", new_size);

            //Modification Room
            if (GUILayout.Button("Modif"))
            {
                cam.gameObject.GetComponent<CameraManager>().SetRoom(
                    cam.gameObject.GetComponent<CameraManager>().GetRoomSelecting(), new Room(roomSelected.GetPosition(), new_size));

                RefreshSceneView();
            }

            //Delete Room
            if (GUILayout.Button("Suppr"))
            {
                cam.gameObject.GetComponent<CameraManager>().EraseRoom();
                if (cam.gameObject.GetComponent<CameraManager>().GetCountRooms() != 0)//Don't Work
                {
                    roomSelected = cam.gameObject.GetComponent<CameraManager>().ChangeRoomIsSelected(-1);
                    new_size = roomSelected.GetSize();
                }
                else
                {
                    roomSelected = null;
                }

                RefreshSceneView();
            }

            if (GUILayout.Button("Select Door"))
            {
                cam.gameObject.GetComponent<CameraManager>().EraseRoom();
                if (cam.gameObject.GetComponent<CameraManager>().GetCountRooms() != 0)//Don't Work
                {
                    roomSelected = cam.gameObject.GetComponent<CameraManager>().ChangeRoomIsSelected(-1);
                    new_size = roomSelected.GetSize();
                }

                RefreshSceneView();
            }

            //Save DataBase
            if (GUILayout.Button("Save"))
            {
                Debug.Log("Button Save Pressed, BUT don't work!");
            }
        }
        else
        {
            if (GUILayout.Button("Create Main Room"))
            {
                Room room = new Room(Vector2.zero, sizeArea);
                cam.gameObject.GetComponent<CameraManager>().AddRoom(room);
                new_size = room.GetSize();
                Debug.Log(cam.gameObject.GetComponent<CameraManager>().GetRooms().Count);

                roomSelected = room;

                RefreshSceneView();
            }
        }
    }

    //Refresh Scene View
    private void RefreshSceneView()
    {
        EditorWindow view = EditorWindow.GetWindow<SceneView>();
        view.Repaint();
    }
}

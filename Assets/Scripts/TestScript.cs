using System;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int ellipseWidth = 75;
    private int ellipseHeight = 25;

    private int numberOfRooms = 150;

    private int roomMinWidth = 4;
    private int roomMaxWidth = 40;
    private int roomMinHeight = 3;
    private int roomMaxHeight = 20;

    private int bigRommThresholdWidth;
    private int bigRommThresholdHeight;

    private RectInt[] rects;
    public RectInt[] Rects => rects;

    private List<RectInt> rooms;
    public List<RectInt> Rooms => rooms;

    private enum Status { Creation, Seperation, Selection, Paths }
    Status status;

    private float time = 0;
    private int counter = 0;
    private bool done = false;

    private void Awake()
    {
        rects = new RectInt[numberOfRooms];
        rooms = new List<RectInt>(16);
    }

    private void Start()
    {
        SetStatus(Status.Creation);

        bigRommThresholdWidth = (int)(((roomMinWidth + roomMaxWidth) / 2) * 1.25f);
        bigRommThresholdHeight = (int)(((roomMinHeight + roomMaxHeight) / 2) * 1.25f);
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > 0.01f)
        {
            time = 0f;
            switch (status)
            {
                case Status.Creation:
                    RoomCreation(counter++);
                    break;
                case Status.Seperation:
                    RoomSeperation();
                    break;
                case Status.Selection:
                    RoomSelection(counter++);
                    break;
                case Status.Paths:
                    Restart();
                    break;
                default:
                    break;
            }
        }
    }

    private void RoomCreation(int i)
    {
        if (i < rects.Length)
        {
            Vector2Int pos = Helpers.PointInEllipse(ellipseHeight, ellipseWidth);
            int width = Helpers.NormalizedRandom(roomMinWidth, roomMaxWidth);
            int height = Helpers.NormalizedRandom(roomMinHeight, roomMaxHeight);
            rects[i] = new RectInt(pos, new Vector2Int(height, width));
        }
        else
        {
            SetStatus(Status.Seperation);
            counter = 0;
        }
    }

    private void RoomSeperation()
    {
        done = true;
        int count = rects.Length;
        for (int i = 0; i < count; i++)
        {
            Vector2 avgDirection = Vector2.zero;
            Vector2Int move;

            for (int j = 0; j < count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                float extendX = (rects[i].width + rects[j].width) * 0.5f;
                float extendY = (rects[i].height + rects[j].height) * 0.5f;

                float distanceX = Mathf.Abs(rects[i].center.x - rects[j].center.x);
                float distanceY = Mathf.Abs(rects[i].center.y - rects[j].center.y);

                if (distanceX - 1 < extendX && distanceY - 1 < extendY)
                {
                    Vector2 direction = rects[j].position - rects[i].position;
                    avgDirection += direction.normalized;
                    done = false;
                }
            }

            if (avgDirection == Vector2.zero)
            {
                continue;
            }

            avgDirection *= -1f;

            if (Mathf.Abs(avgDirection.x) >= Mathf.Abs(avgDirection.y))
            {
                move = new Vector2Int((int)(Mathf.Sign(avgDirection.x)), 0);
            }
            else
            {
                move = new Vector2Int(0, (int)(Mathf.Sign(avgDirection.y)));
            }
            rects[i].position += move;
        }

        if (done)
        {
            SetStatus(Status.Selection);
        }
    }

    private void RoomSelection(int i)
    {
        if (i < rects.Length)
        {
            if (rects[i].width >= 15 && rects[i].height >= 10)
            {
                rooms.Add(rects[i]);
            }
        }
        else
        {
            SetStatus(Status.Paths);
            counter = 0;
        }
    }

    private void SetStatus(Status s)
    {
        status = s;
        Debug.Log(status + " started");
    }

    private void Restart()
    {
        counter = 0;
        done = false;
        Array.Clear(rects, 0, rects.Length);
        rooms.Clear();
        SetStatus(Status.Creation);
    }
}

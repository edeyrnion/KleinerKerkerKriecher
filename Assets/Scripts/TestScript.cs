using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int ellipseWidth = 75;
    private int ellipseHeight = 25;

    private int numberOfRooms = 150;

    private int roomMinWidth = 9;
    private int roomMaxWidth = 26;
    private int roomMinHeight = 4;
    private int roomMaxHeight = 16;

    private RectInt[] rooms;

    private float time = 0;

    private void Start()
    {
        rooms = new RectInt[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector2Int pos = Helpers.PointInEllipse(ellipseHeight, ellipseWidth);
            int width = Helpers.NormalizedRandom(roomMinWidth, roomMaxWidth);
            int height = Helpers.NormalizedRandom(roomMinHeight, roomMaxHeight);

            rooms[i] = new RectInt(pos, new Vector2Int(height, width));
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 0.05f)
        {
            time = 0f;
            Seperation();
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            DrawRect(rooms[i]);
        }
    }

    private void Seperation()
    {
        int count = rooms.Length;
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

                float extendX = (rooms[i].width + rooms[j].width) * 0.5f;
                float extendY = (rooms[i].height + rooms[j].height) * 0.5f;

                float distanceX = Mathf.Abs(rooms[i].center.x - rooms[j].center.x);
                float distanceY = Mathf.Abs(rooms[i].center.y - rooms[j].center.y);

                if (distanceX < extendX && distanceY < extendY)
                {
                    Vector2 direction = rooms[j].position - rooms[i].position;
                    avgDirection += direction.normalized;
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
            rooms[i].position += move;
        }
    }

    private void DrawRect(RectInt r)
    {
        Vector3 p1 = new Vector3(r.xMin, 0, r.yMin);
        Vector3 p2 = new Vector3(r.xMin, 0, r.yMax);
        Vector3 p3 = new Vector3(r.xMax, 0, r.yMax);
        Vector3 p4 = new Vector3(r.xMax, 0, r.yMin);

        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p3);
        Debug.DrawLine(p3, p4);
        Debug.DrawLine(p4, p1);
    }
}

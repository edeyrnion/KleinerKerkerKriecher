using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int numberOfRooms = 100;
    private float radius = 100f;

    private GameObject[] rooms;

    private void Start()
    {
        rooms = new GameObject[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector2 p = Helpers.PointInCircle(radius);
            p = Helpers.RoundToGrid(p, 1);

            var x = (float)Helpers.NormalizedRandom(0, 11);
            var z = (float)Helpers.NormalizedRandom(0, 11);

            x = Helpers.RoundToGrid(x, 1);
            z = Helpers.RoundToGrid(z, 1);

            rooms[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rooms[i].transform.parent = gameObject.transform;
            rooms[i].transform.position = new Vector3(p.x, 0f, p.y);
            rooms[i].transform.localScale = new Vector3(x, 3f, z);
        }
    }
}

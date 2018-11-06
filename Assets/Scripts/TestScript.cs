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

    private void Start()
    {
        rooms = new RectInt[numberOfRooms];

        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector2Int pos = Helpers.PointInEllipse(ellipseHeight, ellipseWidth);
            int width = Helpers.NormalizedRandom(roomMinWidth, roomMaxWidth);
            int height = Helpers.NormalizedRandom(roomMinHeight, roomMaxHeight);

            rooms[i] = new RectInt(pos, new Vector2Int(width, height));
        }
    }

    public void Steer()
    {
        Vector3 avgDirection = Vector3.zero;
        int numberOfCloseVehicles = 0;
        for (int i = 0; i < vehicles.Count; i++)
        {
            Vector3 direction = vehicles[i].transform.position - transform.position;
            float sqrDistance = direction.sqrMagnitude;
            if (sqrDistance < (MaxSeparationDistance * MaxSeparationDistance) && sqrDistance > 0E-5f)
            {
                avgDirection += direction.normalized;
                numberOfCloseVehicles++;
            }
        }
        if (numberOfCloseVehicles == 0) { return; }
        avgDirection /= numberOfCloseVehicles;
        avgDirection = new Vector3(avgDirection.x, 0f, avgDirection.z);
        Vector3 desired = avgDirection * (-1) * DesiredSpeed;
        Vector3 steeringForce = Vector3.ClampMagnitude(desired - vehicle.Velocity, MaxForce);
        vehicle.ApplyForce(steeringForce);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DungeonRenderer : MonoBehaviour
{
    static Material mat;
    private Camera cam;
    [SerializeField] private GameObject dungeonGenerator;
    private RectInt[] rects;
    private List<RectInt> rooms;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {           
        rects = dungeonGenerator.GetComponent<TestScript>().Rects;
        rooms = dungeonGenerator.GetComponent<TestScript>().Rooms;
    }

    void OnPostRender()
    {
        if (!mat)
        {
            CreateMaterial();
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadProjectionMatrix(cam.projectionMatrix);
        GL.modelview = cam.worldToCameraMatrix;

        GL.Begin(GL.LINES);
        GL.Color(Color.white);
        for (int i = 0; i < rects.Length; i++)
        {
            Vector3 p1 = new Vector3(rects[i].xMin, 0, rects[i].yMin);
            Vector3 p2 = new Vector3(rects[i].xMin, 0, rects[i].yMax);
            Vector3 p3 = new Vector3(rects[i].xMax, 0, rects[i].yMax);
            Vector3 p4 = new Vector3(rects[i].xMax, 0, rects[i].yMin);

            GL.Vertex(p1);
            GL.Vertex(p2);

            GL.Vertex(p2);
            GL.Vertex(p3);

            GL.Vertex(p3);
            GL.Vertex(p4);

            GL.Vertex(p4);
            GL.Vertex(p1);
        }
        GL.End();

        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector3 p1 = new Vector3(rooms[i].xMin, 0, rooms[i].yMin);
            Vector3 p2 = new Vector3(rooms[i].xMin, 0, rooms[i].yMax);
            Vector3 p3 = new Vector3(rooms[i].xMax, 0, rooms[i].yMax);
            Vector3 p4 = new Vector3(rooms[i].xMax, 0, rooms[i].yMin);

            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.Vertex(p3);
            GL.Vertex(p4);
        }

        GL.End();
        GL.PopMatrix();
    }

    static void CreateMaterial()
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        mat = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
    }
}

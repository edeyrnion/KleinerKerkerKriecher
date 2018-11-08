using UnityEngine;

public class TestGL : MonoBehaviour
{
    // Draws a line from "startVertex" var to the curent mouse position.
    static Material mat;
    //[SerializeField] 
    private Camera cam;
    Vector3 startVertex;
    Vector3 mousePos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        startVertex = Vector3.zero;
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        // Press space to update startVertex
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startVertex = new Vector3(mousePos.x / Screen.width,0, mousePos.y / Screen.height);
        }
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
        GL.Color(Color.red);
        GL.Vertex(Vector3.zero);
        GL.Vertex(Vector3.one);
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
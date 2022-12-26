using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private GameObject tempLinePrefab;
    
    private GameObject currentLine;
    private GameObject tempLine;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private LineRenderer tempLineRenderer;

    private List<Vector2> fingerPosition;

    [SerializeField]
    private float minDistance;

    bool firstLine;

    [SerializeField] bool canCutIntoLine;

    float colliderHalfWidth = 0.01f;
    
    // Start is called before the first frame update
    void Awake()
    {
        fingerPosition = new List<Vector2>();
        firstLine = true;
        tempLine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.instance.gameState != GameState.Playing || Controller.instance.playingState != PlayingState.Draw) 
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempFingerPos, fingerPosition[fingerPosition.Count - 1]) > minDistance)
            {
                UpdateLine(tempFingerPos);
            }
            UpdateCenterOfMass();
        }

        if (Input.GetMouseButtonUp(0))
        {
            bool checkInvalidLine = DeleteInvalidLine();
            if (!checkInvalidLine)
            {
                LineAffectedByGravity();
                FinishFirstLineAutoPlay();
                ConvertEdgeToPolygon();

                Controller.instance.playingState = PlayingState.BikeMove;
            }
            DestroyTempLine();
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        fingerPosition.Clear();

        fingerPosition.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPosition.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPosition[0]);
        lineRenderer.SetPosition(1, fingerPosition[1]);
        edgeCollider.points = fingerPosition.ToArray();
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPosition.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        edgeCollider.points = fingerPosition.ToArray();

        if (CheckCutIntoObject())
        {
            RemoveCutPoint();
            if (tempLine == null)
            {
                CreateTempLine();
            }
            else UpdateTempLine();
        }
        else DestroyTempLine();

    }

    bool CheckCutIntoObject()
    {
        ContactFilter2D filter = new ContactFilter2D();
        Collider2D[] result = new Collider2D[1];

        if (canCutIntoLine) 
            filter.SetLayerMask(LayerMask.GetMask("Ground", "NoDraw", "Bike", "NoDrawPassable"));
        else filter.SetLayerMask(LayerMask.GetMask("Ground", "NoDraw", "Bike", "NoDrawPassable", "Line"));
        edgeCollider.OverlapCollider(filter, result);
        if (result[0])
            return true;

        return false;
    }

    void RemoveCutPoint()
    {
        fingerPosition.RemoveAt(fingerPosition.Count - 1);
        lineRenderer.positionCount--;
        edgeCollider.points = fingerPosition.ToArray();
    }

    void CreateTempLine()
    {
        tempLine = Instantiate(tempLinePrefab, Vector3.zero, Quaternion.identity);
        tempLineRenderer = tempLine.GetComponent<LineRenderer>();

        tempLineRenderer.SetPosition(0, fingerPosition[fingerPosition.Count - 1]);
        tempLineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void UpdateTempLine()
    {
        tempLineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void DestroyTempLine()
    {
        if (tempLine != null)
        {
            Destroy(tempLine);
            tempLine = null;
        }
    }

    void UpdateCenterOfMass()
    {
        Vector3[] tempList = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(tempList);
        currentLine.GetComponent<Rigidbody2D>().centerOfMass = AveragePosition(tempList);
    }

    Vector3 AveragePosition(Vector3[] list)
    {
        Vector3 result = Vector3.zero;
        foreach (Vector3 vector in list)
            result += vector;
        result /= list.Length;

        return result;
    }

    bool DeleteInvalidLine()
    {
        //bool invalid = true;
        //Vector2 lastPoint = fingerPosition[fingerPosition.Count - 1];

        //fingerPosition.Add(lastPoint + new Vector2(minDistance, 0));
        //edgeCollider.points = fingerPosition.ToArray();
        //if (!CheckCutIntoObject())
        //    invalid = false;

        //fingerPosition[fingerPosition.Count - 1] = lastPoint + new Vector2(0, minDistance);
        //edgeCollider.points = fingerPosition.ToArray();
        //if (!CheckCutIntoObject())
        //    invalid = false;

        //fingerPosition[fingerPosition.Count - 1] = lastPoint + new Vector2(-minDistance, 0);
        //edgeCollider.points = fingerPosition.ToArray();
        //if (!CheckCutIntoObject())
        //    invalid = false;

        //fingerPosition[fingerPosition.Count - 1] = lastPoint + new Vector2(0, -minDistance);
        //edgeCollider.points = fingerPosition.ToArray();
        //if (!CheckCutIntoObject())
        //    invalid = false;

        //if (!invalid)
        //{
        //    edgeCollider.points[edgeCollider.pointCount - 1] = lastPoint;
        //    return;
        //}

        if (edgeCollider == null) return true;

        if (edgeCollider.pointCount <= 2)
        {
            Destroy(currentLine);
            return true;
        }

        return false;
    }

    void LineAffectedByGravity()
    {
        currentLine.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void FinishFirstLineAutoPlay()
    {
        if (firstLine)
        {
            Time.timeScale = 1;
            //firstLine = false;
        }
    }

    void ConvertEdgeToPolygon()
    {
        PolygonCollider2D polygonCollider = currentLine.AddComponent<PolygonCollider2D>();
        polygonCollider.pathCount = edgeCollider.edgeCount;
        for (int i = 0; i < polygonCollider.pathCount; ++i)
        {
            Vector2 point1 = edgeCollider.points[i];
            Vector2 point2 = edgeCollider.points[i + 1];
            
            float opposite = point2.y - point1.y;
            float adjacent = point2.x - point1.x;
            float angle = Mathf.Atan(opposite / adjacent);

            float deltaX = Mathf.Sin(angle) * colliderHalfWidth;
            float deltaY = Mathf.Cos(angle) * colliderHalfWidth;

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(point1.x - deltaX, point1.y + deltaY);
            points[1] = new Vector2(point2.x - deltaX, point2.y + deltaY);
            points[2] = new Vector2(point2.x + deltaX, point2.y - deltaY);
            points[3] = new Vector2(point1.x + deltaX, point1.y - deltaY);

            polygonCollider.SetPath(i, points);
        }

        Destroy(edgeCollider);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.25f;
    private int numberOfSegments = 35;
    private float lineWidth = 0.1f;

    public Transform playerPoint;
    public Transform anchorPoint;
    //public int playerIndex;
    //public Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        this.lineRenderer = GetComponent<LineRenderer>();
        this.edgeCollider = GetComponent<EdgeCollider2D>();
        Vector3 ropeStartPoint = playerPoint.position;

        for (int i = 0; i < numberOfSegments; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }
    private void FixedUpdate()
    {
        Simulate();
    }
    private void Simulate()
    {
        //Simulation
        Vector2 gravityForce = new Vector2(0f,-1.5f);

        for (int i = 1; i < numberOfSegments; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += gravityForce * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }
        //Constraints
        for (int i = 0; i < 50; i++)
        {
            ApplyConstraints();
        }
    }
    private void ApplyConstraints()
    {
        //only sort of understand what this function does 

        RopeSegment firstSegment = this.ropeSegments[10];
        firstSegment.posNow = this.playerPoint.position;
        this.ropeSegments[10] = firstSegment;

        RopeSegment endSegment = this.ropeSegments[this.numberOfSegments - 1];
        endSegment.posNow = this.anchorPoint.position;
        this.ropeSegments[numberOfSegments - 1] = endSegment;

        
        for (int i = 0; i < numberOfSegments-1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow-secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist-this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen) //if the rope segment is too long
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            }
            else if (dist < ropeSegLen) //if the rope is too small
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }
            Vector2 changeAmount = changeDir * error;
            if (i != 10)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
        
    }

    private void DrawRope()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        Vector2[] points = new Vector2[numberOfSegments];
        Vector3[] ropePositions = new Vector3[this.numberOfSegments];
        for (int i = 0; i < this.numberOfSegments; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
            points[i] = ropeSegments[i].posNow;
        }
        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);

        edgeCollider.points = points;
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}

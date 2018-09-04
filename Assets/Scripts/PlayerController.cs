using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    LineRenderer lineRenderer;
    SpringJoint2D joint;
    GameObject puck;

    public float targetJointDistance;
	// Use this for initialization
	void Start ()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        puck = GameObject.Find("Puck");

        joint = GetComponent<SpringJoint2D>();
        joint.connectedBody = puck.GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
	}

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;
            joint.enabled = true;
            lineRenderer.SetPositions(new Vector3[] { rigidbody2d.position , puck.transform.position});
            StartCoroutine(ConstrainJointDistance(targetJointDistance));
        }
        else
        {
            lineRenderer.enabled = false;
            joint.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Vector2 ray = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rigidbody2d.position;
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, ray, ray.magnitude);
        if (!hit)
        {
            rigidbody2d.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            rigidbody2d.MovePosition(hit.point);
        }
    }

    IEnumerator ConstrainJointDistance(float targetDistance)
    {
        if (joint.distance > targetDistance)
        {
            joint.distance = Mathf.Lerp(targetDistance, joint.distance, 0.98f);
            yield return null;
        }
        else
        {
            yield break;
        }
    }
}

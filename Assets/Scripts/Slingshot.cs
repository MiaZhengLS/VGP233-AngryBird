using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public Camera camera;
    public LineRenderer[] slings;
    public Transform[] slingFixedPos;
    public Transform slingDefaultPos;
    public Transform slingCenterPos;
    public GameObject prbBird;
    public float birdPosOffset;
    public float maxSlingLen;
    public float shootSpeed;
    public Trajectory trajectory;

    private Transform bird;
    private Collider2D birdCollider;
    private Rigidbody2D rbBird;
    private bool isDragging;
    private float maxSlingLenSqr;

    void Start()
    {
        slings[0].positionCount = 2;
        slings[1].positionCount = 2;
        slings[0].SetPosition(0, slingFixedPos[0].position);
        slings[1].SetPosition(0, slingFixedPos[1].position);
        maxSlingLenSqr = Mathf.Pow(maxSlingLen, 2);

        ResetSlingshot();
    }

    void Update()
    {
        if(bird == null)
            return;
        if (isDragging)
        {
            var worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = slings[0].GetPosition(0).z;
            worldPos = ValidateSlingLength(worldPos);
            SetSlingPosition(worldPos);
            SetBirdPosition(worldPos);
            trajectory.ShowTrajectory(bird.position, (slingCenterPos.position - bird.position) * shootSpeed, Physics2D.gravity);
        }
    }

    void OnMouseDrag()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        Shoot();
        SetSlingPosition(slingDefaultPos.position);
    }

    void Shoot()
    {
        rbBird.simulated = true;
        birdCollider.enabled = true;
        rbBird.velocity = (slingCenterPos.position - bird.position) * shootSpeed;
        bird = null;
        Invoke("ResetSlingshot", 2f);
    }

    void ResetSlingshot()
    {
        trajectory.HideTrajectory();
        SetSlingPosition(slingDefaultPos.position);
        CreateNewBird();
    }

    void CreateNewBird()
    {
        bird = Instantiate(prbBird).transform;
        birdCollider = bird.GetComponent<Collider2D>();
        rbBird = bird.GetComponent<Rigidbody2D>();
        birdCollider.enabled = false;
        rbBird.simulated = false;
        SetBirdPosition(slingDefaultPos.position);
    }

    void SetSlingPosition(Vector3 pos)
    {
        slings[0].SetPosition(1, pos);
        slings[1].SetPosition(1, pos);
    }

    void SetBirdPosition(Vector3 pos)
    {
        var dir = (pos - slingCenterPos.position).normalized;
        bird.position = pos + dir * birdPosOffset;
        bird.right = -dir;
    }

    Vector3 ValidateSlingLength(Vector3 pos)
    {
        var rawVec = pos - slingCenterPos.position;
        if(rawVec.sqrMagnitude <= maxSlingLenSqr)
        {
            return pos;
        }
        return slingCenterPos.position + rawVec.normalized * maxSlingLen;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float leftRightSpeed = 5;
    public float jumpForce = 6;
    public Animator anim;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(1).GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), 0.2f, LayerMask.GetMask("Ground"));

#if UNITY_EDITOR || UNITY_WINDOWS
        if (GameManager.Instance.GetEnableControls() && GameManager.Instance.canMove)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > LevelBoundary.leftSide)
                    transform.Translate(leftRightSpeed * Time.deltaTime * Vector3.left);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < LevelBoundary.rightSide)
                    transform.Translate(-1 * leftRightSpeed * Time.deltaTime * Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
#endif

#if UNITY_EDITOR
        if (GameManager.Instance.GetEnableControls() && GameManager.Instance.canMove)
        {
            if (Input.GetMouseButton(0))
                MoveTo(Input.mousePosition);
        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        if (GameManager.Instance.GetEnableControls() && GameManager.Instance.canMove)
        {
            if (Input.touchCount == 1)
                MoveTo(Input.GetTouch(0).position);
            if ((Input.acceleration.x > 0 && transform.position.x < LevelBoundary.rightSide) || (Input.acceleration.x < 0 && transform.position.x > LevelBoundary.leftSide))
                transform.Translate(Input.acceleration.x * Time.deltaTime * leftRightSpeed, 0, 0, Space.World);
        }
#endif
    }

    public void Jump()
    {
        if (isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Stumble()
    {
        anim.SetTrigger("Stumble");
        SetCanMove(false);
    }

    public void SetCanMove(bool move)
    {
        GameManager.Instance.canMove = move;
    }

	private void MoveTo(Vector3 pTouchPos)
    {
        if (pTouchPos.x > Screen.width / 2 && transform.position.x < LevelBoundary.rightSide)
            transform.Translate(-1 * leftRightSpeed * Time.deltaTime * Vector3.left);
        else if (pTouchPos.x < Screen.width / 2 && transform.position.x > LevelBoundary.leftSide)
            transform.Translate(leftRightSpeed * Time.deltaTime * Vector3.left);
    }
}

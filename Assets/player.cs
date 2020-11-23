using UnityEngine;

public class player : MonoBehaviour
{
    public Grid grid;
    public float playerSpeed = 1;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //turn
        var turn = Input.GetAxis("Mouse X");
        Vector3 axis = new Vector3(0,0,1);
        grid.transform.RotateAround(Vector3.zero, axis, turn);
        //move
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        animator.enabled = Mathf.Abs(v)+Mathf.Abs(h) >= 0.5f;
        Vector3 move = new Vector3(-h, -v, 0);
        grid.transform.Translate(move * Time.deltaTime * playerSpeed, Space.World);
    }
}
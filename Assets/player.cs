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
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        animator.enabled = Mathf.Abs(v)+Mathf.Abs(h) >= 0.5f;
        Vector3 move = new Vector3(-h, -v, 0);
        grid.transform.Translate(move * Time.deltaTime * playerSpeed);
    }
}
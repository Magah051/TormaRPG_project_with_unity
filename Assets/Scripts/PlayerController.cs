using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Esse script controlará todos os tipos de Players
    public Animator playerAnimator;
    float input_x = 0;
    float input_y = 0;
    public float speed = 2.5f;
    bool isWalking = false;
    Rigidbody2D rb2D;
    Vector2 movement = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);
        movement = new Vector2(input_x, input_y);

        if (isWalking)
        {
            playerAnimator.SetFloat("input_x", input_x);
            playerAnimator.SetFloat("input_y", input_y);
        }

        playerAnimator.SetBool("isWalking", isWalking);

        //O "hero" não possui ataque por enquanto (05/09/2023)
        if (Input.GetButtonDown("Fire1"))
            playerAnimator.SetTrigger("attack");
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * speed * Time.fixedDeltaTime);
    }
}
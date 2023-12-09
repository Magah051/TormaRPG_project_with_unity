using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Pet : MonoBehaviour
{
    public GameObject player;
    public Animator petAnimator;

    public float speed = 1;
    public float keepDistance = 0.3f;

    bool isWalking;

    float input_x;
    float input_y;
    float lastDirectionX;
    float lastDirectionY;

    Vector2 petPos;
    Vector2 playerPos;

    private void Start()
    {
        petAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        petPos = transform.position;
        playerPos = SetDirection(1, 1, player.transform.position);

        transform.position = Vector2.MoveTowards(petPos, playerPos, speed * Time.deltaTime);
    }

    private void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);

        if (isWalking)
        {
            petAnimator.SetFloat("input_x", input_x);
            petAnimator.SetFloat("input_y", input_y);
        }

        if (input_x > 0 || input_x < 0)
            lastDirectionX = input_x;

        if (input_y > 0 || input_y < 0)
            lastDirectionY = input_y;

        petAnimator.SetBool("isWalking", isWalking);

        petPos = transform.position;
        playerPos = SetDirection(lastDirectionX, lastDirectionY, player.transform.position);

        transform.position = Vector2.MoveTowards(petPos, playerPos, speed * Time.deltaTime);
    }

    Vector2 SetDirection(float input_x, float input_y, Vector2 playerPos)
    {
        if (input_x < 0)
        {
            playerPos.x += keepDistance;
        }
        else if (input_x > 0)
        {
            playerPos.x -= keepDistance;
        }

        if (input_y < 0)
        {
            playerPos.y += keepDistance;
        }
        else if (input_y > 0)
        {
            playerPos.y -= keepDistance;
        }

        return playerPos;
    }
}
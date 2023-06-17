using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float speed = 10;

    CharacterController controller;

    void Awake()
    {
        TryGetComponent(out controller);
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (direction.z == 1)
            { 
                direction.z = 0;
            }
            direction.z = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (direction.x == 1)
            {
                direction.x = 0;
            }
            direction.x = -1;
        }

        controller.Move(direction.normalized * speed * Time.deltaTime);
    }
}
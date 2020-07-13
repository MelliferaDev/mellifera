using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private CharacterController controller;

    public float baseSpeed = 10.0f;
    public float rotSpeedX = 3.0f;
    public float rotSpeedY = 1.5f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = transform.forward * baseSpeed;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 yaw = transform.right * (inputX * rotSpeedX * Time.deltaTime);
        Vector3 pitch = transform.up * (inputY * rotSpeedY * Time.deltaTime);
        Vector3 dir = yaw + pitch;

        float maxX = Quaternion.LookRotation(move + dir).eulerAngles.x;
        bool enteringLoop = (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290);
        if (!enteringLoop)
        {
            move += dir;
            transform.rotation = Quaternion.LookRotation(move);
        }

        controller.Move(move * Time.deltaTime);
    }
    
}

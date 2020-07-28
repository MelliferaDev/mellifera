using UnityEngine;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] public float mouseSensitivity = 1;
        [Space(20)]
        [SerializeField] public string speedUpAxis = "Fire1";
        [SerializeField] public string slowDownAxis = "Fire2";
        [SerializeField] public KeyCode powerupKey = KeyCode.E;
        [SerializeField] public KeyCode landFlyKey = KeyCode.Space;

        [Header("Non-configurable")] 
        [SerializeField] public KeyCode pauseKey = KeyCode.P;

        public bool GetSpeedUpBtnClicked() => Input.GetButtonDown(speedUpAxis);

        public bool GetSlowDownBtnClicked() => Input.GetButtonDown(slowDownAxis);

        public bool GetPowerUpKeyClicked() => Input.GetKeyDown(powerupKey);
        
        public bool GetLandFlyKeyClicked() => Input.GetKeyDown(landFlyKey);

        public bool GetPauseBtnClicked() => Input.GetKeyDown(pauseKey);

        public Vector2 GetMouseAxes() => new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, 
            Input.GetAxis("Mouse Y") * mouseSensitivity);
        
    }
}
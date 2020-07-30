using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class InputManagerUI : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        [Space(20)]
        [SerializeField] private InputField mSensitivityIn;
        [SerializeField] private Dropdown speedUpAxisIn;
        [SerializeField] private Dropdown slowDownAxisIn;
        [SerializeField] private Dropdown powerupKeyIn;
        [SerializeField] private Dropdown landFlyKeyIn;
        [Space(20)] 
        [SerializeField] private List<string> axes;
        [SerializeField] private List<KeyCode> keys;
        
        void Start()
        {
            // fill the drop down lists
            speedUpAxisIn.ClearOptions();
            speedUpAxisIn.AddOptions(axes);
            slowDownAxisIn.ClearOptions();
            slowDownAxisIn.AddOptions(axes);
            powerupKeyIn.ClearOptions();
            powerupKeyIn.AddOptions(keys.Select(k => k.ToString()).ToList());
            landFlyKeyIn.ClearOptions();
            landFlyKeyIn.AddOptions(keys.Select(k => k.ToString()).ToList());
            
            // pre-populate with default values (TODO: make this variable)
            mSensitivityIn.text = inputManager.mouseSensitivity.ToString("f2");
            speedUpAxisIn.value = 0;
            slowDownAxisIn.value = 1;
            powerupKeyIn.value = 2;
            landFlyKeyIn.value = 0;
            
            // setup the ui interaction listeners
            mSensitivityIn.onEndEdit.AddListener(MouseSensitivityInput);
            
            speedUpAxisIn.onValueChanged.AddListener(delegate(int arg0)
            {
                inputManager.speedUpAxis = MouseNameToAxisName(axes[arg0]);
            });
            
            slowDownAxisIn.onValueChanged.AddListener(delegate(int arg0)
            {
                inputManager.slowDownAxis = MouseNameToAxisName(axes[arg0]);
            });
            
            powerupKeyIn.onValueChanged.AddListener(delegate(int arg0)
            {
                inputManager.vortexKey = keys[arg0];
            });
            
            landFlyKeyIn.onValueChanged.AddListener(delegate(int arg0)
            {
                inputManager.landFlyKey = keys[arg0];
            });
        }

        private void MouseSensitivityInput(string arg0)
        {
            mSensitivityIn.readOnly = true;
            if (float.TryParse(arg0, out float f1))
            {
                f1 = Mathf.Clamp(f1, 0.1f, 5.0f); // TODO: make variables
                inputManager.mouseSensitivity = f1;
            }
            mSensitivityIn.text = inputManager.mouseSensitivity.ToString("f2");
            mSensitivityIn.readOnly = false;
        }

        private string MouseNameToAxisName(string mouseName)
        {
            switch (mouseName)
            {
                case "Left Mouse": return "Fire1";
                case "Right Mouse": return "Fire2";
                case "Middle Mouse": return "Fire3";
                case "Primary Fire": return "Fire1";
                case "Secondary Fire": return "Fire2";
                case "Tertiary Fire": return "Fire3";
                default: return mouseName;
            }
        }
    }
}
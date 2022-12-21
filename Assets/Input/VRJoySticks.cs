using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JoyStick
{
    [SerializeField] public float A;
    public JoyStick(float a)
    {
        A = a;
    }
}

public class VRJoySticks : MonoBehaviour
{

    [SerializeField] JoyStickActions JoySticks;


    private JoyStick getJoyStickRight()
    {
        return new JoyStick(JoySticks.Player.A.ReadValue<float>());
    }

    private JoyStick getJoyStickLeft()
    {
        return new JoyStick(JoySticks.Player.A.ReadValue<float>());
    }

    [SerializeField]
    private JoyStick _right;
    public JoyStick Right
    {
        get
        {
            return getJoyStickRight();
        }
        set { _right = value; }
    }
    [SerializeField]
    private JoyStick _left;
    public JoyStick Left
    {
        get
        {
            return getJoyStickLeft();

        }
        set { _left = value; }
    }



    void OnEnable()
    {
        JoySticks.Enable();

    }

    void OnDisable()
    {
        JoySticks.Disable();
    }

    void Awake()
    {
        JoySticks = new JoyStickActions();
    }

    // Update is called once per frame
    void Update()
    {
        Left = getJoyStickLeft();

        Right = getJoyStickRight();
    }

}

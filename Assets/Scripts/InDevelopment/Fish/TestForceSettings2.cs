using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InDevelopment.Fish
{
    public class TestForceSettings : MonoBehaviour
    {
        //Use to switch between Force Modes
        enum ModeSwitching
        {
            Start,
            Impulse,
            Acceleration,
            Force,
            VelocityChange
        };

        private ModeSwitching _mModeSwitching;

        Vector3 _mStartPos, _mStartForce;
        Vector3 _mNewForce;
        [SerializeField] private GameObject testObject;
        [SerializeField] private Rigidbody mRigidbody;

        string _mForceXString = string.Empty;
        string _mForceYString = string.Empty;

        float _mForceX, _mForceY;
        float _mResult;


        void Start()
        {
            //You get the Rigidbody component you attach to the GameObject
            
            mRigidbody = testObject.GetComponent<Rigidbody>();

            //This starts at first mode (nothing happening yet)
            _mModeSwitching = ModeSwitching.Start;

            //Initialising the force which is used on GameObject in various ways
            _mNewForce = new Vector3(-5.0f, 1.0f, 0.0f);

            //Initialising floats
            _mForceX = 0;
            _mForceY = 0;

            //The forces typed in from the text fields (the ones you can manipulate in Game view)
            _mForceXString = "0";
            _mForceYString = "0";

            //The GameObject's starting position and Rigidbody position
            _mStartPos = transform.position;
            _mStartForce = mRigidbody.transform.position;
        }

        void FixedUpdate()
        {
            //If the current mode is not the starting mode (or the GameObject is not reset), the force can change
            if (_mModeSwitching != ModeSwitching.Start)
            {
                //The force changes depending what you input into the text fields
                _mNewForce = new Vector3(_mForceX, _mForceY, 0);
            }
            
            //Here, switching modes depend on button presses in the Game mode
            switch (_mModeSwitching)
            {
                //This is the starting mode which resets the GameObject
                case ModeSwitching.Start:
                    //This resets the GameObject and Rigidbody to their starting positions
                    transform.position = _mStartPos;
                    mRigidbody.transform.position = _mStartForce;

                    //This resets the velocity of the Rigidbody
                    mRigidbody.velocity = new Vector3(0f, 0f, 0f);
                    break;

                //These are the modes ForceMode can force on a Rigidbody
                //This is Acceleration mode
                case ModeSwitching.Acceleration:
                    //The function converts the text fields into floats and updates the Rigidbody’s force
                    MakeCustomForce();

                    //Use Acceleration as the force on the Rigidbody
                    mRigidbody.AddForce(_mNewForce, ForceMode.Acceleration);
                    break;

                //This is Force Mode, using a continuous force on the Rigidbody considering its mass
                case ModeSwitching.Force:
                    //Converts the text fields into floats and updates the force applied to the Rigidbody
                    MakeCustomForce();

                    //Use Force as the force on GameObject’s Rigidbody
                    mRigidbody.AddForce(_mNewForce, ForceMode.Force);
                    break;

                //This is Impulse Mode, which involves using the Rigidbody’s mass to apply an instant impulse force.
                case ModeSwitching.Impulse:
                    //The function converts the text fields into floats and updates the force applied to the Rigidbody
                    MakeCustomForce();

                    //Use Impulse as the force on GameObject
                    mRigidbody.AddForce(_mNewForce, ForceMode.Impulse);
                    break;


                //This is VelocityChange which involves ignoring the mass of the GameObject and impacting it with a sudden speed change in a direction
                case ModeSwitching.VelocityChange:
                    //Converts the text fields into floats and updates the force applied to the Rigidbody
                    MakeCustomForce();

                    //Make a Velocity change on the Rigidbody
                    mRigidbody.AddForce(_mNewForce, ForceMode.VelocityChange);
                    break;
            }
        }
        
        
        

        //The function outputs buttons, text fields, and other interactable UI elements to the Scene in Game view
        private void OnGUI()
        {
            //Getting the inputs from each text field and storing them as strings
            _mForceXString = GUI.TextField(new Rect(300, 10, 100, 30), _mForceXString, 25);
            _mForceYString = GUI.TextField(new Rect(300, 100, 100, 30), _mForceYString, 25);
            
            //Press the button to reset the GameObject and Rigidbody
            if (GUI.Button(new Rect(100, 0, 150, 30), "Reset"))
            {
                //This switches to the start/reset case
                _mModeSwitching = ModeSwitching.Start;
            }

            //When you press the Acceleration button, switch to Acceleration mode
            if (GUI.Button(new Rect(100, 30, 150, 30), "Apply Acceleration"))
            {
                //Switch to Acceleration (apply acceleration force to GameObject)
                _mModeSwitching = ModeSwitching.Acceleration;
            }

            //If you press the Impulse button
            if (GUI.Button(new Rect(100, 60, 150, 30), "Apply Impulse"))
            {
                //Switch to impulse (apply impulse forces to GameObject)
                _mModeSwitching = ModeSwitching.Impulse;
            }

            //If you press the Force Button, switch to Force state
            if (GUI.Button(new Rect(100, 90, 150, 30), "Apply Force"))
            {
                //Switch to Force (apply force to GameObject)
                _mModeSwitching = ModeSwitching.Force;
            }

            //Press the button to switch to VelocityChange state
            if (GUI.Button(new Rect(100, 120, 150, 30), "Apply Velocity Change"))
            {
                //Switch to velocity changing
                _mModeSwitching = ModeSwitching.VelocityChange;
            }
        }

        //Changing strings to floats for the forces
        float ConvertToFloat(string floatName)
        {
            float.TryParse(floatName, out _mResult);
            return _mResult;
        }

        //Set the converted float from the text fields as the forces to apply to the Rigidbody
        void MakeCustomForce()
        {
            //This converts the strings to floats
            _mForceX = ConvertToFloat(_mForceXString);
            _mForceY = ConvertToFloat(_mForceYString);
        }
    }
}

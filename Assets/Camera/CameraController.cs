using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    //Camera settings
    public float moveSpeed = 4;
    public float moveSpeedMax = 20;
    public float friction = 2;

    public float pitchSpeed = 1;
    public float maxPitch = 70;
    public float minPitch = 40;

    public float rotationSpeed = 0.2f;
    public float maxRotationSpeed = 4;
    public float rotationFriction = 0.1f;

    public float rotationSpeedMouse = 12;


    public float zoomSpeed = 2;
    public float maxZoom = 20;
    public float minZoom = 4;

    private float xSpeed = 0;
    private float zSpeed = 0;
    private float ySpeed = 0;


    private bool forwards = false;
    private bool backwards = false;
    private bool leftwards = false;
    private bool rightwards = false;

    private bool leftRotation = false;
    private bool rightRotation = false;

    private bool mEnabled = false;

    private bool leftMB = false;
    private bool rightMB = false;

    public Vector3 speed;
    public float rotation = 0;

    Interacteble focus;

    // Update is called once per frame
    void Update()
    {
        keyDownCheck();
        keyUpCheck();

        #region Camera Movment
        //X
        if (forwards && zSpeed < moveSpeedMax) zSpeed += moveSpeed;
        if (backwards && zSpeed > -moveSpeedMax) zSpeed -= moveSpeed;
        if (!backwards && !forwards)
        {
            if (zSpeed > 0.009f) zSpeed -= friction;
            else if (zSpeed < -0.009f) zSpeed += friction;
            else zSpeed = 0;
        }
        //Y
        if (leftwards && xSpeed > -moveSpeedMax) xSpeed -= moveSpeed;
        if (rightwards && xSpeed < moveSpeedMax) xSpeed += moveSpeed;
        if (!leftwards && !rightwards)
        {
            if (xSpeed > 0.009f) xSpeed -= friction;
            else if (xSpeed < -0.009f) xSpeed += friction;
            else xSpeed = 0;
            
        }
        #endregion

        #region Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float distance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        if (distance -scroll < minZoom) scroll = distance - minZoom;
        if (distance -scroll > maxZoom) scroll = distance - maxZoom;

        if (distance > minZoom && scroll > 0) Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, this.transform.position, scroll);
        if (distance < maxZoom && scroll < 0) Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, this.transform.position, scroll);
        #endregion

        #region Rotation

        //Q & E rotation
        if (leftRotation && rotation < maxRotationSpeed) rotation += rotationSpeed;
        if (rightRotation && rotation > -maxRotationSpeed) rotation -= rotationSpeed;
        if (!leftRotation && !rightRotation)
        {
            if (rotation > 0.009f) rotation -= rotationFriction;
            else if (rotation < -0.009f) rotation += rotationFriction;
            else rotation = 0;
        }
        else if (!leftMB)
        {
            Camera.main.transform.RotateAround(this.gameObject.transform.position, Vector3.up, rotation);
            this.gameObject.transform.Rotate(Vector3.up, rotation);
        }

        
        if (leftMB && mEnabled)
        {
            rotation = Input.GetAxis("Mouse X") * rotationSpeedMouse;
            Camera.main.transform.RotateAround(this.gameObject.transform.position, Vector3.up, rotation);
            this.gameObject.transform.Rotate(Vector3.up, rotation);
        }
        

        #endregion

        #region Pitch
        float pitch = Input.GetAxis("Mouse Y") * pitchSpeed;
        if (rightMB && leftMB && mEnabled)
        {
            float zAngle = Camera.main.transform.rotation.eulerAngles.x;

            if (zAngle > minPitch && pitch > 0)
            {
                Camera.main.transform.Rotate(-pitch, 0, 0);
            }
            if (zAngle < maxPitch && pitch < 0)
            {
                Camera.main.transform.Rotate(-pitch, 0, 0);
            }
        }
        #endregion

        
        if (!mEnabled)
        {
            #region Select
            if (Input.GetMouseButtonDown(0))
            {
                //Creating Ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //Check what it his
                if(Physics.Raycast(ray, out hit, 10000))
                {
                    Interacteble interactable = hit.collider.GetComponent<Interacteble>();

                    if (interactable != null)
                    {
                        SetFocus(interactable);
                    }
                    else RemoveFocus();

                }
            }
            #endregion

            if (Input.GetMouseButtonDown(1) && focus != null)
            {
                if (focus.entity)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Unit unit = (Unit)focus;
                    unit.Command(ray);
                }
            }
        }


        speed = new Vector3(xSpeed, ySpeed, zSpeed);
        speed = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y/2, 0) * speed;
        
        if (Camera.main.transform.rotation.y < 0) speed *= -1;
        this.gameObject.transform.Translate(speed);

    }

    void keyDownCheck()
    {
        //Camera movment
        if (Input.GetKeyDown("w")) forwards = true;
        if (Input.GetKeyDown("s")) backwards = true;
        if (Input.GetKeyDown("a")) leftwards = true;
        if (Input.GetKeyDown("d")) rightwards = true;

        
        //Rotation
        leftRotation = Input.GetKey(KeyCode.Q);
        rightRotation = Input.GetKey(KeyCode.E);

        mEnabled = Input.GetKey(KeyCode.LeftShift);


        //Mouse
        if (Input.GetMouseButtonDown(1)) leftMB = true;
        if (Input.GetMouseButtonDown(0)) rightMB = true;
    }

    void keyUpCheck()
    {
        //Camera movment
        if (Input.GetKeyUp("w")) forwards = false;
        if (Input.GetKeyUp("s")) backwards = false;
        if (Input.GetKeyUp("a")) leftwards = false;
        if (Input.GetKeyUp("d")) rightwards = false;

        
        //Rotation
        leftRotation = Input.GetKey(KeyCode.Q);
        rightRotation = Input.GetKey(KeyCode.E);

        mEnabled = Input.GetKey(KeyCode.LeftShift);

        //Mouse
        if (Input.GetMouseButtonUp(1)) leftMB = false;
        if (Input.GetMouseButtonUp(0)) rightMB = false;
    }

    void SetFocus(Interacteble newFocus)
    {
       focus = newFocus;
    }

    void RemoveFocus()
    {
        focus = null;
    }
}




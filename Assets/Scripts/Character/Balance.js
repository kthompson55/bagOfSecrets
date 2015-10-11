#pragma strict
var torque : float;
var rb: Rigidbody;
var push = false;

function Start() {
	rb = GetComponent.<Rigidbody>();
	PushReady();
}

function Update () {
//-------------------Alternative Rotatation Management---------------------------------------------------------------------------- 
//-------------------(calculate angle from center of object using mouse position and rotate object to match)-----------------------
//     var pos = Camera.main.WorldToScreenPoint(transform.position);
//     var dir = Input.mousePosition - pos;
//     var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
//     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
     
     
     var mouseOffset = new Vector3(0, 0, Input.GetAxis("Mouse X"));
  transform.Rotate(mouseOffset, Space.Self);
 }
 
function FixedUpdate() {
        if (push == true){
        	var turn = 2; //Random.Range(10, -11);
        	rb.AddTorque(Vector3(0, -10, 0));
        	PushReady();
        }
    }
    
function PushReady() {
	yield WaitForSeconds(5);
	push = true;
	yield WaitForSeconds(2);
	push = false;
}

//function FixedUpdate () {
//	var v = Input.GetAxis("Horizontal");
//	var rotationVector = transform.rotation.eulerAngles;
//   rotationVector.x = v*20;
//   transform.rotation = Quaternion.Euler(rotationVector);
//}
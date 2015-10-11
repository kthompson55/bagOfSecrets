#pragma strict

function Start () {

}

function Update () {
//-------------------Alternative Rotatation Management---------------------------------------------------------------------------- 
//-------------------(calculate angle from center of object using mouse position and rotate object to match)-----------------------
//     var pos = Camera.main.WorldToScreenPoint(transform.position);
//     var dir = Input.mousePosition - pos;
//     var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
//     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
     
     
     var mouseOffset = new Vector3(Input.GetAxis("Mouse X"), 0, 0);
  transform.Rotate(mouseOffset, Space.Self);
 }

//function FixedUpdate () {
//	var v = Input.GetAxis("Horizontal");
//	var rotationVector = transform.rotation.eulerAngles;
//   rotationVector.x = v*20;
//   transform.rotation = Quaternion.Euler(rotationVector);
//}
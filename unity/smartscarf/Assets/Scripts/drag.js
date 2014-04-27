private var titleCamera : Camera;

function OnMouseDown () {
	titleCamera = Camera.main;
    var screenSpace = titleCamera.WorldToScreenPoint(transform.position);
    var offset = transform.position - titleCamera.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    while (Input.GetMouseButton(0))
    {
        var clickPos = Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        var newPos = titleCamera.ScreenToWorldPoint(clickPos) + offset;
        transform.position = newPos;
        yield;
    }
}

var speed:float = 0.0001;

function Update () {

	if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
		var touchDeltaPosition:Vector3 = Input.GetTouch(0).deltaPosition;
		transform.Translate (touchDeltaPosition.x * speed/3, touchDeltaPosition.y * speed/3, 0);
 	}

}
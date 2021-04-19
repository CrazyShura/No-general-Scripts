using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region Fields
	[SerializeField]
	float moveSpeed;
	[SerializeField]
	float moveAmount;
	[SerializeField]
	float rotSpeed;
	[SerializeField]
	float rotAmount;
	[SerializeField]
	float zoomSpeed;
	[SerializeField]
	float zoomAmount;

	Vector3 targetPosition;
	Quaternion targetRotation;
	Vector3 targetZoom;
	Vector3 trueZoomAmount;

	Vector3 mouseMoveAnchor;
	Vector3 targetMouseMoveAnchor;
	Vector3 mouseRotationAnchor;
	Vector3 targetMouseRotationAnchor;
	Plane plane = new Plane(Vector3.up, 0);

	Transform cameraTransform;
	#endregion

	#region Properies

	#endregion

	#region Methods
	private void Start()
	{
		trueZoomAmount = new Vector3(0, zoomAmount, -zoomAmount);
		targetPosition = this.transform.position;
		targetRotation = this.transform.rotation;
		cameraTransform = this.transform.GetChild(0).transform;
		targetZoom = cameraTransform.localPosition;
	}

	private void Update()
	{
		if (PublicStuff.GameMaster.CurrentUnit != null && Input.GetKey(KeyCode.Space))
		{
			targetPosition = PublicStuff.GameMaster.CurrentUnit.transform.position;
		}
		HandleMouseInput();
		HandleKeyBoardInput();
		HandleMovement();
	}

	void HandleMouseInput()
	{
		if (Input.GetMouseButtonDown(2))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float dist;
			if (plane.Raycast(ray, out dist))
			{
				mouseMoveAnchor = ray.GetPoint(dist);
			}
		}
		if (Input.GetMouseButton(2))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float dist;
			if (plane.Raycast(ray, out dist))
			{
				targetMouseMoveAnchor = ray.GetPoint(dist);

				targetPosition = this.transform.position + mouseMoveAnchor - targetMouseMoveAnchor;
			}
		}
		if (Input.mouseScrollDelta.y != 0)
		{
			targetZoom -= Input.mouseScrollDelta.y * trueZoomAmount * 5;
		}
		if (Input.GetMouseButtonDown(1))
		{
			mouseRotationAnchor = Input.mousePosition;
		}
		if (Input.GetMouseButton(1))
		{
			targetMouseRotationAnchor = Input.mousePosition;
			Vector3 difference = mouseRotationAnchor - targetMouseRotationAnchor;
			mouseRotationAnchor = targetMouseRotationAnchor;
			targetRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
		}
	}

	void HandleKeyBoardInput()
	{
		if (Input.GetAxis("Horizontal") != 0)
		{
			if (Input.GetAxis("Horizontal") > 0)
			{
				targetPosition += this.transform.right * moveAmount;
			}
			else
			{
				targetPosition -= this.transform.right * moveAmount;
			}
		}
		if (Input.GetAxis("Vertical") != 0)
		{
			if (Input.GetAxis("Vertical") > 0)
			{
				targetPosition += this.transform.forward * moveAmount;
			}
			else
			{
				targetPosition -= this.transform.forward * moveAmount;
			}
		}
		if (Input.GetAxis("Rotation") != 0)
		{
			if (Input.GetAxis("Rotation") > 0)
			{
				targetRotation *= Quaternion.Euler(Vector3.up * rotAmount);
			}
			else
			{
				targetRotation *= Quaternion.Euler(Vector3.up * -rotAmount);
			}
		}
		if (Input.GetAxis("CameraZoom") != 0)
		{
			if (Input.GetAxis("CameraZoom") > 0)
			{
				targetZoom += trueZoomAmount;
			}
			else
			{
				targetZoom -= trueZoomAmount;
			}
		}
	}

	void HandleMovement()
	{
		this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * moveSpeed);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
		cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetZoom, Time.deltaTime * zoomSpeed);
	}
	#endregion
}

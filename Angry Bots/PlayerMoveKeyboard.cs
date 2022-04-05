using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveKeyboard : MonoBehaviour
{
    private FreeMovementMotor motor;
    private Transform player;
    public GameObject cursorPrefab;

    public float cameraSmoothing = 0.05f;
    public float cameraPreview = 2f;

    public float cursor_PlaneHeight;
    public float cursor_FacingCamera = 1f;
    public float cursor_SmallerWithDistance = 0f;
    public float cursor_SmallerWhenClose = 1f;

    private Camera mainCamera;
    private Vector3 mainCamera_Velocity;
    private Vector3 mainCamera_Offset;
    private Vector3 initOffsetToPlayer;

    private Transform cursorObject;
    private Vector3 cursorScreenPosition;

    private Quaternion screenMovement_Space;
    private Vector3 screenMovement_Right;
    private Vector3 screenMovement_Forward;

    private Plane playerMovementPlane;

    void Awake()
    {
        motor = GetComponent<FreeMovementMotor>();
        motor.movementDirection = Vector3.zero;
        motor.facingDirection = Vector3.zero;

        mainCamera = Camera.main;

        cursorObject = (Instantiate(cursorPrefab) as GameObject).transform;

        player = transform;

        initOffsetToPlayer = mainCamera.transform.position - player.position;
        mainCamera_Offset = mainCamera.transform.position - player.position;

        cursorScreenPosition = new Vector3(0.5f * Screen.width, 0.5f * Screen.height, 0f);

        // ù��°���ڴ� ����� ��������(ĳ������ y������� ���� �ٶ󺸰ڴ�)
        // �ι�°���ڴ� Plane�� ������ ��ġ
        playerMovementPlane = new Plane(player.up, player.position + player.up * cursor_PlaneHeight);
    }

    void Start()
    {
        screenMovement_Space = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
        screenMovement_Forward = screenMovement_Space * Vector3.forward;
        screenMovement_Right = screenMovement_Space * Vector3.right;
    }

    void FixedUpdate()
    {
        HanglePlayerMovement();
    }

    private void HanglePlayerMovement()
    {
        motor.movementDirection = Input.GetAxis("Horizontal") * screenMovement_Right +
                                  Input.GetAxis("Vertical") * screenMovement_Forward;

        if (motor.movementDirection.sqrMagnitude > 1) motor.movementDirection.Normalize();

        playerMovementPlane.normal = player.up;
        playerMovementPlane.distance = -player.position.y + cursor_PlaneHeight;

        Vector3 cameraAdjustmentVector = Vector3.zero;
        Vector3 cursor_ScreenPosition = Input.mousePosition;

        Vector3 cursorWorldPosition = ScreenPointToWorldPointOnPlane(cursor_ScreenPosition, playerMovementPlane, mainCamera);

        float halfWidth = Screen.width / 2f;
        float halfHeight = Screen.height / 2f;
        float maxHalf = Mathf.Max(halfWidth, halfHeight);

        // screen position�� ���� ����� ��ġ
        Vector3 posRel = cursor_ScreenPosition - new Vector3(halfWidth, halfHeight, cursor_ScreenPosition.z);

        posRel.x /= maxHalf;
        posRel.y /= maxHalf;

        cameraAdjustmentVector = posRel.x * screenMovement_Right + posRel.y * screenMovement_Forward;
        cameraAdjustmentVector.y = 0f;

        HandleCursorAlignment(cursorWorldPosition);

        // Handle camera
        Vector3 cameraTargetPosition = player.position + initOffsetToPlayer + cameraAdjustmentVector * cameraPreview;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, // ������ġ
                                                          cameraTargetPosition, // �����Ϸ��� ��ġ
                                                          ref mainCamera_Velocity, // ����ӵ�
                                                          cameraSmoothing); // Ÿ�ٿ� �����ϱ� ���� �뷫���� �ð�. ���� ���ϼ��� Ÿ�ٿ� ������ ����

        mainCamera_Offset = mainCamera.transform.position - player.position;
    }

    private Vector3 ScreenPointToWorldPointOnPlane(Vector3 screenPoint, Plane plane, Camera myCamera)
    {
        Ray ray = myCamera.ScreenPointToRay(screenPoint);

        float dist;
        plane.Raycast(ray, out dist); // Physics.Raycast�ʹ� �ٸ��� �Ÿ��� ������
        return ray.GetPoint(dist); // ray.GetPoint(dist)�� ���콺�� Ŭ���� plane���� ����
    }

    private void HandleCursorAlignment(Vector3 cursorWorldPosition)
    {
        cursorObject.position = cursorWorldPosition;

        Quaternion cursor_WorldRotation = cursorObject.rotation;

        if (motor.facingDirection != Vector3.zero)
            cursor_WorldRotation = Quaternion.LookRotation(motor.facingDirection);

        Vector3 cursor_ScreenSpaceDirection = Input.mousePosition -
                                              mainCamera.WorldToScreenPoint(transform.position + player.up * cursor_PlaneHeight);

        cursor_ScreenSpaceDirection.z = 0f;

        Quaternion cursor_Bilboard_Rotation = mainCamera.transform.rotation *
                                              Quaternion.LookRotation(cursor_ScreenSpaceDirection, -Vector3.forward);

        cursorObject.rotation = Quaternion.Slerp(cursor_WorldRotation, cursor_Bilboard_Rotation, cursor_FacingCamera);

        float compensatedScale = 0.1f * Vector3.Dot(cursorWorldPosition - mainCamera.transform.position, mainCamera.transform.forward);

        // Mathf.Lerp�� ����°���ڰ��� ������� �Ǿ�����°��̰�
        // Mathf.InverseLerp�� ����°���ڰ��� ù��°���ڿ� �ι�°���� ���̿��� ������� ������ 0~1������ ������ ���ϵ�
        // Mathf.PingPong(Time.time, 3); 0���� 3���� �̵��ߴٰ� �ٽ� 0���� �̵� �ݺ�
        // Mathf.DeltaAngle(0, 90); �� ���� ������ ������ 90
        float cursor_Scale_Multiplier = Mathf.Lerp(0.7f, 1.0f, Mathf.InverseLerp(0.5f, 4.0f, motor.facingDirection.magnitude));

        cursorObject.localScale = Vector3.one * Mathf.Lerp(compensatedScale, 1f, cursor_SmallerWithDistance) * cursor_Scale_Multiplier;
    }

    //// CharacterController������Ʈ�� ���� ��ü�� �ٸ� ��ü�� �ε����� �� ȣ���
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (!characterController.isGrounded)
    //    {
    //        if (hit.collider.tag == "Wall") // �ε��� ���
    //        {
    //            if (verticalVelocity < -0.6f) wallSlide = true;
                
    //            if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                verticalVelocity = jumpForce;

    //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

    //                wallSlide = false;
    //            }
    //        }
    //    }
    //}
}

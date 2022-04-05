using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveKeyboard : MonoBehaviour
{
    private BaseMovement baseMovement;

    private CharacterAnimation playerAnimation;

    private Quaternion screenMovement_Space;

    private Vector3 screenMovement_Forward;
    private Vector3 screenMovement_Right;

    void Awake()
    {
        baseMovement = GetComponent<BaseMovement>();
        baseMovement.movementDirection = Vector3.zero;

        playerAnimation = GetComponent<CharacterAnimation>();
    }

    void OnEnable()
    {
        GMCameraAnimationsController.screenMovement += SetScreenMovement;
    }

    void Update()
    {
        MovementInput();
    }

    private void MovementInput()
    {
        baseMovement.movementDirection = Input.GetAxis(AxisManager.HORIZONTAL_AXIS) * screenMovement_Right +
                                         Input.GetAxis(AxisManager.VERTICAL_AXIS) * screenMovement_Forward;

        if (Input.GetAxis(AxisManager.HORIZONTAL_AXIS) != 0 || Input.GetAxis(AxisManager.VERTICAL_AXIS) != 0)
            playerAnimation.Walk(true);
        else
            playerAnimation.Walk(false);

        // �� �ڵ� ������ ���� �� ���� ������
        if (baseMovement.movementDirection.sqrMagnitude > 1) baseMovement.movementDirection.Normalize();
    }

    private void SetScreenMovement()
    {
        screenMovement_Space = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
        screenMovement_Forward = screenMovement_Space * Vector3.forward;
        screenMovement_Right = screenMovement_Space * Vector3.right;
        // Debug.Log(screenMovement_Space);
        // Debug.Log(screenMovement_Forward);
        // Debug.Log(screenMovement_Right);
    }

    // �÷��̾ ������ �ִϸ��̼��� �̺�Ʈ�� ��ϵ� �޼���
    private void PlayerDied()
    {
        EndGameManager.instance.GameOver(false);
        GetComponent<PlayerAttackInput>().enabled = false;
        enabled = false;
    }

    void OnDisable()
    {
        GMCameraAnimationsController.screenMovement -= SetScreenMovement;
    }
}

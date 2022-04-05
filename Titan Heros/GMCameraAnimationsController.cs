using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMCameraAnimationsController : MonoBehaviour
{
    private Animator anim;

    public GameObject mainCamera, camera_1, camera_2;

    public delegate void ScreenMovement();
    public static event ScreenMovement screenMovement;

    public CountdownController countdown;

    public Camera UI_Camera;
    public GameObject fight_FX;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void TurnOnCamera2()
    {
        camera_1.SetActive(false);
        camera_2.SetActive(true);

        anim.Play(AnimationTags.CAMERA_2_ANIMATION);

        // GameplayController.instance.SpawnPlayer();
    }

    public void TurnOnMainCamera()
    {
        camera_2.SetActive(false);
        mainCamera.SetActive(true);

        if (screenMovement != null) screenMovement();

        // nearClipPlane�� ī�޶� ���� �ִ� �ּҰŸ�
        // fight_FX ������ layer�� UI�� UI_Camera������Ʈ�� Depth only, UI
        Instantiate(fight_FX,
                    UI_Camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, UI_Camera.nearClipPlane * 4)),
                    Quaternion.identity);

        GameplayController.instance.SpawnEnemy(1);
    }

    private void StartCountdownAnimation()
    {
        countdown.StartCountdown();
    }
}

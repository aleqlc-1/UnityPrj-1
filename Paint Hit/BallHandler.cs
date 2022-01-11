using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public static Color oneColor;

    public static float rotationSpeed = 130f;
    public static float rotationTime = 3;

    public static int currentCircleNo;

    public GameObject ball;

    private float speed = 100f;

    private int ballsCount;
    private int circleNo;

    private Color[] ChangingColors;

    public SpriteRenderer spr;

    public Material splashMat;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) HitBall();
    }

    private void ResetGame()
    {
        ChangingColors = ColorScript.colorArray;
        oneColor = ChangingColors[0]; // �� ���� ����
        spr.color = oneColor; // ������ �� ����
        splashMat.color = oneColor; // ?

        // as GameObject �Ⱥ��̸� ������, (GameObject)Instantiate �̷������� �����ĳ��Ʈ ���൵��
        GameObject gameObject2 = Instantiate(Resources.Load("round06" + Random.Range(1, 6))) as GameObject;
        gameObject2.transform.position = new Vector3(0, 20, 23);
        gameObject2.name = "Circle" + circleNo;

        ballsCount = LevelsHandlerScript.ballsCount;

        currentCircleNo = circleNo;

        LevelsHandlerScript.currentColor = oneColor;
        MakeHurdles();
    }

    public void HitBall()
    {
        if (ballsCount <= 1) base.Invoke("MakeNewCircle", 0.4f);

        ballsCount--;

        GameObject gameObject = Instantiate<GameObject>(ball, new Vector3(0, 0, -8), Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.color = oneColor;

        // AddForce�� �� �ѹ� ���� �ִ°��̹Ƿ� ���ư��� ������
        // ������������ ���ư����Ϸ��� Time.deltaTime ���ؾ���
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void MakeNewCircle()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("circle"); // ���� ��ü
        GameObject gameObject = GameObject.Find("Circle" + this.circleNo);

        // 24��° �ڽİ�ü�� tube�� ���� ��� �ڽİ�ü������ ��Ȱ��ȭ
        for (int i = 0; i < 24; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        // ���� ��ü�� ������ BallHandler.oneColor�� ����
        gameObject.transform.GetChild(24).gameObject.GetComponent<MeshRenderer>().material.color = BallHandler.oneColor;

        if (gameObject.GetComponent<iTween>()) gameObject.GetComponent<iTween>().enabled = false; // ?

        // ���ݱ��� ������ ��� ������
        foreach (GameObject target in array)
        {
            iTween.MoveBy(target, iTween.Hash(new object[]
            {
                "y", // y���� ��������
                -2.98f, // -2.98��ŭ ��������
                "easetype",
                iTween.EaseType.spring, // �ణ ƨ����� ������
                "time",
                0.5
            }));
        }

        this.circleNo++;
        currentCircleNo = circleNo;

        GameObject gameObject2 = Instantiate(Resources.Load("round06" + Random.Range(1, 6))) as GameObject;
        gameObject2.transform.position = new Vector3(0, 20, 23);
        gameObject2.name = "Circle" + circleNo;

        ballsCount = LevelsHandlerScript.ballsCount;

        oneColor = ChangingColors[circleNo]; // �� ���� ����
        spr.color = oneColor; // ������ �� ����
        splashMat.color = oneColor; // ?

        LevelsHandlerScript.currentColor = oneColor;
        MakeHurdles();
    }

    private void MakeHurdles()
    {
        if (circleNo == 1) FindObjectOfType<LevelsHandlerScript>().MakeHurdles1();
        if (circleNo == 2) FindObjectOfType<LevelsHandlerScript>().MakeHurdles2();
        if (circleNo == 3) FindObjectOfType<LevelsHandlerScript>().MakeHurdles3();
        if (circleNo == 4) FindObjectOfType<LevelsHandlerScript>().MakeHurdles4();
        if (circleNo == 5) FindObjectOfType<LevelsHandlerScript>().MakeHurdles5();
    }
}

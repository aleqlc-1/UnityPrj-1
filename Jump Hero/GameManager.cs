using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject platform;

    private float minX = -2.5f, maxX = 2.5f, minY = -4.7f, maxY = -3.7f;
    private float lerpTime = 1.5f;
    private float lerpX;

    private bool lerpCamera;

    public Text scoreTxt;

    void Awake()
    {
        MakeInstance();
        CreateInitialPlatforms();
    }

    private void MakeInstance()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        if (lerpCamera)
        {
            LerpTheCamera();
        }
    }

    private void LerpTheCamera()
    {
        float x = Camera.main.transform.position.x;
        x = Mathf.Lerp(x, lerpX, lerpTime * Time.deltaTime);
        Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (Camera.main.transform.position.x >= (lerpX - 0.07f)) lerpCamera = false;
    }

    private void CreateInitialPlatforms()
    {
        // -2.5 ~ -1.3
        Vector3 temp = new Vector3(Random.Range(minX, minX + 1.2f), Random.Range(minY, maxY), 0);
        Instantiate(platform, temp, Quaternion.identity);

        temp.y += 2f;
        Instantiate(player, temp, Quaternion.identity);

        // 1.3 ~ 2.5
        temp = new Vector3(Random.Range(maxX, maxX - 1.2f), Random.Range(minY, maxY), 0);
        Instantiate(platform, temp, Quaternion.identity);
    }

    public void CreateNewPlatformAndLerp(float lerpPosition)
    {
        CreateNewPlatform();

        lerpX = lerpPosition + maxX;
        lerpCamera = true;
    }

    private void CreateNewPlatform()
    {
        float cameraX = Camera.main.transform.position.x;
        float newMaxX = cameraX + (maxX * 2); // ���ο� �÷����� ���� �÷����� ���ļ� �������� �ʰ� 2 ������
        Instantiate(platform, new Vector3(Random.Range(newMaxX, newMaxX - 1.2f), Random.Range(maxY, maxY - 1.2f), 0), Quaternion.identity);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}

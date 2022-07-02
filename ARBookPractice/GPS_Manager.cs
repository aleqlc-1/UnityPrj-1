using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPS_Manager : MonoBehaviour
{
    public static GPS_Manager instance;

    public Text latitude_text;
    public Text longitude_text;

    public float latitude = 0;
    public float longitude = 0;

    public float maxWaitTime = 10.0f; // �ִ� ���� ���ð�
    private float waitTime = 0f; // ���� ����� ���ð�

    public float resendTime = 1.0f;
    public bool receiveGPS = false;

	void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        StartCoroutine(GPS_On());
    }
    public IEnumerator GPS_On()
	{
        // GPS����㰡�� �������ߴٸ� �����㰡 �˾��� ����
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
		{
            Permission.RequestUserPermission(Permission.FineLocation);
            
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
			{
                yield return null;
			}
		}

        // ������� GPS ��ġ�� ���������� ��ġ������ ������ �� ���ٰ� ǥ���ϰ� �ڷ�ƾ����
        if (!Input.location.isEnabledByUser)
		{
            latitude_text.text = "GPS Off";
            longitude_text.text = "GPS Off";
            yield break;
		}

        // ��ġ������ ��û -> ���Ŵ��
        Input.location.Start();

        // GPS ���Ż��°� �ʱ���¿��� maxWaitTime���� ����Ѵ�.
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
		{
            yield return new WaitForSeconds(1.0f);
            waitTime++;
		}

        // ���Ž��� �� �������
        if (Input.location.status == LocationServiceStatus.Failed)
		{
            latitude_text.text = "��ġ ���� ���� ����";
            longitude_text.text = "��ġ ���� ���� ����";
		}

        // ������ð� �ʰ���
        if (waitTime > maxWaitTime)
		{
            latitude_text.text = "���� ��� �ð� �ʰ�";
            longitude_text.text = "���� ��� �ð� �ʰ�";
        }

        // ���ŵ� GPS �����͸� ȭ�鿡 ���
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "���� : " + latitude.ToString();
        longitude_text.text = "�浵 : " + longitude.ToString();

        // ù���Žõ� ���� while�� ��� ���鼭 resendTime���� ���������ִ��� üũ�Ͽ� ������� ����
        receiveGPS = true;
        while (receiveGPS)
		{
            yield return new WaitForSeconds(resendTime);
            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;
            latitude_text.text = "���� : " + latitude.ToString();
            longitude_text.text = "�浵 : " + longitude.ToString();
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public class ImageGPSData
{
    public string name;
    public float latitude;
    public float longitude;
    public bool isCaptured = false;

    // ������
    public ImageGPSData(string objName, float lat, float lon, bool captured)
    {
        name = objName;
        latitude = lat;
        longitude = lon;
        isCaptured = captured;
    }
}

public class DB_Manager : MonoBehaviour
{
    public static DB_Manager instance;

    private Vector2 currentPos;
    private string objectName = "";
    private string currentKey = "";
    private bool isSearch = false;

    public string databaseUrl = "https://myarproject-dc1fa-default-rtdb.firebaseio.com/";

	void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        // DB�� URL�� ����
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(databaseUrl);
        
        // ������ �����Լ� ����
        // SaveData();
    }

    private void SaveData()
	{
        // ����� Ŭ���� ���� ����
        ImageGPSData data1 = new ImageGPSData("Cat", 37.48985f, 126.9601f, false);
        ImageGPSData data2 = new ImageGPSData("SCar", 37.47811f, 126.95151f, false);

        // Ŭ���� ������ Json �����ͷ� �����ϱ�
        string jsonCat = JsonUtility.ToJson(data1);
        string jsonSCar = JsonUtility.ToJson(data2);

        // DB�� �ֻ��(Root) ���͸��� �����Ѵ�.
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.RootReference;

        // �ֻ�� ���͸��� �������� ���� ���͸��� ������ json �����͸� DB�� �����Ѵ�.
        refData.Child("Markers").Child("Data1").SetRawJsonValueAsync(jsonCat);
        refData.Child("Markers").Child("Data2").SetRawJsonValueAsync(jsonSCar);

        print("������ ���� �Ϸ�!");
	}

    // �����ͺ��̽� �˻��Լ�
    public IEnumerator LoadData(Vector2 myPos, Transform trackedImage)
	{
        // ���� ���� ��ġ�� ����
        currentPos = myPos;

        // �����͸� �о�������� ���س�带 ����
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference("Markers");

        // DB�κ��� ������ �޾ƿ���
        isSearch = true;
        refData.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("DB���� �����͸� �������� �� �����߽��ϴ�.");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("DB���� �����͸� �������� ���� ��ҵƽ��ϴ�.");
            }
            else if (task.IsCompleted)
            {
                // DB�κ��� �����͸� �����´�.
                DataSnapshot snapShot = task.Result;

                // ��ü �����͸� ��ȸ�Ѵ�.
                foreach (DataSnapshot data in snapShot.Children)
                {
                    // ������ �����͸� Json �����ͷ� ��ȯ�Ѵ�.
                    string myData = data.GetRawJsonValue();

                    // Json �����͸� ImageGPSData ������ �����Ѵ�.
                    ImageGPSData myClassData = JsonUtility.FromJson<ImageGPSData>(myData);

                    // ����, �������� ��ȹ���� �ʾҴٸ�
                    if (!myClassData.isCaptured)
                    {
                        // DB �����Ϳ� ����� ��ġ�� ������� ���� ��ġ���� �Ÿ��� ����
                        Vector2 dataPos = new Vector2(myClassData.latitude, myClassData.longitude);

                        float distance = Vector2.Distance(currentPos, dataPos);

                        // �Ÿ� ���̰� 0.001 �̳���� ������ �������� �̸��� DB Ű ���� �����Ѵ�.
                        if (distance < 0.001f)
                        {
                            objectName = myClassData.name;
                            currentKey = data.Key;
                        }
                    }
                }
            }

            isSearch = false;
        });

        // DB�κ��� �����͸� �޾ƿ��� ���ȿ��� �Լ� ������ �ߴ�
        while (isSearch)
		{
            yield return null;
		}

        // Resouce �������� objectName�� �̸��� ������ �̸��� �������� ã�´�.
        GameObject imagePrefab = Resources.Load<GameObject>(objectName);

        // ����, �˻��� �������� �����ϰ�
        if (imagePrefab != null)
		{
            // �̹����� ��ϵ� �ڽİ�ü�� ���ٸ�
            if (trackedImage.transform.childCount < 1)
			{
                // �̹��� ��ġ�� �������� �����ϰ� �̹����� �ڽİ�ü�� ���
                GameObject go = Instantiate(imagePrefab, trackedImage.position, trackedImage.rotation);
                go.transform.SetParent(trackedImage.transform);
			}
		}
	}

    // ��ȹ ������ DB ���� �Լ�
    public void UpdateCaptured()
	{
        // Ű ���� ������ ��θ� ������ DB�� Ư�� ��带 �����Ѵ�
        string dataPath = "/Markers/" + currentKey + "/isCaptured";
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference(dataPath);

        if (refData != null)
		{
            // ���� ������ ����� ���� false���� true�� ����
            refData.SetValueAsync(true);
		}
	}
}
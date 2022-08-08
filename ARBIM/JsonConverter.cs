using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// ���� json �׽�Ʈ
public class TargetClass
{
	public int id = 1;
    public string name = "asd";
}

// ���� json �׽�Ʈ
[System.Serializable]
public class TargetClass_Multi
{
	public List<int> id = new List<int>();
	public List<string> name = new List<string>();
	public int power;
	public string stat;
}


public class JsonConverter : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			ConvertToJson();
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			ConvertToClass();
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			ConvertToMultiClassAndMakeJson();
		}
	}

	// �ʹ� �ʰ� ������
	public void ConvertToJson()
	{
		// json ��������
		TargetClass targetClass = new TargetClass();
		string json = JsonUtility.ToJson(targetClass);
		Debug.Log(json);

		// json���ϻ���
		string fileName = "Target";
		string path = Application.dataPath + "/" + fileName + ".Json"; // Assets ������ ����
		if (!File.Exists(path))
		{
			return;
		}
		File.WriteAllText(path, json);

		//// byte�� �����Ἥ json���ϻ���
		//TargetClass targetClass = new TargetClass();
		//string json = JsonUtility.ToJson(targetClass);
		//Debug.Log(json);
		//string fileName = "Target";
		//string path = Application.dataPath + "/" + fileName + ".Json"; // Assets ������ ����
		//FileStream fileStream = new FileStream(path, FileMode.Create);
		//byte[] data = Encoding.UTF8.GetBytes(json);
		//fileStream.Write(data, 0, data.Length);
		//fileStream.Close();
	}

	// path�� �ִ� Json������ Class�� ��ȯ
	public void ConvertToClass()
	{
		//// �����б�
		//string fileName = "Target";
		//string path = Application.dataPath + "/" + fileName + ".Json";
		//string json = File.ReadAllText(path);

		//TargetClass targetClass = JsonUtility.FromJson<TargetClass>(json);
		//Debug.Log(targetClass.id); // int
		//Debug.Log(targetClass.name); // string

		// ����Ʈ�� �б�
		string fileName = "Target";
		string path = Application.dataPath + "/" + fileName + ".Json";
		if (!File.Exists(path))
		{
			return;
		}
		FileStream fileStream = new FileStream(path, FileMode.Open);
		byte[] data = new byte[fileStream.Length];
		fileStream.Read(data, 0, data.Length);
		fileStream.Close();
		string json = Encoding.UTF8.GetString(data);
		TargetClass targetClass = JsonUtility.FromJson<TargetClass>(json);

		Debug.Log(targetClass.id);
		Debug.Log(targetClass.name);
	}

	public void ConvertToMultiClassAndMakeJson()
	{
		string fileName = "Target";
		string path = Application.dataPath + "/" + fileName + ".Json";
		string loadJson = File.ReadAllText(path); // path�� �ִ� json���� �о��
		Debug.Log(loadJson);
		if (!File.Exists(path))
		{
			return;
		}
		
		// �� ����� loadJson�� {"Ű1" : ["��1", "��2", "��3"], "Ű2" : [int��1, int��2, int��3], "Ű3" : int��, "Ű4" : int��} �϶�
		// �ҷ��� loadJson������ targetClass_Multi�� ����
		TargetClass_Multi targetClass_Multi = JsonUtility.FromJson<TargetClass_Multi>(loadJson);

		// json���Ϸκ��� �ҷ��� �����͸� class�� ������ ������ ����
		if (targetClass_Multi != null)
		{
			Debug.Log(targetClass_Multi.id.Count);
			for (int i = 0; i < targetClass_Multi.id.Count; i++)
			{
				Debug.Log(targetClass_Multi.id[i] + " : " + targetClass_Multi.name[i]);
			}

			Debug.Log(targetClass_Multi.power);
			Debug.Log(targetClass_Multi.stat);
		}

		//string json = JsonUtility.ToJson(targetClass_Multi, true); // class�� �ٽ� json����
		//File.WriteAllText(path, json); // path�� json���� ����
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class FtpManager : MonoBehaviour
{
    private string ftpPath = "ftp://jhchoi@127.0.0.1/";
    private string fileName = "FromUnityFbx1.fbx"; // ���ε��ҽ� ���� ������ ���ϸ�, ���� �̹� �ִ��̸��̸� �������
    private string userName = "jhchoi";
    private string pwd = "fgftd4";
    private string UploadDirectory = "UploadTest1";

	private void Start()
	{
        Debug.Log(Application.persistentDataPath);
	}
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
		{
            FtpMakeFolder();
        }

        if (Input.GetKeyDown(KeyCode.U))
		{
            UploadFile(ftpPath, fileName, userName, pwd, UploadDirectory);
		}

        if (Input.GetKeyDown(KeyCode.D))
		{
            FtpDownload();
		}

        if (Input.GetKeyDown(KeyCode.T))
		{
            ConvertInUnity();
		}

        if (Input.GetKeyDown(KeyCode.I))
		{
            FileMoveAndInstantiate();
        }

        if (Input.GetKeyDown(KeyCode.V))
		{
            UploadFileFromUnity(ftpPath, fileName, userName, pwd, UploadDirectory);
        }
    }

    // ���������
    private void FtpMakeFolder()
    {
        string userName = "jhchoi";
        string pwd = "fgftd4";
        string folderName = "testFolder"; // ���θ��� �����̸�

        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://jhchoi@127.0.0.1/UploadTest1/test.txt/" + folderName);
        request.Credentials = new NetworkCredential(user, pwd);
        request.UsePassive = true;
        request.UseBinary = true;
        request.KeepAlive = false;
        request.Method = WebRequestMethods.Ftp.MakeDirectory; // ���������

        try
        {
            FtpWebResponse res = (FtpWebResponse)request.GetResponse(); // ������ ����
            //m_lUploadList.Add(URL + folderName); // ����� ����Ʈ�� �߰�
        }
        catch (WebException ex)
        {
            // ����ó��.
            FtpWebResponse response = (FtpWebResponse)ex.Response;

            switch (response.StatusCode)
            {
                case FtpStatusCode.ActionNotTakenFileUnavailable:
                    {
                        Debug.Log("CreateFolders ] Probably the folder already exist : " + folderName);
                    }
                    break;
            }
        }
    }

    // ���Ͼ��ε�(����pc -> ftp)
    private string UploadFile(string ftpPath, string fileName, string userName, string pwd, string UploadDirectory = "")
	{
        string PureFileName = new FileInfo(fileName).Name;
        string uploadPath = string.Format("{0}/{1}/{2}", ftpPath, UploadDirectory, PureFileName);
        Debug.Log(uploadPath);
        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uploadPath);
        req.Proxy = null;
        req.Method = WebRequestMethods.Ftp.UploadFile;
        req.Credentials = new NetworkCredential(userName, pwd);
        req.UseBinary = true;
        req.UsePassive = true;

        byte[] data = File.ReadAllBytes(@"C:\Users\Desktop\ConvertResult\202_84A_Type_skp_d.txt"); // ftp������ ���ε��� ���������ּ�
        req.ContentLength = data.Length;
        Stream stream = req.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();

        FtpWebResponse res = (FtpWebResponse)req.GetResponse();
        return res.StatusDescription;
	}

    // ���ϴٿ�ε�(ftp -> ����pc)
    private void FtpDownload()
    {
        FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create("ftp://jhchoi@127.0.0.1/UploadTest1/fbxToTxt.txt"); // ftp�������� �ٿ�ε��� �����ּ�
        ftpWebRequest.Credentials = new NetworkCredential(userName, pwd);
        ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

        using (var localfile = File.Open(Application.persistentDataPath + "/fbxtotxt.txt", FileMode.Create)) // ������ ��������� �����Ͽ� ��������
        using (var ftpStream = ftpWebRequest.GetResponse().GetResponseStream())
        {
            byte[] buffer = new byte[1024];
            int n;
            while ((n = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                localfile.Write(buffer, 0, n);
            }
        }
    }

    // ����Ƽ ���ο��� ftp�� ���ε�
    private string UploadFileFromUnity(string ftpPath, string fileName, string userName, string pwd, string UploadDirectory = "")
    {
        string PureFileName = new FileInfo(fileName).Name;
        string uploadPath = string.Format("{0}/{1}/{2}", ftpPath, UploadDirectory, PureFileName);
        Debug.Log(uploadPath);
        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uploadPath);
        req.Proxy = null;
        req.Method = WebRequestMethods.Ftp.UploadFile;
        req.Credentials = new NetworkCredential(userName, pwd);
        req.UseBinary = true;
        req.UsePassive = true;

        byte[] data = File.ReadAllBytes("Assets/Resources/ConvertResult.fbx"); // ftp������ ���ε��� ����Ƽ������ �����ּ�
        req.ContentLength = data.Length;
        Stream stream = req.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();

        FtpWebResponse res = (FtpWebResponse)req.GetResponse();
        return res.StatusDescription;
    }

    // ����Ƽ ���ο��� �������ĺ�ȯ �� Resources������ ����
    private void ConvertInUnity()
	{
		// ����Ƽ�ȿ��� �������� ��ȯ
		string myFile = Application.persistentDataPath + "/fbxtotxt.txt";
		string newFile = Application.persistentDataPath + "/fbxtotxt.fbx";
		FileInfo f = new FileInfo(myFile);
		f.MoveTo(Path.ChangeExtension(newFile, ".fbx"));
	}

    // ��ȯ�ð� �����ɸ��Ƿ� ftp������ ��ȯ�� ���� �����ϰ� fbx�� www�� �ҷ�����
    private void FileMoveAndInstantiate()
	{
        File.Move(Application.persistentDataPath + "/fbxtotxt.fbx", "Assets/Resources/fbxtotxt.fbx");
        GameObject go = Resources.Load("fbxtotxt") as GameObject;
        Instantiate(go);
    }
}

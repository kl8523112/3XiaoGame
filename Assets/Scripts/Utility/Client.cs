using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

public class Client : MonoBehaviour
{
    public static string host = "47.94.251.161";
    //private static string IpStr = "47.94.251.161";
    public static int port = 6321;
    //private static string EndToken = "%END";

    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public List<Score> scoreList;
    public MenuManager mm;

    public Score score;

    public delegate void OnConnectedToServerHandler();
    OnConnectedToServerHandler OnConnectedHandler=null;

    private void Start()
    {
        
    }

    public void Init()
    {
        scoreList = GameObject.Find("Canvas").GetComponent<MenuManager>().scoreList;
        mm = GameObject.Find("Canvas").GetComponent<MenuManager>();
    }

    ~Client()
    {
        CloseSocket();
    }
    void OnDestroy()
    {
        CloseSocket();
    }

    public void ConnectToServer(OnConnectedToServerHandler Method)
    {
        Thread connectThread = new Thread(ConnectServer);
        OnConnectedHandler += Method;
        connectThread.Start();
    }

    void ConnectServer()
    {
        #region 如果已经连上了，不再做连接
        if (socketReady)
            return;
        #endregion

        #region 创建socket 


        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();       //得到socket中获取到的数据流  
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);  //初始化读和写  
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e.Message);
        }
        #endregion

        if(OnConnectedHandler!=null)
        OnConnectedHandler();
    }

    public void SendMessageToServer(string data)
    {
        if (!socketReady) return;
        Debug.Log("客户端发送数据:" + data);
        writer.WriteLine(data);
        writer.Flush();
    }

    public void SendEndToken()
    {
        if (!socketReady) return;
        //writer.WriteLine(EndToken);
        //writer.Flush();
    }

    void Update()
    {
        if (!socketReady)
            return;
        if (stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
                OnIncomingData(data);
        }
    }

    private void OnIncomingData(string data)
    {
        Debug.Log("接收到服务器数据:" + data);
        if (data.Contains("|"))
        {
            Debug.Log("接收到分数！");
            data=data.Remove(data.Length-1);
            string[] message = data.Split(',');
            for (int i = 0; i < message.Length; i++)
            {
                string[] scoreString = message[i].Split('|');
                Score score = new Score(scoreString[0], int.Parse(scoreString[1]));
                Debug.Log(scoreString[0] + " " + scoreString[1]);
                scoreList.Add(score);
            }
            mm.ShowRanking();
            scoreList.Clear();
        }
    }

    public void CloseSocket()
    {
        if (!socketReady) return;
        Debug.Log("关闭连接");

        SendEndToken();

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;

    }

    internal void Init(object v)
    {
        throw new NotImplementedException();
    }

    public void SendMessageGet()
    {
        SendMessageToServer("%get");
    }

    public void SendMessageScore()
    {
       SendMessageToServer(score.name + "|" + score.score.ToString());
    }
}

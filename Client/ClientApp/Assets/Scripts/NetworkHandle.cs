using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandle : MonoBehaviour {

    [SerializeField]
    private int sendPort = 1304;
    [SerializeField]
    private int listenPort = 1305;

    [SerializeField]
    private int RefreshRate = 5;

    private int RefreshTimer;

    private PlayerController pc;
    private RemotePlayerController rpc;

    private void Start()
    {
        RefreshTimer = RefreshRate;
    }

    void Update()
    {
        Debug.Log(RefreshTimer);
        RefreshTimer -= 1;
        if(RefreshTimer <= 0)
        {
            if (pc)
                TransmitPacket();
            else
                pc = FindObjectOfType<PlayerController>();

            if (rpc)
                RetrievePacket();
            else
                rpc = FindObjectOfType<RemotePlayerController>();

            RefreshTimer = RefreshRate;
        }
    }

    void TransmitPacket()
    {
        Boolean exception_thrown = false;

        // Create the socket to send from
        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Create an address object used to store the IP of the target
        IPAddress send_to_address = IPAddress.Parse("10.40.60.249");

        // Create a target to send to
        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, sendPort);

        // the socket object must have an array of bytes to send.
        // this loads the string entered by the user into an array of bytes.
        byte[] send_buffer = new byte[64];
        kf.MemBlock mb = new kf.MemBlock(send_buffer);

        // Store Position
        mb.setFloat(pc.gameObject.transform.position.x);
        mb.setFloat(pc.gameObject.transform.position.y);
        mb.setFloat(pc.gameObject.transform.position.z);

        // Store Rotation
        mb.setFloat(pc.Chara.gameObject.transform.eulerAngles.y);

        // Remind the user of where this is going.
        Debug.Log ("sending to address: {0} port: {1}" + sending_end_point.Address.ToString() + sending_end_point.Port.ToString());
        try
        {
            sending_socket.SendTo(send_buffer, sending_end_point);
        }
        catch (Exception send_exception)
        {
            exception_thrown = true;
            Debug.Log(" Exception {0}" + send_exception.Message);
        }
        if (exception_thrown == false)
        {
            Debug.Log("Message has been sent to the broadcast address");
        }
        else
        {
            exception_thrown = false;
            Console.WriteLine("The exception indicates the message was not sent.");
        }
    }

    void RetrievePacket()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
        byte[] receive_byte_array;
        try
        {
            Debug.Log("Waiting for broadcast");

            if (listener.Available > 0)
            {
                receive_byte_array = listener.Receive(ref groupEP);
                Debug.Log("Received a broadcast from {0}" + groupEP.ToString());

                // the socket object must have an array of bytes to send.
                // this loads the string entered by the user into an array of bytes.
                kf.MemBlock mb = new kf.MemBlock(receive_byte_array);

                // Set Position
                Vector3 NewPos = new Vector3(mb.getFloat(), mb.getFloat(), mb.getFloat());

                rpc.gameObject.transform.position = NewPos;

                // Set Rotation
                Vector3 NewRot = new Vector3(0.0f, mb.getFloat(), 0.0f);
                rpc.gameObject.transform.eulerAngles = NewRot;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        listener.Close();
    }
}

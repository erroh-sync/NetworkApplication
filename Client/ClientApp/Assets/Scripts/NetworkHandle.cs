using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandle : MonoBehaviour {

    [SerializeField]
    // TEMP: SHOULD BE READ FROM A DATA SHEET
    private const int listenPort = 1305;

    [SerializeField]
    // TEMP: SHOULD BE READ FROM A DATA SHEET
    private const int RefreshRate = 20;

    private int RefreshTimer = RefreshRate;

    private PlayerController pc;
    private RemotePlayerController rpc;

    public struct GamePacket
    {
        int rotation; // 0 to 360
        private Vector3 position; // Position
    }

    void Update()
    {
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
        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, listenPort);

        //TODO: FORM PACKET HERE
        string text_to_send = "Bepis bepis in my conke";

        // the socket object must have an array of bytes to send.
        // this loads the string entered by the user into an array of bytes.
        byte[] send_buffer = Encoding.ASCII.GetBytes(text_to_send); // TODO: Send packet using Greg's stuff-to-byte thing

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
        string received_data;
        byte[] receive_byte_array;
        try
        {
            Debug.Log("Waiting for broadcast");

            if (listener.Available > 0)
            {
                receive_byte_array = listener.Receive(ref groupEP);
                Debug.Log("Received a broadcast from {0}" + groupEP.ToString());
                received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                Debug.Log("data follows {0}" + received_data);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        listener.Close();
    }
}

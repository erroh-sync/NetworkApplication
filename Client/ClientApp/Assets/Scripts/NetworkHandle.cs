﻿using System;
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
        #region comments
        // Create a socket object. This is the fundamental device used to network
        // communications. When creating this object we specify:
        // Internetwork: We use the internet communications protocol
        // Dgram: We use datagrams or broadcast to everyone rather than send to
        // a specific listener
        // UDP: the messages are to be formated as user datagram protocal.
        // The last two seem to be a bit redundant.
        #endregion
        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
        ProtocolType.Udp);
        #region comments 
        // create an address object and populate it with the IP address that we will use
        // in sending at data to. This particular address ends in 255 meaning we will send
        // to all devices whose address begins with 192.168.2.
        // However, objects of class IPAddress have other properties. In particular, the
        // property AddressFamily. Does this constructor examine the IP address being
        // passed in, determine that this is IPv4 and set the field. If so, the notes
        // in the help file should say so.
        #endregion
        IPAddress send_to_address = IPAddress.Parse("10.40.60.249");
        #region comments
        // IPEndPoint appears (to me) to be a class defining the first or final data
        // object in the process of sending or receiving a communications packet. It
        // holds the address to send to or receive from and the port to be used. We create
        // this one using the address just built in the previous line, and adding in the
        // port number. As this will be a broadcase message, I don't know what role the
        // port number plays in this.
        #endregion
        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, listenPort);
        #region comments
        // The below three lines of code will not work. They appear to load
        // the variable broadcast_string witha broadcast address. But that
        // address causes an exception when performing the send.
        //
        //string broadcast_string = IPAddress.Broadcast.ToString();
        //Console.WriteLine("broadcast_string contains {0}", broadcast_string);
        //send_to_address = IPAddress.Parse(broadcast_string);
        #endregion

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
            /*
            // this is the line of code that receives the broadcase message.
            // It calls the receive function from the object listener (class UdpClient)
            // It passes to listener the end point groupEP.
            // It puts the data from the broadcast message into the byte array
            // named received_byte_array.
            // I don't know why this uses the class UdpClient and IPEndPoint like this.
            // Contrast this with the talker code. It does not pass by reference.
            // Note that this is a synchronous or blocking call.
            */
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

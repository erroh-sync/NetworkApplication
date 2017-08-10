#include <iostream>
#include <winsock2.h>
#include <mswsock.h>

using namespace std;

class CursorInfo
{
public:
	CursorInfo() :m_posX(0), m_posY(0), m_data(0)
	{
	}
	unsigned short m_posX;
	unsigned short m_posY;
	unsigned char m_data;
};

class Packet
{
public:
	enum
	{
		e_pixel = 1,		// Client to server. Draw a pixel.
		e_line,				// Client to server. Draw a line.
		e_box,				// Client to server. Draw a box.
		e_circle,			// Client to server. Draw a circle.
		e_clientAnnounce,	// Client to server. Client announces that it exists. Server responds with Server Info packet containing window size.
		e_clientCursor,		// Client to server. Current cursor position sent to the server. Server responds with Server Cursors
		e_serverInfo,		// Server to client. Contains server window resolution.
		e_serverCursors		// Server to client. Contains an array of every client cursor value.
	};
	int type;
};

class PacketPixel : public Packet
{
public:
	int x;
	int y;
	float r;
	float g;
	float b;
};

class PacketBox : public Packet
{
public:
	int x;
	int y;
	int w;
	int h;
	float r;
	float g;
	float b;
};

class PacketLine : public Packet
{
public:
	int x1;
	int y1;
	int x2;
	int y2;
	float r;
	float g;
	float b;
};

class PacketCircle : public Packet
{
public:
	int x;
	int y;
	int radius;
	float r;
	float g;
	float b;
};

class PacketClientCursor : public Packet
{
public:
	CursorInfo cursor;
};

class PacketClientAnnounce : public Packet
{
public:

};

class PacketServerInfo : public Packet
{
public:
	unsigned short width;
	unsigned short height;
};

class PacketServerCursors : public Packet
{
public:
	unsigned short count;
	CursorInfo cursor[1];
};

int main()
{
	int portNum;
	const char* ip = "10.40.60.249";

	cin >> portNum;

	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		cout << "WSAStartup failed." << endl;
		return 1;
	}

	sockaddr_in send_address;
	send_address.sin_family = AF_INET;
	send_address.sin_port = htons(portNum);
	send_address.sin_addr.s_addr = inet_addr(ip);

	sockaddr_in recv_address;
	recv_address.sin_family = AF_INET;
	recv_address.sin_port = htons(portNum);
	recv_address.sin_addr.s_addr = INADDR_ANY;

	SOCKET send_socket = socket(AF_INET, SOCK_DGRAM, 0);
	if (send_socket == SOCKET_ERROR)
	{
		cout << "Error Opening socket: Error " << WSAGetLastError();
		return 1;
	}

	DWORD dwBytesReturned = 0;
	BOOL bNewBehavior = FALSE;
	WSAIoctl(send_socket,SIO_UDP_CONNRESET,&bNewBehavior,sizeof(bNewBehavior),NULL,0,&dwBytesReturned,NULL,NULL);
	
	bool running = true;

	while (running) {
		char *buffer = "Test data";
		int result;
		result = sendto(send_socket, // the client's socket
			buffer, // address of data to be send
			(int)strlen(buffer) + 1, // length to send
			0, // flags
			(SOCKADDR*)&send_address, // the destination address
			sizeof(send_address)); // size of the address structure

		if (result == SOCKET_ERROR)
			cout << "sendto() failed: Error " << WSAGetLastError() << endl;

		char input;
		cin >> input;
		if(input == 'stop')
			running = false;
	}

	return 0;
}
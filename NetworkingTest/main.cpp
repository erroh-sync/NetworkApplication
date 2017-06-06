#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <winsock2.h>
#include <mswsock.h>
#include <stdio.h>
#include <math.h>

#pragma pack(push,1)
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

#pragma pack(pop)

int main() {
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0){
		return 1;
	}

	sockaddr_in server_address;
	server_address.sin_family = AF_INET;
	server_address.sin_port = htons(1300);
	server_address.sin_addr.s_addr = inet_addr("255.255.255.255");

	SOCKET sock = socket(AF_INET, SOCK_DGRAM, 0);

	DWORD dwBytesReturned = 0;
	BOOL bNewBehavior = FALSE;
	WSAIoctl(sock,
		SIO_UDP_CONNRESET,
		&bNewBehavior,
		sizeof(bNewBehavior),
		NULL,
		0,
		&dwBytesReturned,
		NULL,
		NULL);

	BOOL bOptVal = TRUE;
	setsockopt(sock,
		SOL_SOCKET,
		SO_BROADCAST,
		(const char *)&bOptVal,
		sizeof(BOOL));

	PacketClientAnnounce pca;
	pca.type = Packet::e_clientAnnounce;
	int result = sendto(sock, (const char*)&pca, sizeof(pca), 0, (SOCKADDR*)&server_address, sizeof(server_address));

	for (int i = 0; i < 50; i++)
	{
		PacketCircle circle;
		circle.type = Packet::e_circle;
		circle.x = 100 + i * 10;
		circle.y = 75 + sin(i) * 10;
		circle.radius = 10;
		circle.r = 0.25 + (i/50.0) * (0 - 0.25);
		circle.g = 1 + (i / 50.0) * (0 - 1);
		circle.b = 0 + (i/50.0) * (1 - 0);

		sendto(sock, (const char*) &circle, sizeof(circle), 0, (SOCKADDR*)&server_address, sizeof(server_address));
	}

	return 0;
}
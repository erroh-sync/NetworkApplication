#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <winsock2.h>
#include <mswsock.h>

int main() {
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)

	{

		return 1;

	}

	sockaddr_in server_address;

	server_address.sin_family = AF_INET;

	server_address.sin_port = htons(1360);

	server_address.sin_addr.s_addr = inet_addr("10.40.60.78");

	SOCKET sock = socket(AF_INET, SOCK_DGRAM, 0);

	char *buffer = "HACKING THE MAINFRAME";

	int result;

	for (int i = 0; i < 1000000; i++)
	{
		result = sendto(sock, // the client's socket

			buffer, // address of data to be send

			(int)strlen(buffer) + 1, // length to send

			0, // flags

			(SOCKADDR*)&server_address, // the destination address

			sizeof(server_address)); // size of the address structure
	}

	return 0;
}
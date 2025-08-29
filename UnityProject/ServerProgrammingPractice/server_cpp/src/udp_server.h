#pragma once
#include <winsock2.h>
#include <ws2tcpip.h>

class UDPServer {
public:
    UDPServer(unsigned short port);
    ~UDPServer();
    void run();

private:
    SOCKET sock;
    unsigned short port;
};

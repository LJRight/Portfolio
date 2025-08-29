#pragma once
#include <winsock2.h>
#include <ws2tcpip.h>
#include <string>

class TCPServer {
public:
    TCPServer(unsigned short port);
    ~TCPServer();
    void run();

private:
    SOCKET listenSock;
    unsigned short port;
};

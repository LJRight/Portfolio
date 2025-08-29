#include "tcp_server.h"
#include <iostream>
#include <thread>

TCPServer::TCPServer(unsigned short p) : port(p) {
    listenSock = socket(AF_INET, SOCK_STREAM, 0);
    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = INADDR_ANY;
    addr.sin_port = htons(port);

    bind(listenSock, (sockaddr*)&addr, sizeof(addr));
    listen(listenSock, SOMAXCONN);
    std::cout << "[TCP] Listening on " << port << "\n";
}

TCPServer::~TCPServer() {
    closesocket(listenSock);
}

void TCPServer::run() {
    while (true) {
        SOCKET clientSock = accept(listenSock, nullptr, nullptr);
        if (clientSock == INVALID_SOCKET) continue;

        std::thread([clientSock]() {
            char buf[1024];
            int n;
            while ((n = recv(clientSock, buf, sizeof(buf), 0)) > 0) {
                send(clientSock, buf, n, 0); // echo
            }
            closesocket(clientSock);
        }).detach();
    }
}

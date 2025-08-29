#include "udp_server.h"
#include <iostream>

UDPServer::UDPServer(unsigned short p) : port(p) {
    sock = socket(AF_INET, SOCK_DGRAM, 0);
    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = INADDR_ANY;
    addr.sin_port = htons(port);
    bind(sock, (sockaddr*)&addr, sizeof(addr));
    std::cout << "[UDP] Listening on " << port << "\n";
}

UDPServer::~UDPServer() {
    closesocket(sock);
}

void UDPServer::run() {
    char buf[1024];
    sockaddr_in client{};
    int clen = sizeof(client);
    while (true) {
        int n = recvfrom(sock, buf, sizeof(buf), 0, (sockaddr*)&client, &clen);
        if (n > 0) {
            sendto(sock, buf, n, 0, (sockaddr*)&client, clen); // echo
        }
    }
}

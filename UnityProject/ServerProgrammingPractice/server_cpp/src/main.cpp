#include "tcp_server.h"
#include "udp_server.h"
#include <thread>

int main() {
    WSADATA wsa;
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) {
        printf("WSAStartup failed\n");
        return 1;
    }

    std::thread tcpThread([](){
        TCPServer server(20000);
        server.run();
    });

    std::thread udpThread([](){
        UDPServer server(20001);
        server.run();
    });

    tcpThread.join();
    udpThread.join();

    WSACleanup();
    return 0;
}

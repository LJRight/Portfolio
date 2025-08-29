cmake_minimum_required(VERSION 3.20)
project(GameServer LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(GameServer
    src/main.cpp
    src/tcp_server.cpp
    src/udp_server.cpp
)

target_link_libraries(GameServer PRIVATE ws2_32)
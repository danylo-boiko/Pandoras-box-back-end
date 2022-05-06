import grpc
from concurrent import futures
from configparser import ConfigParser
from nsfw_pb2.nsfw_pb2_grpc import add_NsfwDetectionProtoServiceServicer_to_server
from services.nsfw_detection_service import NsfwDetectionService

ONE_MB_IN_BYTES = 1048576


def Serve():
    config = ConfigParser()
    config.read('config.dev.ini')

    server = grpc.server(futures.ThreadPoolExecutor(max_workers=config.getint('GrpcServer', 'max_workers')), options=[
        ('grpc.max_receive_message_length', ONE_MB_IN_BYTES * config.getint('GrpcServer', 'max_receive_message_size_mb'))
    ])

    add_NsfwDetectionProtoServiceServicer_to_server(NsfwDetectionService(), server)

    server.add_insecure_port(f"[::]:{config.getint('GrpcServer', 'port')}")
    print(f"Server running on port: {config.getint('GrpcServer','port')}")
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    Serve()

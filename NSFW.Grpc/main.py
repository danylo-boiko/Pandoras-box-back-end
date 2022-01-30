import grpc
from concurrent import futures
from nsfw_pb2.nsfw_pb2_grpc import add_NsfwDetectionProtoServiceServicer_to_server
from services.nsfw_detection_service import NsfwDetectionService

ONE_MB_IN_BYTES = 1048576


def Serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10), options=[
        ('grpc.max_receive_message_length', ONE_MB_IN_BYTES * 100)
    ])
    add_NsfwDetectionProtoServiceServicer_to_server(NsfwDetectionService(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    Serve()

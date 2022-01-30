from nsfw_pb2.nsfw_pb2_grpc import NsfwDetectionProtoServiceServicer
from nsfw_pb2.nsfw_pb2 import *
import opennsfw2 as n2


class NsfwDetectionService(NsfwDetectionProtoServiceServicer):
    def DetectFromVideo(self, request, context):
        def GetVideoResponse(classification, probability_limit):
            return VideoDetectionResponse(**{
                'classification': Classifications.Value(classification),
                'unsafe_frames': [UnsafeFrame(**{
                    'second': second,
                    'nsfw_probability': probability
                }) for second, probability in list(filter(lambda item: item[1] >= probability_limit, predictions))]
            })

        elapsed_seconds, nsfw_probabilities = n2.predict_video_frames(request.video, frame_interval=24)
        predictions = zip(elapsed_seconds, nsfw_probabilities)

        if any(probability >= 0.9 for probability in nsfw_probabilities):
            return GetVideoResponse('Unsafe', 0.9)
        elif any(probability >= 0.6 for probability in nsfw_probabilities):
            return GetVideoResponse('ProbablyUnsafe', 0.6)
        elif any(probability >= 0.3 for probability in nsfw_probabilities):
            return GetVideoResponse('ProbablySafe', 0.3)
        else:
            return GetVideoResponse('Safe', 0.1)

    def DetectFromImage(self, request, context):
        def GetImageResponse(classification, nsfw_probability):
            return ImageDetectionResponse(**{
                'classification': Classifications.Value(classification),
                'nsfw_probability': nsfw_probability
            })

        nsfw_probability = n2.predict_image(request.image)

        if nsfw_probability >= 0.8:
            return GetImageResponse('Unsafe', nsfw_probability)
        elif nsfw_probability >= 0.5:
            return GetImageResponse('ProbablyUnsafe', nsfw_probability)
        elif nsfw_probability >= 0.3:
            return GetImageResponse('ProbablySafe', nsfw_probability)
        else:
            return GetImageResponse('Safe', nsfw_probability)

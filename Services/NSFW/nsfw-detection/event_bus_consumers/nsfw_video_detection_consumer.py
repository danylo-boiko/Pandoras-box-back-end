from events.nsfw_video_detection_event import NsfwVideoDetectionEvent
from enums.classification_status import ClassificationStatus
import json
import opennsfw2 as n2


def get_video_classification(nsfw_probabilities):
    if any(probability >= 0.9 for probability in nsfw_probabilities):
        return ClassificationStatus.Unsafe
    elif any(probability >= 0.6 for probability in nsfw_probabilities):
        return ClassificationStatus.ProbablyUnsafe
    elif any(probability >= 0.3 for probability in nsfw_probabilities):
        return ClassificationStatus.ProbablySafe
    else:
        return ClassificationStatus.Safe


def nsfw_video_detection_consumer(channel, method, properties, body):
    detection_event = NsfwVideoDetectionEvent(**json.loads(body)["message"])

    _, nsfw_probabilities = n2.predict_video_frames(detection_event.video_location, frame_interval=24)
    classification = get_video_classification(nsfw_probabilities)

    print(classification.name)

    channel.basic_ack(delivery_tag=method.delivery_tag)

import pika
import json
import opennsfw2 as n2
from enums.event_bus_exchanger import EventBusExchanger
from enums.event_bus_queue import EventBusQueue
from events.video_classification_status_update_event import VideoClassificationStatusUpdateEvent
from events.nsfw_video_detection_event import NsfwVideoDetectionEvent
from enums.classification_status import ClassificationStatus
from masstransit.create_masstransit_response import create_masstransit_response


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
    request = json.loads(body.decode('UTF-8'))
    detection_event = NsfwVideoDetectionEvent(**request["message"])

    _, nsfw_probabilities = n2.predict_video_frames(detection_event.videoLocation, frame_interval=24)
    classification = get_video_classification(nsfw_probabilities)
    channel.basic_ack(delivery_tag=method.delivery_tag)
    
    status_update_event = VideoClassificationStatusUpdateEvent(detection_event.videoId, detection_event.authorId, classification)
    response = create_masstransit_response(status_update_event, request, EventBusExchanger.VIDEO_CLASSIFICATION_STATUS_UPDATING)

    channel.basic_publish(exchange=EventBusExchanger.VIDEO_CLASSIFICATION_STATUS_UPDATING,
                          routing_key=EventBusQueue.VIDEO_CLASSIFICATION_STATUS_UPDATING,
                          properties=pika.BasicProperties(correlation_id=properties.correlation_id),
                          body=response)

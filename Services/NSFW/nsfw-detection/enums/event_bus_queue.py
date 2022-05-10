from strenum import StrEnum


class EventBusQueue(StrEnum):
    NSFW_VIDEOS_DETECTION = "nswfvideosdetection-queue"
    VIDEO_CLASSIFICATION_STATUS_UPDATING = "videoclassificationstatusupdating-queue"
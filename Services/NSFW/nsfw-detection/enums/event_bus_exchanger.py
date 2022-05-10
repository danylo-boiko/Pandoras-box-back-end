from strenum import StrEnum


class EventBusExchanger(StrEnum):
    INTEGRATION_BASE = "EventBus.Messages.Events:IntegrationBaseEvent"
    NSFW_VIDEOS_DETECTION = "EventBus.Messages.Events:NsfwVideoDetectionEvent"
    VIDEO_CLASSIFICATION_STATUS_UPDATING = "EventBus.Messages.Events:VideoClassificationStatusUpdateEvent"


from events.integration_base_event import IntegrationBaseEvent


class VideoClassificationStatusUpdateEvent(IntegrationBaseEvent):
    def __init__(self, videoId, authorId, classificationStatusCode):
        super().__init__()
        self.videoId = videoId
        self.authorId = authorId
        self.classificationStatusCode = int(classificationStatusCode)

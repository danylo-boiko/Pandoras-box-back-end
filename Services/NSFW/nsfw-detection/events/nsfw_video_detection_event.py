from events.integration_base_event import IntegrationBaseEvent


class NsfwVideoDetectionEvent(IntegrationBaseEvent):
    def __init__(self, id, videoId, authorId, videoLocation, creationDate):
        super().__init__(id, creationDate)
        self.videoId = videoId
        self.authorId = authorId
        self.videoLocation = videoLocation

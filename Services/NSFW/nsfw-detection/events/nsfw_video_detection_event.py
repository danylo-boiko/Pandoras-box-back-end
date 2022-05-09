class NsfwVideoDetectionEvent:
    def __init__(self, id, videoId, authorId, videoLocation, creationDate):
        self.id = id
        self.video_id = videoId
        self.author_id = authorId
        self.video_location = videoLocation
        self.creation_date = creationDate

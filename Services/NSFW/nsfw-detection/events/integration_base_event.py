from uuid import uuid4
from datetime import datetime


class IntegrationBaseEvent:
    def __init__(self, id=str(uuid4()), creationDate=datetime.now().isoformat()):
        self.id = id
        self.creationDate = creationDate

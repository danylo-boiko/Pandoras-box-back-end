from enum import Enum


class ClassificationStatus(Enum):
    UnReviewed = 0
    Safe = 1
    ProbablySafe = 2
    ProbablyUnsafe = 3
    Unsafe = 4

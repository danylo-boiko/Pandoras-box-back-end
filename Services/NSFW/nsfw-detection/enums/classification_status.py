from enum import IntEnum


class ClassificationStatus(IntEnum):
    UnReviewed = 0
    Safe = 1
    ProbablySafe = 2
    ProbablyUnsafe = 3
    Unsafe = 4

"""
Inference utilities.
"""
import io
import tempfile
from typing import List, Optional, Sequence, Tuple

import cv2  # type: ignore
import numpy as np
from PIL import Image  # type: ignore

from ._download import get_default_weights_path
from ._image import preprocess_image, Preprocessing
from ._inspection import make_and_save_nsfw_grad_cam
from ._model import make_open_nsfw_model


def predict_image(
        image_data: bytes,
        preprocessing: Preprocessing = Preprocessing.YAHOO,
        weights_path: Optional[str] = get_default_weights_path(),
        grad_cam_path: Optional[str] = None,
        grad_cam_height: int = 512,
        grad_cam_width: int = 512,
        alpha: float = 0.5
) -> float:
    """
    Pipeline from single image path to predicted NSFW probability.
    Optionally generate and save the Grad-CAM plot.
    """
    pil_image = Image.open(io.BytesIO(image_data))
    image = preprocess_image(pil_image, preprocessing)
    model = make_open_nsfw_model(weights_path=weights_path)
    nsfw_probability = float(
        model.predict(np.expand_dims(image, 0), batch_size=1)[0][1]
    )

    if grad_cam_path is not None:
        make_and_save_nsfw_grad_cam(
            pil_image, preprocessing, model, grad_cam_path,
            grad_cam_height, grad_cam_width, alpha
        )

    return nsfw_probability


def predict_video_frames(
        video_data: bytes,
        frame_interval: int = 8,
        output_video_path: Optional[str] = None,
        preprocessing: Preprocessing = Preprocessing.YAHOO,
        weights_path: Optional[str] = get_default_weights_path()
) -> Tuple[List[float], List[float]]:
    """
    Make prediction for each video frame.
    """

    with tempfile.NamedTemporaryFile(mode='wb', delete=True) as video:
        video.write(video_data)

        cap = cv2.VideoCapture(video.name)  # pylint: disable=no-member
        fps = cap.get(cv2.CAP_PROP_FPS)  # pylint: disable=no-member

        model = make_open_nsfw_model(weights_path=weights_path)

        video_writer: Optional[cv2.VideoWriter] = None  # pylint: disable=no-member
        nsfw_probability = 0.0
        nsfw_probabilities: List[float] = []
        frame_count = 0

        while cap.isOpened():
            ret, bgr_frame = cap.read()  # Get next video frame.
            if not ret:
                break  # End of given video.

            frame_count += 1
            frame = cv2.cvtColor(bgr_frame, cv2.COLOR_BGR2RGB)  # pylint: disable=no-member

            if video_writer is None and output_video_path is not None:
                video_writer = cv2.VideoWriter(  # pylint: disable=no-member
                    output_video_path,
                    cv2.VideoWriter_fourcc(*"mp4v"),  # pylint: disable=no-member
                    fps, (frame.shape[1], frame.shape[0])
                )

            if frame_count == 1 or (frame_count + 1) % frame_interval == 0:
                pil_frame = Image.fromarray(frame)
                input_frame = preprocess_image(pil_frame, preprocessing)
                predictions = model.predict(np.expand_dims(input_frame, axis=0), 1)
                nsfw_probability = np.round(predictions[0][1], 2)

            nsfw_probabilities.append(nsfw_probability)

            if video_writer is not None:
                result_text = f"NSFW probability: {str(nsfw_probability)}"
                # RGB colour.
                colour = (255, 0, 0) if nsfw_probability >= 0.8 else (0, 0, 255)
                cv2.putText(  # pylint: disable=no-member
                    frame, result_text, (10, 30),
                    cv2.FONT_HERSHEY_SIMPLEX,  # pylint: disable=no-member
                    1, colour, 2, cv2.LINE_AA  # pylint: disable=no-member
                )
                video_writer.write(
                    cv2.cvtColor(frame, cv2.COLOR_RGB2BGR)  # pylint: disable=no-member
                )

        if video_writer is not None:
            video_writer.release()
        cap.release()
        cv2.destroyAllWindows()  # pylint: disable=no-member

        elapsed_seconds = (np.arange(1, len(nsfw_probabilities) + 1) / fps).tolist()
        return elapsed_seconds, nsfw_probabilities

import pika
from configparser import ConfigParser
from enums.event_bus_queue import EventBusQueue
from enums.event_bus_exchanger import EventBusExchanger
from event_bus_consumers.nsfw_video_detection_consumer import nsfw_video_detection_consumer


def create_rabbitmq_channel(config):
    host = config.get('RabbitMQ', 'host')
    port = config.getint('RabbitMQ', 'port')

    connection_parameters = pika.ConnectionParameters(host=host, port=port)
    connection = pika.BlockingConnection(connection_parameters)
    return connection.channel()


def setup_channel_queues(channel):
    channel.queue_declare(queue=EventBusQueue.NSFW_VIDEOS_DETECTION)
    channel.queue_bind(queue=EventBusQueue.NSFW_VIDEOS_DETECTION, exchange=EventBusExchanger.NSFW_VIDEOS_DETECTION)
    channel.basic_consume(queue=EventBusQueue.NSFW_VIDEOS_DETECTION, on_message_callback=nsfw_video_detection_consumer)


def main():
    config = ConfigParser()
    config.read('config.ini')

    channel = create_rabbitmq_channel(config)
    setup_channel_queues(channel)

    print("Start consuming")
    channel.start_consuming()


if __name__ == '__main__':
    main()

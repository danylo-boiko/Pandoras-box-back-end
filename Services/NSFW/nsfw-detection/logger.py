import logging
import sys
from configparser import ConfigParser
from cmreslogging.handlers import CMRESHandler


def get_es_connection_info():
    config = ConfigParser()
    config.read('config.ini')

    host = config.get('Elasticsearch', 'host')
    port = config.getint('Elasticsearch', 'port')

    return host, port


def setup_logger():
    stdout_handler = logging.StreamHandler(sys.stdout)
    stdout_handler.setLevel(logging.DEBUG)

    host, port = get_es_connection_info()
    es_handler = CMRESHandler(hosts=[{'host': host, 'port': port}],
                              auth_type=CMRESHandler.AuthType.NO_AUTH,
                              es_index_name=f"pandoras-box-log-nsfw-detection-development",
                              es_additional_fields={'application': 'nsfw-detection', 'environment': 'Development'})
    es_handler.setLevel(logging.DEBUG)

    logging.basicConfig(
        level=logging.INFO,
        handlers=[
            stdout_handler,
            es_handler
        ]
    )

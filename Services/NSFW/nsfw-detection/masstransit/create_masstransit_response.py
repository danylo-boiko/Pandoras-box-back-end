import json


def create_masstransit_response(result, request_body, exchanger_type):
    response = {
        'requestId': request_body['requestId'],
        'correlationId': request_body['correlationId'],
        'conversationId': request_body['conversationId'],
        'initiatorId': request_body['correlationId'],
        'sourceAddress': request_body['destinationAddress'],
        'destinationAddress': request_body['sourceAddress'],
        'messageType': [
            f'urn:message:{exchanger_type}',
            'urn:message:EventBus.Messages.Events:IntegrationBaseEvent'
        ],
        'message': result.__dict__
    }

    return json.dumps(response)

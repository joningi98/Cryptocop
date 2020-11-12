import pika
import json
from credit_card_checker import CreditCardChecker

connection = pika.BlockingConnection(pika.ConnectionParameters('cryptocop_rabbit'))
channel = connection.channel()
exchangeKey = 'cryptocop'
routingKey = 'create-order'
paymentQueue = 'payment-queue'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchangeKey, exchange_type='fanout')

# Declare the queue, if it doesn't exist
channel.queue_declare(queue=paymentQueue,
                      durable=True,
                      exclusive=False,
                      auto_delete=False,
                      arguments=None)

# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchangeKey,
                   queue=paymentQueue,
                   routing_key=routingKey)


def checkCreditCard(ch, method, properties, data):
    parsed_msg = json.loads(data)
    card = parsed_msg['CreditCard']
    if CreditCardChecker(card).valid():
        print('valid card')
    else:
        print('invalid')


channel.basic_consume(on_message_callback=checkCreditCard,
                      queue=paymentQueue,
                      auto_ack=True)

channel.start_consuming()
connection.close()

import pika
import requests
import json

connection = pika.BlockingConnection(pika.ConnectionParameters('cryptocop_rabbit'))
channel = connection.channel()
exchangeKey = 'cryptocop'
routingKey = 'create-order'
emailQueue = 'email-queue'
emailTemplate = '<h2>Thank you for ordering @ Cryptocop!</h2><p>We hope you will enjoy our lovely product and don\'t ' \
                'hesitate to contact us if there are any questions.</p>%s'

itemsTamplate = '<table><b>Cart</b><thead><tr style="background-color: ' \
                'rgba(155, 155, 155, .2)"><th>Product ID</th><th>Quantity</th><th>Unit Price</th><th>Row ' \
                'price</th></tr></thead><tbody>%s</tbody></table> '

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchangeKey, exchange_type='fanout')

# Declare the queue, if it doesn't exist
channel.queue_declare(queue=emailQueue,
                      durable=True,
                      exclusive=False,
                      auto_delete=False,
                      arguments=None)

# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchangeKey,
                   queue=emailQueue,
                   routing_key=routingKey)


def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandbox0f575595142f4720bfe4a289db3dbe79.mailgun.org/messages",
        auth=("api", "635250d79e3f6bb15024d5a0dae71f5f-ea44b6dc-6259d89b"),
        data={"from": "Mailgun Sandbox <postmaster@sandbox8ed42b90d7ca4428b06ec0c4424c4aff.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})


def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)
    email = parsed_msg['Email']

    user_html = "<p>%s</p><p>%s</p><p>%s</p><p>%s</p><p>%s</p><p>%s</p><p>%s</p>" % \
                (parsed_msg['FullName'], parsed_msg['StreetName'], parsed_msg['HouseNumber'],
                 parsed_msg['City'], parsed_msg['ZipCode'], parsed_msg['Country'], parsed_msg['OrderDate'])

    items = parsed_msg['OrderItems']
    items_html = ''.join(['<tr><td>%s</td><td>%.2f</td><td>%.2f</td><td>%.2f</td></tr>' % (
        item['ProductIdentifier'], item['UnitPrice'], item['Quantity'], item['TotalPrice']) for
                          item
                          in items])

    representation = (emailTemplate % user_html) + (itemsTamplate % items_html)

    send_simple_message(email, 'Crypto order', representation)


channel.basic_consume(on_message_callback=send_order_email,
                      queue=emailQueue,
                      auto_ack=True)

channel.start_consuming()
connection.close()

#!/usr/bin/env python3
import pika

credentials = pika.PlainCredentials('guest', 'guest')
connection = pika.BlockingConnection(pika.ConnectionParameters(host="localhost", port=5672,
                                                               virtual_host="/",
                                                               credentials=credentials))

q_name = "mixing"
channel = connection.channel()

def callback(ch, method, properties, body):
    print('{} {} {}'.format(body, properties, method))
    ch.basic_ack(delivery_tag=method.delivery_tag)


channel.basic_qos(prefetch_count=100)  # mandatory
channel.basic_consume(
    queue=q_name,
    on_message_callback=callback,
    arguments={
        'x-stream-offset': 'first'  # here you can specify the offset: : first, last, next, and timestamp
        # with first start consuming always from the beginning
    },
    auto_ack=False)
channel.start_consuming()

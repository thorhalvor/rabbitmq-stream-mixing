#!/usr/bin/env python3
import pika
import time


print(" Starting AMQP Producer'")

credentials = pika.PlainCredentials('guest', 'guest')
connection = pika.BlockingConnection(pika.ConnectionParameters(host="localhost", port=5672,
                                                               virtual_host="/",
                                                               credentials=credentials))
numberOfMessages = 100
q_name = "mixing"
channel = connection.channel()

print(" Sending {} standard messages to queue '{}'".format(numberOfMessages, q_name))

for i in range(numberOfMessages):
    channel.basic_publish(exchange='', routing_key=q_name, body='AMQP python message {}'.format(i))

print(" Sending {} messages with properties to queue '{}'".format(numberOfMessages, q_name))

for i in range(numberOfMessages):
    prop = pika.BasicProperties(
        content_type='application/json',
        content_encoding='utf-8',
        headers={'key': 'value'},
        delivery_mode=1,
    )
    channel.basic_publish(exchange='', routing_key=q_name, body='AMQP python message {}'.format(i), properties=prop)


time.sleep(1)
channel.close()
connection.close()

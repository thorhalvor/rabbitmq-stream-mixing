#!/usr/bin/env python3
from threading import Thread
import time  # for sleep

# -*- coding: utf-8 -*-
# pylint: disable=C0111,C0103,R0205

import functools
import logging
import threading
import time
import pika
from pika.exchange_type import ExchangeType

LOG_FORMAT = ('%(levelname) -10s %(asctime)s %(name) -30s %(funcName) '
              '-35s %(lineno) -5d: %(message)s')
LOGGER = logging.getLogger(__name__)

logging.basicConfig(level=logging.INFO, format=LOG_FORMAT)

q_name = "mixing"


def ack_message(ch, delivery_tag):
    """Note that `ch` must be the same pika channel instance via which
    the message being ACKed was retrieved (AMQP protocol constraint).
    """
    if ch.is_open:
        ch.basic_ack(delivery_tag)
    else:
        # Channel is already closed, so we can't ACK this message;
        # log and/or do something that makes sense for your app in this case.
        pass


def do_work(ch, delivery_tag, body):
    thread_id = threading.get_ident()
    LOGGER.info('Thread id: %s Delivery tag: %s Message body: %s', thread_id,
                delivery_tag, body)
    # Sleeping to simulate 10 seconds of work
    cb = functools.partial(ack_message, ch, delivery_tag)
    ch.connection.add_callback_threadsafe(cb)


def on_message(ch, method, properties, body, args):
    if ch.is_open:
        print('{} {} {}'.format(body, properties, method))
        ch.basic_ack(delivery_tag=method.delivery_tag)


credentials = pika.PlainCredentials('guest', 'guest')
# Note: sending a short heartbeat to prove that heartbeats are still
# sent even though the worker simulates long-running work
parameters = pika.ConnectionParameters(
    'localhost', credentials=credentials, heartbeat=5)
connection = pika.BlockingConnection(parameters)

channel = connection.channel()
channel.basic_qos(prefetch_count=1)

threads = []
on_message_callback = functools.partial(on_message, args=(threads))
channel.basic_consume(on_message_callback=on_message_callback, queue=q_name,
                      arguments={
                          'x-stream-offset': 'first'
                          # here you can specify the offset: : first, last, next, and timestamp
                      },
                      auto_ack=False)

try:
    channel.start_consuming()
except KeyboardInterrupt:
    channel.stop_consuming()

# Wait for all to complete
for thread in threads:
    thread.join()

connection.close()

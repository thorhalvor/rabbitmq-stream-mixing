import pika

connection = pika.BlockingConnection()
channel = connection.channel()
channel.basic_qos(prefetch_count=100)  # mandatory

q_name = "mixing"
# Get ten messages and break out
for method, properties, body in channel.consume(q_name, inactivity_timeout=2, auto_ack=False, arguments={
    'x-stream-offset': 'first'  # here you can specify the offset: : first, last, next, and timestamp

}):
    if channel.is_open:
        print('{} {} {}'.format(body, properties, method))
        try:
            channel.basic_ack(method.delivery_tag)
        except AttributeError as e:
            break
requeued_messages = channel.cancel()

# Close the channel and the connection
channel.close()
connection.close()

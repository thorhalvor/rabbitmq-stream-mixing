import asyncio
import rbfly.streams as rbs
from rbfly.streams._client import PublisherTrait, Publisher, PublisherBatch, \
    PublisherBatchMem, PublisherBin, PublisherBinBatch
from rbfly.amqp._message import MessageCtx, encode_amqp




async def publish():

    MESSAGES = 100
    STREAM_NAME= "mixing"

   
    client = rbs.streams_client("rabbitmq-stream://guest:guest@localhost:5552/")
    
    await client.create_stream("mixing");

    # sendint 100 sinple messages with application_properties
    async with client.publisher("mixing", name="test") as pub:

        for n in range(100):
        
            message = 'message sent with python_rbfly'
            #message =    MessageCtx(b'hello')
            #buffer = bytearray()
            #encode_amqp(buffer, message)


            await pub.send(message)
            #await pub.send(message)

    # sendint 100 sinple messages with application_properties
    #async with client.publisher("mixing", name="test") as pub:
    #    for n in range(100):
       

    #        application_properties = {
    #            "key_string": "values",
    #             "key2_int": "1111",
    #             "key2_decimal": 10_000_000_000
    #         }
    #        message = MessageCtx(b'3210000123', app_properties=application_properties)
    #        buffer = bytearray()
    #        encode_amqp(buffer, message)

    #        await pub.send(buffer)

    #print(client._publishers)
    #pub = client._publishers[1]
  
    #pub = Publisher(client, 'testXXXX', 2, 'test', 12)
       



asyncio.run(publish())
#client.connect()

  
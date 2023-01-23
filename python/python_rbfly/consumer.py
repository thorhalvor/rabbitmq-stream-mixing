import asyncio
import rbfly.streams as rbs
from rbfly.streams._client import PublisherTrait, Publisher, PublisherBatch, \
    PublisherBatchMem, PublisherBin, PublisherBinBatch
from rbfly.amqp._message import MessageCtx, decode_amqp, get_message_ctx
from rbfly.streams.offset import Offset, OffsetType


async def consume():
   
    client = rbs.streams_client("rabbitmq-stream://guest:guest@localhost:5552/")
    
    # sendint 100 sinple messages with application_properties
    messages =  client.subscribe("mixing",  offset=Offset.reference('mixing'))
    async for msg in messages:
        print(msg)  

    await consumer.start()
    await consumer.run()
    exit()

   
# main coroutine
async def main():
    # schedule the task
    task = asyncio.create_task(consume())
    # suspend a moment
      # wait a moment
    await asyncio.sleep(5)
    # cancel the task
    was_cancelled = task.cancel()
   
    # report a message
    print('Main done')
    exit()
 
# run the asyncio program
asyncio.run(main())



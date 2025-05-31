import pika
import json
import logging
import time
from typing import Dict, Any, Callable

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

from config import HOST, RESPONSE_QUEUE, USER_NAME, PASSWORD, REQUEST_QUEUE

# In my case  just have queue for process data to embedding vector and send back to .net server message to queue response


class EventBusRabbitMQ:
    def __init__(self):
        self.connection = None
        self.chanel = None
        self._connection()

    def _connect(self):
        """Kết nối RabbitMQ với retry"""
        for attempt in range(3):
            try:
                credentials = pika.PlainCredentials(USER_NAME, PASSWORD)
                self.connection = pika.BlockingConnection(
                    pika.ConnectionParameters(HOST, credentials=credentials)
                )
                self.channel = self.connection.channel()

                # Tạo queue durable (không mất khi restart)
                self.channel.queue_declare(queue=REQUEST_QUEUE, durable=True)
                self.channel.queue_declare(queue=RESPONSE_QUEUE, durable=True)

                logger.info("Kết nối RabbitMQ thành công")
                return
            except Exception as e:
                logger.error(f"Lần thử {attempt + 1}: {e}")
                if attempt < 2:
                    time.sleep(2)
        raise Exception("Không thể kết nối RabbitMQ")

    def _ensure_connection(self):
        """Đảm bảo connection còn sống"""
        if not self.connection or self.connection.is_closed:
            self._connect()

    def publish(self, queue: str, data: Dict[Any, Any]) -> bool:
        """Gửi message an toàn"""
        try:
            self._ensure_connection()

            message = json.dumps(data, ensure_ascii=False)

            # Gửi với persistent (không mất khi restart)
            self.channel.basic_publish(
                exchange='',
                routing_key=queue,
                body=message,
                properties=pika.BasicProperties(delivery_mode=2)
            )

            logger.info(f"Đã gửi: {data}")
            return True

        except Exception as e:
            logger.error(f"Lỗi gửi message: {e}")
            return False

    def consume(self, queue: str, callback: Callable):
        """Nhận message an toàn"""
        try:
            self._ensure_connection()

            # Chỉ nhận 1 message tại 1 thời điểm
            self.channel.basic_qos(prefetch_count=1)

            def safe_callback(ch, method, properties, body):
                try:
                    # Parse message
                    data = json.loads(body.decode('utf-8'))
                    logger.info(f"📨 Nhận: {data}")

                    # Xử lý message
                    success = callback(data)

                    if success:
                        # Xác nhận đã xử lý thành công
                        ch.basic_ack(delivery_tag=method.delivery_tag)
                        logger.info("Xử lý thành công")
                    else:
                        # Từ chối và đưa lại queue để thử lại
                        ch.basic_nack(delivery_tag=method.delivery_tag, requeue=True)
                        logger.warning(" Xử lý thất bại, thử lại")

                except Exception as e:
                    logger.error(f"Lỗi xử lý: {e}")
                    # Từ chối và thử lại
                    ch.basic_nack(delivery_tag=method.delivery_tag, requeue=True)

            # Bắt đầu lắng nghe (không tự động xác nhận)
            self.channel.basic_consume(
                queue=queue,
                on_message_callback=safe_callback,
                auto_ack=False
            )

            logger.info(f"Đang lắng nghe queue: {queue}")
            logger.info("Nhấn Ctrl+C để dừng")

            self.channel.start_consuming()

        except KeyboardInterrupt:
            logger.info("Dừng lắng nghe")
            self.channel.stop_consuming()
        except Exception as e:
            logger.error(f"Lỗi consume: {e}")

    def close(self):
        """Đóng kết nối"""
        if self.connection and not self.connection.is_closed:
            self.connection.close()
            logger.info("✅ Đã đóng kết nối")
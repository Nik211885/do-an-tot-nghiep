import re
import torch
import numpy as np
import json
from typing import List, Dict, Any, Optional
from transformers import AutoTokenizer, RobertaModel
import logging
from datetime import datetime, timezone

# Tắt warning để chạy nhanh hơn
logging.getLogger("transformers").setLevel(logging.ERROR)


class PhoBERTChunker:
    def __init__(self, model_name="vinai/phobert-base", device=None):
        """
        PhoBERT Chunker trả về format DocSource đơn giản
        """
        # Auto detect device
        if device is None:
            self.device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
        else:
            self.device = torch.device(device)

        print(f"🚀 Sử dụng device: {self.device}")

        # Load model và tokenizer
        print("📥 Đang load PhoBERT...")
        self.tokenizer = AutoTokenizer.from_pretrained(model_name)
        self.model = RobertaModel.from_pretrained(model_name)
        self.model.to(self.device)
        self.model.eval()

        # Cấu hình
        self.max_tokens = 200
        self.batch_size = 8
        self.model_name = model_name

        print("✅ PhoBERT đã sẵn sàng!")

    def count_tokens(self, text: str) -> int:
        """Đếm tokens nhanh"""
        return len(self.tokenizer.encode(text, add_special_tokens=True, truncation=True))

    def split_sentences(self, text: str) -> List[str]:
        """Tách câu nhanh với regex tối ưu"""
        pattern = r'(?<=[.!?])\s+'
        sentences = re.split(pattern, text.strip())
        return [s.strip() for s in sentences if s.strip()]

    def create_chunks(self, text: str) -> List[str]:
        """Tạo chunks thông minh và nhanh"""
        text = re.sub(r'\s+', ' ', text.strip())
        sentences = self.split_sentences(text)

        chunks = []
        current_chunk = ""

        for sentence in sentences:
            test_chunk = f"{current_chunk} {sentence}".strip()

            if self.count_tokens(test_chunk) <= self.max_tokens:
                current_chunk = test_chunk
            else:
                if current_chunk:
                    chunks.append(current_chunk)

                if self.count_tokens(sentence) <= self.max_tokens:
                    current_chunk = sentence
                else:
                    sub_chunks = self._split_long_sentence(sentence)
                    chunks.extend(sub_chunks[:-1])
                    current_chunk = sub_chunks[-1] if sub_chunks else ""

        if current_chunk:
            chunks.append(current_chunk)

        return chunks

    def _split_long_sentence(self, sentence: str) -> List[str]:
        """Cắt câu dài theo dấu phẩy"""
        parts = re.split(r'[,;]', sentence)
        chunks = []
        current = ""

        for part in parts:
            part = part.strip()
            test = f"{current}, {part}".strip(', ')

            if self.count_tokens(test) <= self.max_tokens:
                current = test
            else:
                if current:
                    chunks.append(current)
                current = part

        if current:
            chunks.append(current)

        return chunks

    def get_batch_embeddings(self, texts: List[str]) -> List[List[float]]:
        """Tạo embeddings cho nhiều text cùng lúc"""
        if not texts:
            return []

        try:
            inputs = self.tokenizer(
                texts,
                return_tensors="pt",
                truncation=True,
                max_length=256,
                padding=True
            )

            inputs = {k: v.to(self.device) for k, v in inputs.items()}

            with torch.no_grad():
                outputs = self.model(**inputs)
                embeddings = outputs.last_hidden_state[:, 0, :]

            return embeddings.cpu().numpy().tolist()

        except Exception as e:
            print(f"❌ Lỗi batch embedding: {e}")
            return []

    def process_text(self, text: str) -> List[Dict[str, Any]]:
        """
        Main function: Chỉ cần truyền text vào

        Args:
            text: Text cần chunk

        Returns:
            List of DocSource dictionaries
        """
        print("🔄 Đang chunk text...")
        chunks = self.create_chunks(text)

        print(f"📝 Tạo được {len(chunks)} chunks")
        print("🧠 Đang tạo embeddings...")

        # Tạo embeddings batch
        embeddings = []
        for i in range(0, len(chunks), self.batch_size):
            batch = chunks[i:i + self.batch_size]
            batch_embeddings = self.get_batch_embeddings(batch)
            embeddings.extend(batch_embeddings)
            print(f"   ✅ Xử lý batch {i // self.batch_size + 1}/{(len(chunks) - 1) // self.batch_size + 1}")

        # Tạo DocSource format
        doc_sources = []
        timestamp = datetime.now(timezone.utc)

        for chunk, embedding in zip(chunks, embeddings):
            doc_source = {
                "content": chunk,
                "embedding": embedding,
                "embedding_dim": len(embedding) if embedding else 0,
                "token_count": self.count_tokens(chunk),
                "word_count": len(chunk.split()),
                "char_count": len(chunk),
                "created_at": timestamp.isoformat(),
                "model_name": self.model_name
            }
            doc_sources.append(doc_source)

        print("✅ Hoàn thành!")
        return doc_sources

    def process_text_to_json(self, text: str) -> str:
        """
        Trả về JSON string của DocSource list
        """
        doc_sources = self.process_text(text)
        return json.dumps(doc_sources, ensure_ascii=False, indent=2)


def main():
    """Demo function"""
    print("🚀 PhoBERT Simple Chunker Demo")
    print("=" * 50)

    chunker = PhoBERTChunker()

    # Sample text
    sample_text = """
    Trí tuệ nhân tạo (AI) đang thay đổi cách chúng ta làm việc và sống. 
    Công nghệ này không chỉ giúp tự động hóa các tác vụ lặp đi lặp lại mà còn 
    hỗ trợ ra quyết định phức tạp. Trong lĩnh vực y tế, AI giúp chẩn đoán bệnh 
    chính xác hơn. Trong giáo dục, AI cá nhân hóa trải nghiệm học tập.

    Việt Nam đang đầu tư mạnh vào phát triển AI. Nhiều trường đại học đã mở 
    các chương trình đào tạo về AI và machine learning. Các công ty công nghệ 
    trong nước cũng đang phát triển những sản phẩm AI tiên tiến.
    """

    import time
    start_time = time.time()

    # Process text - chỉ cần gửi text lên
    doc_sources = chunker.process_text(sample_text)

    end_time = time.time()
    print(f"\n⏱️  Thời gian xử lý: {end_time - start_time:.2f}s")
    print(f"📊 Tổng số chunks: {len(doc_sources)}")

    # Show sample results
    print("\n📋 Sample DocSource results:")
    for i, doc_source in enumerate(doc_sources):
        print(f"\nChunk {i + 1}:")
        print(f"  Content: {doc_source['content'][:100]}...")
        print(f"  Token count: {doc_source['token_count']}")
        print(f"  Word count: {doc_source['word_count']}")
        print(f"  Char count: {doc_source['char_count']}")
        print(f"  Embedding dim: {doc_source['embedding_dim']}")
        print(f"  Model: {doc_source['model_name']}")
        print(f"  Created at: {doc_source['created_at']}")

    # JSON format
    print("\n📤 JSON format (first 500 chars):")
    json_result = chunker.process_text_to_json(sample_text)
    print(json_result[:500] + "...")

    print(f"\n📊 Tổng kích thước JSON: {len(json_result):,} bytes")


# Sử dụng đơn giản:
# chunker = PhoBERTChunker()
# results = chunker.process_text("Your text here")
# json_results = chunker.process_text_to_json("Your text here")

if __name__ == "__main__":
    main()
import re

def split_long_sentence(sentence, max_tokens=512):
    words = sentence.split()
    if len(words) <= max_tokens:
        return [sentence]
    chunks = []
    for i in range(0, len(words), max_tokens):
        chunk = ' '.join(words[i:i + max_tokens])
        chunks.append(chunk)
    return chunks

def sentences_text_with_token_limit(text, max_tokens=512):
    # Tách câu theo dấu câu
    sentences = re.split(r'(?<=[.!?])\s+', text.strip())
    result = []
    for sentence in sentences:
        # Chia nhỏ câu dài nếu cần
        split_sentences = split_long_sentence(sentence, max_tokens)
        result.extend(split_sentences)
    return result



if __name__ == '__main__':
    print(sentences_text_with_token_limit('Hôm nay trời đẹp. Tôi đi học. Bạn có khỏe không?'))
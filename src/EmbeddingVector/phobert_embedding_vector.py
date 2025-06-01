import torch
from transformers import AutoTokenizer, RobertaModel

tokenizer = AutoTokenizer.from_pretrained("vinai/phobert-base")
model = RobertaModel.from_pretrained("vinai/phobert-base")

def transform_vector_embedding(sentence):
    inputs = tokenizer(sentence, return_tensors="pt")

    with torch.no_grad():
        outputs = model(**inputs)
        last_hidden_state = outputs.last_hidden_state

    sentence_embedding = last_hidden_state[:, 0, :]
    return sentence_embedding.cpu().numpy().tolist()

if __name__ == '__main__':
    vector_embedding = transform_vector_embedding('Le Khac Ninh')
    print(f'Vector embdding for author Le Khac Ninh is {vector_embedding}')
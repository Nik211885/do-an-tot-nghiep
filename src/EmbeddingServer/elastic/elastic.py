from config import URI
from elasticsearch import  Elasticsearch
from numpy import ndarray

class ElasticClient:
    def __init__(self):
        self.es = Elasticsearch(URI)
    def index_embedding(self, doc_id: str, content: str, embedding_vector: ndarray, index_name: str = 'vector_doc'):
        embedding_list = embedding_vector.flatten().tolist()
        doc = {
            "content": content,
            "embedding":embedding_list
        }
        resp = self.es.index(index = index_name, id = doc_id, document = doc);
        return resp

if __name__ == "__main__":
    elastic = ElasticClient()
    elastic.index_embedding('id_01','content', [1,0,1,0.2])
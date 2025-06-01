from flask import Flask, request, jsonify, redirect
from flasgger import Swagger
from phobert_embedding_vector import transform_vector_embedding

app = Flask(__name__)
swagger = Swagger(app)

@app.route("/")
def say_hello():
    return redirect('/apidocs', code=302)

@app.route('/phobert', methods=['POST'])
def transform_vector():
    """
    Get vector embedding using PhoBERT
    ---
    parameters:
      - name: sentence
        in: formData
        type: string
        required: true
        description: The sentence to transform
    responses:
      200:
        description: A JSON object with the vector embedding
    """
    sentence = request.form.get("sentence")
    if not sentence:
        return jsonify({"error": "Missing 'sentence'"}), 400
    return jsonify(transform_vector_embedding(sentence))

if __name__ == '__main__':
    app.run(debug=True)

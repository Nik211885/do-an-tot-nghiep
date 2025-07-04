from flask import Flask, request, jsonify, redirect
from flasgger import Swagger
from jina_vector_embedding import SimpleTextChunkerJina

app = Flask(__name__)
swagger = Swagger(app)

@app.route("/")
def say_hello():
    return redirect('/apidocs', code=302)

@app.route('/jina', methods=['POST'])
def transform_vector():
    """
    Get vector embedding using PhoBERT
    ---
    parameters:
      - name: body
        in: body
        schema:
          type: object
          properties:
            sentence:
              type: string
        required: true
        description: JSON object with sentence and doc_id
    responses:
      200:
        description: A JSON object with the vector embedding
    """
    data = request.get_json()
    if not data or 'sentence' not in data:
        return jsonify({"error": "Missing 'sentence' or 'doc_id'"}), 400

    sentence = data['sentence']

    jina = SimpleTextChunkerJina()
    results = jina.process_data(sentence)
    return jsonify(results)


if __name__ == '__main__':
    app.run(debug=True)

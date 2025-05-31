from flask import Flask
from hello_word import say_hello

app = Flask(__name__)

@app.route('health-check')
def health_check():
    return "health, good!"

@app.route('/')
def hello():
    return f"{say_hello()} my name is Ninh"
@app.route('/tranform')
def shape_tranform():
    return "tranform"

if __name__ == '__main__':
    app.run(debug=True)

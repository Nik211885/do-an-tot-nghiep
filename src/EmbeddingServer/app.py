from flask import Flask
from hello_word import say_hello


app = Flask(__name__)

@app.route('/')
def hello():
    return f"{say_hello()} my name is Ninh"

if __name__ == '__main__':
    app.run(debug=True)

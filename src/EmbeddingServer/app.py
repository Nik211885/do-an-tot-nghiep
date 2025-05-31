from flask import Flask, jsonify
from hello_word import say_hello
from tranform_vector_embedding import tranform_vector_embedding;

app = Flask(__name__)

@app.route('/')
def hello():
    return f"{say_hello()} my name is Ninh"
@app.route('/tranform')
def shape_tranform():
    embedding = tranform_vector_embedding("""
    Cách đây 26 năm tôi có viết một truyện dài dựa theo cuộc đời của anh thương binh Phan Thành Lợi. Đây là sáng kiến của bà Đỗ Duy Liên, lúc ấy là phó chủ tịch UBND Thành phố HCM.

Anh Lợi quê ở Củ Chi, là một thương binh bị cụt cả hai tay, hai chân. Lúc đó anh được cấp một căn hộ nhỏ trong "làng phế binh" Thủ Đức.

Tôi lui tới làm việc với anh trong vài tháng và viết xong một truyện dài lấy tên là Qua Sông. Tác phẩm được nhà xuất bản Văn Nghệ in năm 1986 với số lượng là 10.150 cuốn, khổ 13x19cm.

Hồi đó sách in bằng giấy đen, sần sùi, trông rất xấu xí, nhưng vẫn không đủ sách để mà bán.
    """)
    return jsonify(embedding=embedding.tolist())

if __name__ == '__main__':
    app.run(debug=True)
